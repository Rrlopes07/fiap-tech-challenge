namespace ContactsApi.Requests.Validators;

public class DddRequestValidator : AbstractValidator<DddRequest>
{
    public DddRequestValidator()
	{
		RuleFor(x => x.Region)
            .MinimumLength(4)
            .MaximumLength(100)
            .NotEmpty()
            .NotNull();
        RuleFor(x => x.DddNumber)
            .InclusiveBetween(10, 99)
            .NotNull();
    }
}
