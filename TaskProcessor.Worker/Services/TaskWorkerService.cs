using TaskProcessor.Domain.DTOs;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Worker.Factories;

namespace TaskProcessor.Worker.Services
{
    public class TaskWorkerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TaskWorkerService> _logger;

        public TaskWorkerService(IServiceProvider serviceProvider, ILogger<TaskWorkerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TaskWorkerService iniciado");

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var messageConsumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer>();

                _logger.LogInformation("Iniciando consumo de mensagens da fila 'tarefas-para-processar'");

                await messageConsumer.IniciarConsumoAsync<ProcessarTarefaMessage>(
                    "tarefas-para-processar",
                    async mensagem =>
                    {
                        _logger.LogInformation("Nova mensagem recebida: TarefaId={TarefaId}, Tipo={TipoTarefa}", 
                            mensagem.TarefaId, mensagem.TipoTarefa);
                            
                        using var messageScope = _serviceProvider.CreateScope();

                        var tarefaRepository = messageScope.ServiceProvider.GetRequiredService<ITarefaRepository>();
                        var processorFactory = messageScope.ServiceProvider.GetRequiredService<ProcessadorTarefasFactory>();

                        await ProcessarTarefaAsync(mensagem, tarefaRepository, processorFactory);
                    });

                _logger.LogInformation("Consumo de mensagens iniciado com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao iniciar consumo de mensagens");
                throw;
            }

            _logger.LogInformation("Worker aguardando mensagens");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogInformation("TaskWorkerService finalizado");
        }

        private async Task ProcessarTarefaAsync(
            ProcessarTarefaMessage mensagem,
            ITarefaRepository tarefaRepository,
            ProcessadorTarefasFactory processorFactory)
        {
            _logger.LogInformation("Processando tarefa {TarefaId} do tipo {TipoTarefa}",
                mensagem.TarefaId, mensagem.TipoTarefa);

            try
            {
                var tarefa = await tarefaRepository.GetByIdAsync(mensagem.TarefaId);
                if (tarefa == null)
                {
                    _logger.LogWarning("Tarefa {TarefaId} não encontrada", mensagem.TarefaId);
                    return;
                }

                if (!tarefa.PodeSerProcessada())
                {
                    _logger.LogWarning("Tarefa {TarefaId} não pode ser processada. Status: {Status}, Tentativas: {Tentativas}/{Maximo}",
                        mensagem.TarefaId, tarefa.Status, tarefa.NumeroTentativas, tarefa.MaximoTentativas);
                    return;
                }

                tarefa.MarcarComoEmProcessamento();
                await tarefaRepository.UpdateAsync(tarefa);

                var processor = processorFactory.CriarProcessor(mensagem.TipoTarefa);
                await processor.ProcessarAsync(mensagem);

                tarefa.MarcarComoConcluida();
                await tarefaRepository.UpdateAsync(tarefa);

                _logger.LogInformation("Tarefa {TarefaId} processada com sucesso", mensagem.TarefaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar tarefa {TarefaId}", mensagem.TarefaId);

                try
                {
                    var tarefa = await tarefaRepository.GetByIdAsync(mensagem.TarefaId);
                    if (tarefa != null)
                    {
                        tarefa.MarcarComoFalhou(ex.Message);
                        await tarefaRepository.UpdateAsync(tarefa);

                        if (tarefa.FalhouDefinitivamente())
                        {
                            _logger.LogError("Tarefa {TarefaId} falhou definitivamente após {Tentativas} tentativas",
                                mensagem.TarefaId, tarefa.NumeroTentativas);
                        }
                        else
                        {
                            _logger.LogWarning("Tarefa {TarefaId} falhou, será reprocessada. Tentativa {Tentativas}/{Maximo}",
                                mensagem.TarefaId, tarefa.NumeroTentativas, tarefa.MaximoTentativas);
                        }
                    }
                }
                catch (Exception updateEx)
                {
                    _logger.LogError(updateEx, "Erro ao atualizar status da tarefa {TarefaId} após falha", mensagem.TarefaId);
                }
            }
        }
    }
}
