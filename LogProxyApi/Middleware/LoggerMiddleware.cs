using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LogProxyApi.Middleware
{
    /// <summary>
    /// Middleware to handle basic logging
    /// </summary>
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor of LoggerMiddleware
        /// </summary>
        /// <param name="next">Next Handler</param>
        /// <param name="loggerFactory">Logger Factory</param>
        public LoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggerMiddleware>();
        }

        /// <summary>
        /// Wraps next handlers with basic logs
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"[{context.TraceIdentifier}] HTTP-{context.Request?.Method} Call Received on URL \"{context.Request?.Path.Value}\"");
            try
            {
                await _next.Invoke(context);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"[{context.TraceIdentifier}] Internal error occured!");
            }
            finally
            {
                _logger.LogInformation($"[{context.TraceIdentifier}] Call handled with status code: {context.Response?.StatusCode}");
            }
        }
    }
}
