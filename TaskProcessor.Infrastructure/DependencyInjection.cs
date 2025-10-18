using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskProcessor.Application.AppServices;
using TaskProcessor.Application.Validators;
using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Services;
using TaskProcessor.Infrastructure.Data.MongoDB;
using TaskProcessor.Infrastructure.Settings;
using TaskProcessor.Infrastructure.Messaging.RabbitMQ;
using TaskProcessor.Infrastructure.Data.MongoDB.Repositories;

namespace TaskProcessor.Infrastructure
{
    public static class DependencyInjection
    {

        #region Public Methods

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSettings(configuration);
            services.AddMongoDB();
            services.AddRabbitMQ();
            services.AddApplication();
            return services;
        }

        #endregion Public Methods

        #region Private Methods

        private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
            return services;
        }

        private static IServiceCollection AddMongoDB(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbContext>();
            services.AddRepositories();
            return services;
        }


        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Tarefa>>(provider =>
            {
                var context = provider.GetRequiredService<MongoDbContext>();
                return new BaseRepository<Tarefa>(context.Tarefas);
            });

            services.AddScoped<ITarefaRepository>(provider =>
            {
                var context = provider.GetRequiredService<MongoDbContext>();
                return new TarefaRepository(context.Tarefas);
            });

            return services;
        }

        private static IServiceCollection AddRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMQConnection>();
            services.AddScoped<IMessagePublisher, RabbitMQPublisher>();
            services.AddScoped<IMessageConsumer, RabbitMQConsumer>();
            return services;
        }

        private static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITarefaService, TarefaService>();

            services.AddScoped<TarefaAppService>();

            services.AddValidatorsFromAssemblyContaining<CriarTarefaValidator>();

            return services;
        }

        #endregion Private Methods
    }
}
