using AutoMapper;
using LojaTobias.Api.Core.Controllers;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;

namespace LojaTobias.Catalogo.Api.Controllers
{
    public class ProdutoController : CrudController<ProdutoModel, ProdutoResponseModel, Produto>
    {
        public ProdutoController(
            IService<Produto> service, 
            IMapper mapper, 
            ILogger<CrudController<ProdutoModel, ProdutoResponseModel, Produto>> logger,
            INotifiable notifiable) : base(service, mapper, logger, notifiable)
        {

        }
    }
}
