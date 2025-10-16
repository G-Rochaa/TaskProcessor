using TaskProcessor.Domain.Enums;

namespace TaskProcessor.Domain.DTOs
{
    public class TarefaResponse
    {
        public Guid Id { get; set; }
        public string TipoTarefa { get; set; } = string.Empty;
        public string DadosTarefa { get; set; } = string.Empty;
        public StatusTarefaEnum Status { get; set; }
        public int NumeroTentativas { get; set; }
        public int MaximoTentativas { get; set; }
        public DateTime DtCriacao { get; set; }
        public DateTime DtAtualizacao { get; set; }
        public DateTime? DtProcessamento { get; set; }
        public string? MensagemErro { get; set; }
    }
}
