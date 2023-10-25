using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using LojaTobias.Core.Interfaces;
using LojaTobias.Api.Core.Models;

namespace LojaTobias.Api.Core.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ICollection<string> Erros = new List<string>();

        protected readonly INotifiable _notifiable;

        public MainController(INotifiable notifiable)
        {
            _notifiable = notifiable;
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new OkModel(result));
            }

            return BadRequest(new BadRequestModel(_notifiable.GetNotifications.ToArray()));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                _notifiable.AddNotification(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected IActionResult InternalServerError(object resultado = null)
        {
            return StatusCode(500, new InternalServerErrorModel(resultado, this.Erros));
        }

        protected IActionResult PagingResponse(int pageIndex, int total, int totalPages, object resultado = null)
        {
            if (OperacaoValida())
            {
                SetPaginateHeader(pageIndex, totalPages, total);

                return Ok(new OkModel(resultado));
            }

            return Ok(new BadRequestModel(this.Erros));
        }

        protected void SetPaginateHeader(int numeroPagina, int totalPaginas, int total)
        {
            HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(
                new { NumeroPagina = numeroPagina, TotalPaginas = totalPaginas, Total = total }
            ));
        }


        protected bool OperacaoValida()
        {
            return !_notifiable.HasNotification;
        }
    }
}
