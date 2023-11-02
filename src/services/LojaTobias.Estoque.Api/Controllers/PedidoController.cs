using AutoMapper;
using LojaTobias.Api.Core.Controllers;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Estoque.Api.Controllers
{
    [Route("api/pedido")]
    public class PedidoController : CrudController<PedidoModel, PedidoResponseModel, Pedido>
    {
        public PedidoController(
            IService<Pedido> service, 
            IMapper mapper, 
            INotifiable notifiable) : base(service, mapper, notifiable)
        {
        }
    }
}
