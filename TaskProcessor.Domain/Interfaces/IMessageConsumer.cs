namespace TaskProcessor.Domain.Interfaces
{
    public interface IMessageConsumer
    {
        Task IniciarConsumoAsync<T>(string nomeFila, Func<T, Task> manipuladorMensagem) where T : class;
        Task PararConsumoAsync();
    }
}
