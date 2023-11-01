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

        public CrudController(IService<TEntity> service, IMapper mapper, INotifiable notifiable) : base(notifiable)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public virtual async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var resultado = _mapper.Map<TResponse>(await _service.GetAsync(id));

            return CustomResponse(resultado);
        }

        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public virtual async Task<IActionResult> Insert([FromBody] TModel item)
        {
            var resultado = await _service.InsertAsync(_mapper.Map<TEntity>(item));
            return CustomResponse(resultado);

        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public virtual async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TModel item)
        {
            await _service.UpdateAsync(id, _mapper.Map<TEntity>(item));

            return CustomResponse();

        }


        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);

            return CustomResponse();
            
        }

    }
}
