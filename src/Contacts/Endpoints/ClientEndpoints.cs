namespace ContactsApi.Endpoints;

public static class ClientEndpoints
{
	public static RouteGroupBuilder MapClientEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", GetAllClients);
		group.MapGet("/{id}", FindById);
		group.MapPost("/", AddClient)
			.AddEndpointFilter<ValidationFilter<ClientRequest>>();
		group.MapPut("/", UpdateClient)
			.AddEndpointFilter<ValidationFilter<ClientRequestEdit>>();
		group.MapDelete("/{id}", DeleteClient);

		return group;
	}

	public static async Task<Ok<List<ClientResponse>>> GetAllClients(ICacheService cache, IClientRepository repository, [FromQuery(Name = "dddId")] Guid? dddId)
	{
		var key = $"clients-{dddId.ToString() ?? ""}";
		var cachedClient = cache.Get(key);

		if (cachedClient is not null)
			return TypedResults.Ok(cachedClient as List<ClientResponse>);

		var result = await repository.FindAll();

		if (dddId is not null)
			result = result.Where(e => e.DddId.Equals(dddId)).ToList();

		var response = result.ToResponseList();

		cache.Set(key, response);
		return TypedResults.Ok(response);
	}

	public static async Task<Results<Ok<ClientResponse>, NotFound<string>>> FindById(IClientRepository repository, Guid id)
		=> await repository.FindById(id)
			is { } client
				? TypedResults.Ok(client.ToResponse())
				: TypedResults.NotFound("Client not found");

	public static async Task<Created> AddClient(ICacheService cache, IClientRepository repository, IDddRepository dddRepository, [FromBody] ClientRequest clientRequest)
	{
		var client = await VerifyDddAndReturnEntity(dddRepository, clientRequest);

		repository.Add(client);
		ClearCache(cache, client.DddId);
		return TypedResults.Created();
	}

	public static async Task<Ok> UpdateClient(ICacheService cache, IClientRepository repository, [FromBody] ClientRequestEdit clientRequest)
	{
		var client = await repository.FindById(clientRequest.Id)
			?? throw new ArgumentException("Please inform an valid client");

		client.Name = clientRequest.Name;
		client.Telephone = clientRequest.Telephone;
		client.Email = clientRequest.Email;

		repository.Update(client);
		ClearCache(cache, client.DddId);
		return TypedResults.Ok();
	}

	public static async Task<NoContent> DeleteClient(ICacheService cache, IClientRepository repository, Guid id)
	{
		var clientToDelete = await repository.FindById(id)
			?? throw new ArgumentException("Please inform an valid Client");

		repository.Delete(clientToDelete);
		ClearCache(cache, clientToDelete.DddId);
		return TypedResults.NoContent();
	}

	public static void ClearCache(ICacheService cache, Guid dddId)
	{
		var key = $"clients-{dddId.ToString() ?? ""}";

		cache.Delete("clients-");
		cache.Delete(key);
	}

	public static async Task<Client> VerifyDddAndReturnEntity(IDddRepository dddRepository, ClientRequest clientRequest)
	{
		var clientDdd = await dddRepository.FindById(clientRequest.DddId)
			?? throw new ArgumentException("Please inform an valid DDD");

		var client = clientRequest.ToClient();
		client.Ddd = clientDdd;

		return client;
	}
}
