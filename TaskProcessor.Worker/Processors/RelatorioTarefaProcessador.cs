using TaskProcessor.Domain.DTOs;
using TaskProcessor.Domain.Interfaces;

namespace TaskProcessor.Worker.Processors
{
    public class RelatorioTarefaProcessador : ITaskProcessor
    {
        private readonly ILogger<RelatorioTarefaProcessador> _logger;

        public RelatorioTarefaProcessador(ILogger<RelatorioTarefaProcessador> logger)
        {
            _logger = logger;
        }

        public bool PodeProcessar(string tipoTarefa)
        {
            return tipoTarefa == "GerarRelatorio";
        }

        public async Task ProcessarAsync(ProcessarTarefaMessage mensagem)
        {
            _logger.LogInformation("Iniciando geração de relatório para tarefa {TarefaId}", mensagem.TarefaId);

            try
            {
                var dados = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(mensagem.DadosTarefa);

                var random = new Random();
                if (random.Next(100) < 10)
                {
                    throw new Exception("Falha aleatória simulada");
                }

                _logger.LogInformation("Gerando relatório '{TipoRelatorio}' para período '{Periodo}'", 
                    dados["tipoRelatorio"], dados["periodo"]);

                await Task.Delay(10000);

                _logger.LogInformation("Relatório gerado com sucesso para tarefa {TarefaId}", mensagem.TarefaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar relatório para tarefa {TarefaId}", mensagem.TarefaId);
                throw;
            }
        }
    }
}
