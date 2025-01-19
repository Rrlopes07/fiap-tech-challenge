namespace ContactsApi.Extensions;

public static class AppExtensions
{
	public static WebApplication UseConfigurations(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		return app;
	}

	public static WebApplication UseEndpoints(this WebApplication app) 
	{
		app.MapGroup("/ddd")
			.MapDddEndpoints();
		app.MapGroup("/client")
			.MapClientEndpoints();

		return app;
	}

	public static WebApplication UseErrorConfigurations(this WebApplication app) 
	{
		app.UseExceptionHandler(error =>
		{
			error.Run(async context =>
			{
				context.Response.StatusCode = 500;
				context.Response.ContentType = "application/json";

				var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

				if (contextFeature is not null)
				{
					var error = contextFeature.Error;

					if (error is ArgumentException)
						context.Response.StatusCode = 400;

					await context.Response.WriteAsJsonAsync(new
					{
						Status = context.Response.StatusCode,
						Message = error.Message
					});
				}
			});
		});

		return app;
	}
}
