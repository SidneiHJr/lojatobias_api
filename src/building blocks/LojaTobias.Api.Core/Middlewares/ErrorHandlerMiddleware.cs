using LojaTobias.Core.Exceptions;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace LojaTobias.Api.Core
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogProvider logProvider)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException e:
                        await logProvider.InserirLogExcecao(context.Request.Method, $"Erro: {error?.Message} - Inner: {error?.InnerException?.Message}");
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        await logProvider.InserirLogExcecao(context.Request.Method, $"Erro: {error?.Message} - Inner: {error?.InnerException?.Message}");
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        await logProvider.InserirLogExcecao(context.Request.Method, $"Erro: {error?.Message} - Inner: {error?.InnerException?.Message}");
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
