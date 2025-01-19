var builder = WebApplication.CreateBuilder(args);

builder.AddDb()
	.AddServices()
	.AddDependencies();

var app = builder.Build();

app.UseConfigurations()
	.UseErrorConfigurations()
	.UseEndpoints();

app.Run();
