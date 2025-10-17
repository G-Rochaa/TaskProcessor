using TaskProcessor.Domain.DTOs;
using TaskProcessor.Domain.Interfaces;

namespace TaskProcessor.Worker.Processors
{
    public class PadraoTarefaProcessador : ITaskProcessor
    {
        private readonly ILogger<PadraoTarefaProcessador> _logger;

        public PadraoTarefaProcessador(ILogger<PadraoTarefaProcessador> logger)
        {
            _logger = logger;
        }

        public bool PodeProcessar(string tipoTarefa)
        {
            return true; 
        }

        public async Task ProcessarAsync(ProcessarTarefaMessage mensagem)
        {
            _logger.LogInformation("Processando tarefa genérica {TipoTarefa} para tarefa {TarefaId}", 
                mensagem.TipoTarefa, mensagem.TarefaId);

            try
            {
                _logger.LogInformation("Executando processamento genérico para tipo '{TipoTarefa}'", mensagem.TipoTarefa);

                await Task.Delay(3000);

                _logger.LogInformation("Processamento genérico concluído para tarefa {TarefaId}", mensagem.TarefaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no processamento genérico para tarefa {TarefaId}", mensagem.TarefaId);
                throw;
            }
        }
    }
}
