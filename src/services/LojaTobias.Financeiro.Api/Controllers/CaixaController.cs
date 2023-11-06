using AutoMapper;
using LojaTobias.Api.Core.Controllers;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Financeiro.Api.Controllers
{
    [Route("api/caixa")]
    [Authorize]
    public class CaixaController : MainController
    {
        private readonly ICaixaService _caixaService;
        private readonly IMapper _mapper;
        public CaixaController(
            INotifiable notifiable, ICaixaService caixaService, IMapper mapper) : base(notifiable)
        {
            _caixaService = caixaService;
            _mapper = mapper;
        }

        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CaixaResponseModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BuscarCaixa()
        {
            var resultado = _mapper.Map<CaixaResponseModel>(await _caixaService.BuscarCaixa());

            return CustomResponse(resultado);
        }

        [HttpGet("movimentacoes")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<MovimentacaoResponseModel>), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BuscarMovimentacoes()
        {
            var resultado = _mapper.Map<IEnumerable<MovimentacaoResponseModel>>(await _caixaService.BuscarMovimentacoes());

            return CustomResponse(resultado);
        }

        [HttpPost("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Inserir([FromBody] CaixaModel model)
        {
            var resultado = await _caixaService.InserirCaixaAsync(model.SaldoInicial);

            return CustomResponse(resultado);
        }
    }
}
