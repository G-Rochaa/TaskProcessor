using TaskProcessor.Infrastructure;
using TaskProcessor.Worker.Services;
using TaskProcessor.Worker.Extensions;
using TaskProcessor.Infrastructure.Data.MongoDb.Configurations;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWorkerServices(); 

TarefaConfiguration.Configure();

builder.Services.AddHostedService<TaskWorkerService>();

var host = builder.Build();
host.Run();
