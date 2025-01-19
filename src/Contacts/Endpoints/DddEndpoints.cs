namespace ContactsApi.Endpoints;

public static class DddEndpoints
{
	public static RouteGroupBuilder MapDddEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", GetAllDdd);
		group.MapGet("/{id}", FindById);
		group.MapPost("/", AddDdd)
			.AddEndpointFilter<ValidationFilter<DddRequest>>();
		group.MapPut("/", UpdateDdd)
			.AddEndpointFilter<ValidationFilter<DddRequestEdit>>();
		group.MapDelete("/{id}", DeleteDdd);

		return group;
	}

	public static async Task<Ok<List<DddResponse>>> GetAllDdd(IDddRepository repository)
	{
		var result = await repository.FindAll();

		return TypedResults.Ok(result.ToResponseList());
	} 
		
	public static async Task<Results<Ok<DddResponse>, NotFound<string>>> FindById(IDddRepository repository, Guid id)
		=> await repository.FindById(id)
		is { } ddd
			? TypedResults.Ok(ddd.ToResponse())
			: TypedResults.NotFound("DDD not found");

	public static Created AddDdd(IDddRepository repository, [FromBody] DddRequest request)
	{
		var entity = request.ToDdd();

		repository.Add(entity);
		return TypedResults.Created();
	}

	public static async Task<Ok> UpdateDdd(IDddRepository repository, [FromBody] DddRequestEdit request)
	{
		var dddToUpdate = await repository.FindById(request.Id)
			?? throw new ArgumentException("Please inform an valid DDD");

		dddToUpdate.Region = request.Region;
		dddToUpdate.DddNumber = request.DddNumber;

		repository.Update(dddToUpdate);
		return TypedResults.Ok();
	}

	public static async Task<NoContent> DeleteDdd(IDddRepository repository, Guid id)
	{
		var dddToDelete = await repository.FindById(id)
			?? throw new ArgumentException("Please inform an valid DDD");

		repository.Delete(dddToDelete);
		return TypedResults.NoContent();
	}
}
