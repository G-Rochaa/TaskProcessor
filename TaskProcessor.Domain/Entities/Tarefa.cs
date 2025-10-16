using TaskProcessor.Domain.Enums;

namespace TaskProcessor.Domain.Entities
{
    public class Tarefa
    {
        #region Public Properties

        public Guid Id { get; private set; }
        public string TipoTarefa { get; private set; } = string.Empty;
        public StatusTarefaEnum Status { get; private set; }
        public int NumeroTentativas { get; private set; }
        public int MaximoTentativas { get; private set; }
        public DateTime DtCriacao { get; private set; }
        public DateTime DtAtualizacao { get; private set; }
        public DateTime? DtProcessamento { get; private set; }

        #endregion Public Properties

        #region Public Constructor

        public Tarefa(string tipoTarefa, string dadosTarefa, int maximoTentativas = 3)
        {
            if (string.IsNullOrWhiteSpace(tipoTarefa))
                throw new ArgumentException("O tipo da tarefa não pode ser vazio.", nameof(tipoTarefa));

            if (string.IsNullOrWhiteSpace(dadosTarefa))
                throw new ArgumentException("Os dados da tarefa não podem ser vazios.", nameof(dadosTarefa));

            Id = Guid.NewGuid();
            TipoTarefa = tipoTarefa;
            Status = StatusTarefaEnum.Pendente;
            NumeroTentativas = 0;
            MaximoTentativas = maximoTentativas;
            DtCriacao = DateTime.UtcNow;
            DtAtualizacao = DateTime.UtcNow;
        }

        #endregion Public Constructor

        #region Public Methods


        #endregion Public Methods
    }

}
