namespace ContactsApi.Requests.Validators;

public class ClientRequestEditValidator : AbstractValidator<ClientRequestEdit>
{
    public ClientRequestEditValidator()
    {
		RuleFor(x => x.Name)
			.MinimumLength(4).WithMessage("Name should have a minimum length of 4 characters")
			.MaximumLength(100).WithMessage("Name should have a maximum length of 100 characters")
			.NotEmpty().WithMessage("Name should not be empty")
			.NotNull().WithMessage("Name field do not acceppt null values");
		RuleFor(x => x.Telephone)
			.Length(9).WithMessage("Telephone should have a length of 9 characters")
			.NotEmpty().WithMessage("Telephone should not be empty")
			.NotNull().WithMessage("Telephone field do not acceppt null values");
		RuleFor(x => x.Email)
			.EmailAddress().WithMessage("Please inform an valid e-mail")
			.MinimumLength(9).WithMessage("E-mail should have a minimum length of 9 characters")
			.MaximumLength(100).WithMessage("E-mail should have a maximum length of 100 characters")
			.NotEmpty().WithMessage("E-mail should not be empty")
			.NotNull().WithMessage("E-mail field do not acceppt null values");
		RuleFor(x => x.Id)
			.NotEmpty().WithMessage("Client is not informed")
			.NotNull().WithMessage("Client field do not acceppt null values");
		RuleFor(x => x.DddId)
			.NotEmpty().WithMessage("Ddd is not informed")
			.NotNull().WithMessage("Ddd field do not acceppt null values");
	}
}
