namespace ContactsApi.Extensions;

public static class ClientExtensions
{
	public static Client ToClient(this ClientRequest request)
		=> new() 
		{ 
			Id = Guid.NewGuid(), 
			Name = request.Name, 
			Telephone = request.Telephone, 
			Email = request.Email, 
			DddId = request.DddId
		};

	public static ClientResponse ToResponse(this Client client)
		=> new(client.Id, client.Name, client.Telephone, client.Email, client.DddId);

	public static List<ClientResponse> ToResponseList(this IList<Client> list)
		=> list.Select(c => c.ToResponse()).ToList();
}
