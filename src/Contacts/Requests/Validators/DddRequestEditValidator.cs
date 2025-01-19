namespace ContactsApi.Requests.Validators;

public class DddRequestEditValidator : AbstractValidator<DddRequestEdit>
{
    public DddRequestEditValidator()
    {
		RuleFor(x => x.Region)
			.MinimumLength(4)
			.MaximumLength(100)
			.NotEmpty()
			.NotNull();
		RuleFor(x => x.DddNumber)
			.InclusiveBetween(10, 99)
			.NotNull();
		RuleFor(x => x.Id)
			.NotEmpty()
			.NotNull();
	}
}
