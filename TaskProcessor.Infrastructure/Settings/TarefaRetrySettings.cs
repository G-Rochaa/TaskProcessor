namespace TaskProcessor.Infrastructure.Settings
{
    public class TarefaRetrySettings
    {
        public bool Habilitado { get; set; } = true;
        public int IntervaloMinutos { get; set; } = 1;
        public int MaximoTarefasPorCiclo { get; set; } = 100;
        public int TempoMinimoEntreTentativasMinutos { get; set; } = 1;
    }
}
