using LojaTobias.Api.Core.Controllers;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Catalogo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : MainController
    {
        public WeatherForecastController(INotifiable notifiable) : base(notifiable)
        {
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            return CustomResponse();
        }
    }

}