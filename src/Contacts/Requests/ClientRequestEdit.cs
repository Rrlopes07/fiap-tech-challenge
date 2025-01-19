namespace ContactsApi.Requests;

public record ClientRequestEdit(
	Guid Id, 
	string Name, 
	string Telephone, 
	string Email) 
	: ClientRequest(Name, Telephone, Email, Guid.NewGuid());
