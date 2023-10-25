using AutoMapper;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LojaTobias.Api.Core.Controllers
{
    public class CrudController<TModel, TResponse, TEntity> : MainController where TEntity : EntityBase
    {
        private readonly IService<TEntity> _service;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        public CrudController(IService<TEntity> service, IMapper mapper,
            ILogger<CrudController<TModel, TResponse, TEntity>> logger, INotifiable notifiable) : base(notifiable)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public virtual async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var resultado = _mapper.Map<TResponse>(await _service.GetAsync(id));
                return CustomResponse(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message}");
            }
        }

        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public virtual async Task<IActionResult> Insert([FromBody] TModel item)
        {
            try
            {
                var resultado = await _service.InsertAsync(_mapper.Map<TEntity>(item));
                return CustomResponse(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public virtual async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TModel item)
        {
            try
            {
                await _service.UpdateAsync(id, _mapper.Map<TEntity>(item));

                return CustomResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return CustomResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message}");
            }
        }

    }
}
