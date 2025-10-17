using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Worker.Factories;
using TaskProcessor.Worker.Services;

namespace TaskProcessor.Worker.Extensions
{
    public static class WorkerDependencyInjection
    {
        public static IServiceCollection AddWorkerServices(this IServiceCollection services)
        {
            services.AddScoped<ProcessadorTarefasFactory>();

            var assembly = typeof(TaskWorkerService).Assembly;
            var processorTypes = assembly.GetTypes()
                .Where(t => typeof(ITaskProcessor).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

            foreach (var type in processorTypes)
                services.AddScoped(typeof(ITaskProcessor), type);

            return services;
        }
    }
}
