
using LojaTobias.Api.Core.Controllers;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Identidade.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : MainController
    {
        public AuthController(INotifiable notifiable) : base(notifiable)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Teste()
        {
            return Ok("Sucesso!");
        }
    }
}
