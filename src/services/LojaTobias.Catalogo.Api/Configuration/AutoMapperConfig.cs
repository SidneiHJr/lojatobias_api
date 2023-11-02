using AutoMapper;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;

namespace LojaTobias.Catalogo.Api.Configuration
{
    public class ProdutoMapperConfig : Profile
    {
        public ProdutoMapperConfig()
        {
            CreateMap<ProdutoModel, Produto>();
            CreateMap<Produto, ProdutoResponseModel>();

            CreateMap<UnidadeMedidaModel, UnidadeMedida>();
            CreateMap<UnidadeMedida, UnidadeMedidaResponseModel>();

        }
    }
}
