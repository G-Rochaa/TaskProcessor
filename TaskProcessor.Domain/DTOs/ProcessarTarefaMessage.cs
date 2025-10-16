namespace TaskProcessor.Domain.DTOs
{
    public class ProcessarTarefaMessage
    {
        public Guid TarefaId { get; set; }
        public string TipoTarefa { get; set; } = string.Empty;
        public string DadosTarefa { get; set; } = string.Empty;
        public int NumeroTentativas { get; set; }
        public int MaximoTentativas { get; set; }
    }
}
