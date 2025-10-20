using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Infrastructure;
using TaskProcessor.Infrastructure.Data.MongoDb.Configurations;
using TaskProcessor.Worker.Factories;
using TaskProcessor.Worker.Services;

namespace TaskProcessor.Worker.Extensions
{
    public static class WorkerDependencyInjection
    {
        public static IServiceCollection AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
        {
            TarefaConfiguration.Configure();

            services.AddInfrastructure(configuration);

            services.AddScoped<ProcessadorTarefasFactory>();

            var assembly = typeof(TaskWorkerService).Assembly;
            var processorTypes = assembly.GetTypes()
                .Where(t => typeof(ITaskProcessor).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

            foreach (var type in processorTypes)
                services.AddScoped(typeof(ITaskProcessor), type);

            services.AddHostedService<TaskWorkerService>();
            services.AddHostedService<TarefaRetryWorker>();

            return services;
        }
    }
}
