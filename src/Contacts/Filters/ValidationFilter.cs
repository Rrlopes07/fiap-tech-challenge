namespace ContactsApi.Filters;

public class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
{
	private readonly IValidator<TRequest> Validator = validator;

	public async ValueTask<object?> InvokeAsync(
		EndpointFilterInvocationContext context,
		EndpointFilterDelegate next)
	{
		var request = context.Arguments.OfType<TRequest>().First();

		var result = await Validator.ValidateAsync(request, context.HttpContext.RequestAborted);

		if (!result.IsValid)
			return TypedResults.ValidationProblem(result.ToDictionary());

		return await next(context);
	}
}

public static class ValidationExtensions
{
	public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
		=> builder.AddEndpointFilter<ValidationFilter<TRequest>>()
			.ProducesValidationProblem();
}
