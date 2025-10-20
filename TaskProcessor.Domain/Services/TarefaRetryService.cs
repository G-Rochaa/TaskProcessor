using Microsoft.Extensions.Logging;
using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.Enums;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Mappings;

namespace TaskProcessor.Domain.Services
{
    public class TarefaRetryService : ITarefaRetryService
    {
        #region Private Fields

        private readonly ITarefaRepository _tarefaRepository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<TarefaRetryService> _logger;

        #endregion Private Fields

        #region Public Constructor

        public TarefaRetryService(ITarefaRepository tarefaRepository, IMessagePublisher messagePublisher, ILogger<TarefaRetryService> logger)
        {
            _tarefaRepository = tarefaRepository;
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        #endregion Public Constructor

        #region Public Methods

        public async Task<IEnumerable<Tarefa>> ObterTarefasParaRetryAsync()
        {
            _logger.LogDebug("Buscando tarefas pendentes para retry");

            var tarefasPendentes = await _tarefaRepository.GetTarefasPendentesAsync();
            
            var tarefasParaRetry = new List<Tarefa>();
            
            foreach (var tarefa in tarefasPendentes)
            {
                if (await TarefaDeveSerReprocessadaAsync(tarefa))
                {
                    tarefasParaRetry.Add(tarefa);
                }
            }

            _logger.LogInformation("Encontradas {Count} tarefas para retry", tarefasParaRetry.Count);
            
            return tarefasParaRetry;
        }

        public async Task<bool> TarefaDeveSerReprocessadaAsync(Tarefa tarefa)
        {
            if (tarefa.Status != StatusTarefaEnum.Pendente)
            {
                _logger.LogDebug("Tarefa {TarefaId} não está pendente. Status: {Status}", 
                    tarefa.Id, tarefa.Status);
                return false;
            }

            if (!tarefa.PodeSerProcessada())
            {
                _logger.LogDebug("Tarefa {TarefaId} não pode ser processada. Tentativas: {Tentativas}/{Maximo}", 
                    tarefa.Id, tarefa.NumeroTentativas, tarefa.MaximoTentativas);
                return false;
            }

            var tempoDesdeUltimaTentativa = DateTime.UtcNow - tarefa.DtAtualizacao;
            var tempoMinimoEntreTentativas = TimeSpan.FromMinutes(1);

            if (tempoDesdeUltimaTentativa < tempoMinimoEntreTentativas)
            {
                _logger.LogDebug("Tarefa {TarefaId} ainda não pode ser reprocessada. Tempo desde última tentativa: {Tempo}", 
                    tarefa.Id, tempoDesdeUltimaTentativa);
                return false;
            }

            _logger.LogDebug("Tarefa {TarefaId} deve ser reprocessada", tarefa.Id);
            return true;
        }

        public async Task ProcessarTarefasPendentesAsync()
        {
            _logger.LogDebug("Iniciando processamento de tarefas pendentes para retry");

            var tarefasParaRetry = await ObterTarefasParaRetryAsync();

            var tarefasProcessadas = 0;

            foreach (var tarefa in tarefasParaRetry)
            {
                try
                {
                    var mensagem = TarefaMapping.ToMessage(tarefa);
                    await _messagePublisher.PublicarAsync(mensagem, "tarefas-para-processar");

                    tarefasProcessadas++;

                    _logger.LogDebug("Tarefa {TarefaId} recolocada na fila para retry. Tentativa {Tentativas}/{Maximo}",
                        tarefa.Id, tarefa.NumeroTentativas + 1, tarefa.MaximoTentativas);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao recolocar tarefa {TarefaId} na fila", tarefa.Id);
                }
            }

            if (tarefasProcessadas > 0)
            {
                _logger.LogInformation("Processamento de retry concluído. {Processadas} tarefas recolocadas na fila",
                    tarefasProcessadas);
            }
            else
            {
                _logger.LogDebug("Nenhuma tarefa encontrada para retry");
            }
        }

        #endregion Public Methods
    }
}