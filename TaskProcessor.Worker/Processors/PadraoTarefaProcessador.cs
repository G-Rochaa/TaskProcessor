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

            // TESTE DE FALHA - Descomente a linha abaixo para simular erro
            throw new Exception($"Erro simulado para testar retry");

            await Task.Delay(1000);

            _logger.LogInformation("Tarefa genérica {TipoTarefa} processada com sucesso para tarefa {TarefaId}",
                mensagem.TipoTarefa, mensagem.TarefaId);
        }
    }
}
