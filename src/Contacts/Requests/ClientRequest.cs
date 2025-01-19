namespace ContactsApi.Requests;

public record ClientRequest(
	string Name,
	string Telephone,
	string Email,
	Guid DddId);
