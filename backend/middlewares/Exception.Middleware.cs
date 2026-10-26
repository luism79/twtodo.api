using backend.services.exceptions;
using backend.shared;
using System.Net;

namespace backend.middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        { 
            _next = next;
            _environment = environment;
        }
        

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }            
            catch (Exception e)
            {
                await ExceptionAsync(context, e);
            }
        }

        private async Task ExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, message) = ex switch
            {                
                ServicesPostExceptions => (HttpStatusCode.BadRequest, "Falha na criação do registro"),
                ServicesPutExceptions => (HttpStatusCode.BadRequest, "Falha na atualização"),
                ServicesDeleteExceptions => (HttpStatusCode.BadRequest,"Falha na exclusão"),
                ServicesAuthenticatedExceptions => (HttpStatusCode.Unauthorized, "Falha na atenticação"),
                ServicesExceptions => (HttpStatusCode.NotFound, "Falha ao localizar o registro"),
            };

            // Em produção, não expõe detalhes de erros inesperados
            var error = statusCode == HttpStatusCode.InternalServerError && !_environment.IsDevelopment()
                ? "Ocorreu um erro inesperado."
                : ex.Message;

            var resposta = ApiResponse.Error(message, error);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(resposta);
        }
    }
}
