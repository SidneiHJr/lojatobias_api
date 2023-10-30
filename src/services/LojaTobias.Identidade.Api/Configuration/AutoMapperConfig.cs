using AutoMapper;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;

namespace LojaTobias.Identidade.Api.Configuration
{
    public class UsuarioMapperConfig : Profile
    {
        public UsuarioMapperConfig()
        {
            CreateMap<UsuarioModel, Usuario>();
            CreateMap<Usuario, UsuarioResponseModel>()
                .ForMember(p => p.Email, opt => opt.MapFrom(x => x.Email.Endereco));
        }
    }
}
