using LojaTobias.Api.Core.Controllers;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Identidade.Api.Controllers
{
    [Route("api/usuario")]
    public class UsuarioController : MainController
    {
        public UsuarioController(INotifiable notifiable) : base(notifiable)
        {
        }



    }
}
