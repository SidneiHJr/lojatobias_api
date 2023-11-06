
using AutoMapper;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;

namespace LojaTobias.Financeiro.Api.Configuration
{
    public class CaixaMapperConfig : Profile
    {
        public CaixaMapperConfig()
        {
            CreateMap<CaixaModel, Caixa>();
            CreateMap<Caixa, CaixaResponseModel>();

            CreateMap<Movimentacao, MovimentacaoResponseModel>();
        }
    }
}
