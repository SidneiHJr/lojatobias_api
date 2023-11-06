using AutoMapper;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;

namespace LojaTobias.Estoque.Api.Configuration
{
    public class PedidoMapperConfig : Profile
    {
        public PedidoMapperConfig()
        {
            CreateMap<PedidoCompraModel, Pedido>();
            CreateMap<PedidoVendaModel, Pedido>();
            CreateMap<Pedido, PedidoResponseModel>();

            CreateMap<PedidoItemModel, PedidoItem>();
            CreateMap<PedidoItem, PedidoItemResponseModel>();

            CreateMap<AjusteModel, Ajuste>();
            CreateMap<Ajuste, AjusteResponseModel>();
        }
    }
}
