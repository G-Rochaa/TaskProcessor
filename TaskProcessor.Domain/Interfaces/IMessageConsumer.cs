namespace TaskProcessor.Domain.Interfaces
{
    public interface IMessageConsumer
    {
        #region Public Methods

        Task IniciarConsumoAsync<T>(string nomeFila, Func<T, Task> manipuladorMensagem) where T : class;
        Task PararConsumoAsync();

        #endregion Public Methods
    }
}
