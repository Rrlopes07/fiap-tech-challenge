namespace ContactsApi.Response;

public record ClientResponse(Guid Id, string Name, string Telephone, string Email, Guid DddId);
