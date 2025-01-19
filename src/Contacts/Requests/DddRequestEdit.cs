namespace ContactsApi.Requests;

public record DddRequestEdit(Guid Id, string Region, int DddNumber)
	: DddRequest(Region, DddNumber);
