using System.Net;
using Newtonsoft.Json;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.Configuration;

public class ErrorExceptionHandlingMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public ErrorExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "error during executing {Context}", context.Request.Path.Value);
            var response = context.Response;
            response.ContentType = "application/json";

            HttpStatusCode code;
            string messageError = exception.Message;
            switch (exception.InnerException)
            {
                case KeyNotFoundException
                    or FileNotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case ArgumentException
                    or InvalidOperationException
                    or FormatException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case Npgsql.NpgsqlException:
                    code = HttpStatusCode.ServiceUnavailable;
                    messageError = "Serviço indisponível. Falha ao conectar no banco de dados. Tente novamente.";
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            var message = JsonConvert.SerializeObject(
            new
            {
                status_code = (int)code,
                message_error = messageError
            });

            response.StatusCode = (int)code;
            await response.WriteAsync(message);
        }
    }
}
