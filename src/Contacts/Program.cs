using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.AddDb()
	.AddServices()
	.AddDependencies()
	.AddPort()
	.Services.UseHttpClientMetrics()
	.AddHealthChecks();

var app = builder.Build();

app.UseConfigurations()
	.UseErrorConfigurations()
	.UseEndpoints();

app.Run();
