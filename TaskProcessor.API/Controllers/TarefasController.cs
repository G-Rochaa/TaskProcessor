using Microsoft.AspNetCore.Mvc;
using TaskProcessor.Application.AppServices;
using TaskProcessor.Domain.DTOs;
using FluentValidation;

namespace TaskProcessor.API.Controllers
{
    [ApiController]
    [Route("api/tarefas")]
    public class TarefasController : ControllerBase
    {
        #region Private Fields
        private readonly TarefaAppService _tarefaAppService;
        private readonly IValidator<CriarTarefaRequest> _validator;

        #endregion Private Fields

        #region Public Constructor
        public TarefasController(TarefaAppService tarefaAppService, IValidator<CriarTarefaRequest> validator)
        {
            _tarefaAppService = tarefaAppService;
            _validator = validator;
        }

        #endregion Public Constructor

        #region Public Methods

        [HttpPost]
        [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TarefaResponse>> CriarTarefaAsync([FromBody] CriarTarefaRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Dados inválidos", errors = errors });
            }

            try
            {
                var tarefa = await _tarefaAppService.CriarTarefaAsync(request);
                return CreatedAtRoute(routeName: "ObterTarefa", routeValues: new { id = tarefa.Id }, value: tarefa);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}", Name = "ObterTarefa")]
        [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TarefaResponse>> ObterTarefaAsync(Guid id)
        {
            var tarefa = await _tarefaAppService.ObterTarefaAsync(id);
            
            if (tarefa == null)
                return NotFound(new { message = "Tarefa não encontrada" });

            return Ok(tarefa);
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TarefaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TarefaResponse>>> ListarTarefaAsync()
        {
            var tarefas = await _tarefaAppService.ListarTarefasAsync();
            return Ok(tarefas);
        }

        [HttpGet("pendentes")]
        [ProducesResponseType(typeof(IEnumerable<TarefaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TarefaResponse>>> ListarTarefaPendentesAsync()
        {
            var tarefas = await _tarefaAppService.ListarTarefasPendentesAsync();
            return Ok(tarefas);
        }

        #endregion Public Methods
    }
}
