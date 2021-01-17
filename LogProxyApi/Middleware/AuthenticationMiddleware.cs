using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LogProxyApi.Middleware
{
    /// <summary>
    /// Middleware to handle basic authentication
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        // user and password need to be encripted and outsourced
        private readonly string _username;
        private readonly string _password;

        /// <summary>
        /// Contructor of AuthenticationMiddleware
        /// </summary>
        /// <param name="next">Next Handler</param>
        /// <param name="configuration">App Configuration</param>
        public AuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            var authenticationConfig = configuration.GetSection("Authentication");
            _username = authenticationConfig["User"];
            _password = authenticationConfig["Password"];
        }

        /// <summary>
        /// Checks for basic authentication before proceeding with next tasks
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                if (username == _username && password == _password)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401; //Unauthorized
                    await context.Response.WriteAsync("You're not authorized");
                    return;
                }
            }
            else
            {
                // no authorization header
                context.Response.StatusCode = 401; //Unauthorized
                await context.Response.WriteAsync("Authorization header missing");
                return;
            }
        }
    }
}
