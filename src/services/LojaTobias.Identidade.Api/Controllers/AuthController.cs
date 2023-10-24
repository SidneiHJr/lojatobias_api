
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Identidade.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Teste()
        {
            return Ok("Sucesso!");
        }
    }
}
