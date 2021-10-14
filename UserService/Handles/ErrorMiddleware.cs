using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Handles
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ErrorMiddleware>();
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppCustomException_v1 error)
            {
                //global error v1

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new UserService.Models.v1.Base_Response(Models.v1.Base_Reponse_Status.bad_request, description: error.Message));
                await response.WriteAsync(result);

            }
            catch (AppCustomException_v2 error)
            {
                //global error v1

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new UserService.Models.v2.Base_Response(false, error.need_retry, description: error.Message));
                await response.WriteAsync(result);

            }
            catch (Exception error)
            {
                //std error message of internal app

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;

                _logger.LogError($"Error : {error.Message}");
                await response.WriteAsync("");
            }
        }
    }
}
