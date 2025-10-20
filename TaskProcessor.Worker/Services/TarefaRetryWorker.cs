using Microsoft.Extensions.Options;
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Infrastructure.Settings;

namespace TaskProcessor.Worker.Services
{
    public class TarefaRetryWorker : BackgroundService
    {
        #region Private Fields

        private readonly IServiceProvider _serviceProvider;
        private readonly TarefaRetrySettings _retrySettings;
        private readonly ILogger<TarefaRetryWorker> _logger;

        #endregion Private Fields

        #region Public Constructor

        public TarefaRetryWorker(
            IServiceProvider serviceProvider,
            IOptions<TarefaRetrySettings> retrySettings,
            ILogger<TarefaRetryWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _retrySettings = retrySettings.Value;
            _logger = logger;
        }

        #endregion Public Constructor

        #region Protected Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TarefaRetryWorker iniciado");

            if (!_retrySettings.Habilitado)
            {
                _logger.LogInformation("TarefaRetryWorker desabilitado pela configuração");
                return;
            }

            _logger.LogInformation("TarefaRetryWorker configurado com intervalo de {IntervaloMinutos} minutos",
                _retrySettings.IntervaloMinutos);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogDebug("Executando ciclo de retry de tarefas");

                        using var scope = _serviceProvider.CreateScope();
                        var tarefaRetryAppService = scope.ServiceProvider.GetRequiredService<ITarefaRetryAppService>();

                        await tarefaRetryAppService.ProcessarTarefasPendentesAsync();

                        var quantidadePendentes = await tarefaRetryAppService.ObterQuantidadeTarefasPendentesAsync();

                        if (quantidadePendentes > 0)
                        {
                            _logger.LogDebug("Ainda existem {Quantidade} tarefas pendentes para retry",
                                quantidadePendentes);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro durante ciclo de retry de tarefas");
                    }

                    var intervalo = TimeSpan.FromMinutes(_retrySettings.IntervaloMinutos);
                    _logger.LogDebug("Aguardando {Intervalo} para próximo ciclo de retry", intervalo);

                    await Task.Delay(intervalo, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("TarefaRetryWorker cancelado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro crítico no TarefaRetryWorker");
                throw;
            }

            _logger.LogInformation("TarefaRetryWorker finalizado");
        }

        #endregion Protected Methods
    }
}