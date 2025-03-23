namespace ContactsApi.Extensions;

public static class BuilderExtensions
{
	public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		builder.Services.AddMemoryCache();
		builder.Services.AddValidatorsFromAssemblyContaining<Program>();

		return builder;
	}

	public static WebApplicationBuilder AddDb(this WebApplicationBuilder builder) 
	{
		var configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseNpgsql(configuration
				.GetConnectionString("ConnectionString")));

		return builder;
	}

	public static WebApplicationBuilder AddPort(this WebApplicationBuilder builder)
	{
		builder.WebHost.ConfigureKestrel((context, serverOpt) =>
			serverOpt.ListenAnyIP(5000));

		return builder;
	}

	public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder) 
	{
		builder.Services.AddTransient<ICacheService, CacheService>();
		builder.Services.AddTransient<IDddRepository, DddRepository>();
		builder.Services.AddTransient<IClientRepository, ClientRepository>();

		return builder;
	}
}
