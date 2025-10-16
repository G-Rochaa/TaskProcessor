using FluentValidation;
using TaskProcessor.Domain.DTOs;

namespace TaskProcessor.Application.Validators
{
    public class CriarTarefaValidator : AbstractValidator<CriarTarefaRequest>
    {
        public CriarTarefaValidator()
        {
            RuleFor(x => x.TipoTarefa)
                .NotEmpty()
                .WithMessage("Tipo da tarefa é obrigatório")
                .MaximumLength(100)
                .WithMessage("Tipo da tarefa deve ter no máximo 100 caracteres")
                .Matches("^[a-zA-Z0-9_]+$")
                .WithMessage("Tipo da tarefa deve conter apenas letras, números e underscore");

            RuleFor(x => x.DadosTarefa)
                .NotEmpty()
                .WithMessage("Dados da tarefa são obrigatórios")
                .Must(BeValidJson)
                .WithMessage("Dados da tarefa devem ser um JSON válido");

            RuleFor(x => x.MaximoTentativas)
                .GreaterThan(0)
                .WithMessage("Máximo de tentativas deve ser maior que zero")
                .LessThanOrEqualTo(10)
                .WithMessage("Máximo de tentativas não pode ser maior que 10");
        }

        private bool BeValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                System.Text.Json.JsonDocument.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
