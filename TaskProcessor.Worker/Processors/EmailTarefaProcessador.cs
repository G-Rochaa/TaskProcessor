using TaskProcessor.Domain.DTOs;
using TaskProcessor.Domain.Interfaces;

namespace TaskProcessor.Worker.Processors
{
    public class EmailTarefaProcessador : ITaskProcessor
    {
        private readonly ILogger<EmailTarefaProcessador> _logger;

        public EmailTarefaProcessador(ILogger<EmailTarefaProcessador> logger)
        {
            _logger = logger;
        }

        public bool PodeProcessar(string tipoTarefa)
        {
            return tipoTarefa == "EnviarEmail";
        }

        public async Task ProcessarAsync(ProcessarTarefaMessage mensagem)
        {
            _logger.LogInformation("Iniciando processamento de email para tarefa {TarefaId}", mensagem.TarefaId);

            try
            {
                var dados = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(mensagem.DadosTarefa);

                var random = new Random();
                if (random.Next(100) < 10)
                {
                    throw new Exception("Falha aleatória simulada");
                }

                _logger.LogInformation("Enviando email para {Email} com assunto '{Assunto}'", 
                    dados["email"], dados["assunto"]);

                await Task.Delay(5000);

                _logger.LogInformation("Email enviado com sucesso para tarefa {TarefaId}", mensagem.TarefaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar email para tarefa {TarefaId}", mensagem.TarefaId);
                throw;
            }
        }
    }
}
