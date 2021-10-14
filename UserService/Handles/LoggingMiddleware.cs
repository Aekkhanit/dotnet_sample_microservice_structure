using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Handles
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var log = new AppLoging
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                QueryString = context.Request.QueryString.ToString(),
                RespondedAt = DateTime.Now
            };
            if (context.Request.Method == "POST" || context.Request.Method == "PUT")
            {
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body)
                                                    .ReadToEndAsync();
                context.Request.Body.Position = 0;
                log.Payload = body;
            }


            using (Stream originalRequest = context.Response.Body)
            {
                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        context.Response.Body = memStream;
                        await _next.Invoke(context);
                        memStream.Position = 0;
                        var response = await new StreamReader(memStream).ReadToEndAsync();
                        log.Response = response;
                        log.ResponseCode = context.Response.StatusCode.ToString();
                        log.RespondedAt = DateTime.Now;
                        memStream.Position = 0;
                        await memStream.CopyToAsync(originalRequest);
                    }
                }
                finally
                {
                    // assign the response body to the actual context
                    context.Response.Body = originalRequest;

                }
            }
            _logger.LogInformation(JsonConvert.SerializeObject(log));

        }


        public class AppLoging
        {
            public string Path { get; set; }
            public string Method { get; set; }
            public string QueryString { get; set; }
            public string Payload { get; set; }
            public DateTime RquestAt { get; set; }
            public string Response { get; set; }
            public string ResponseCode { get; set; }
            public DateTime RespondedAt { get; set; }
        }
    }
}
