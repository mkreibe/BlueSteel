using BlueSteel.Actuators;
using BlueSteel.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueSteel.Middleware
{
    /// <summary>
    /// Defines the middle ware for the actuarots.
    /// </summary>
    public class ActuatorMiddleware
    {

        /// <summary>
        /// Holds the logger.
        /// </summary>
        private ILogger<ActuatorMiddleware> Logger { get; set; }

        /// <summary>
        /// Holds the service.
        /// </summary>
        private IActuatorRepository Service { get; set; }

        /// <summary>
        /// Holds the next thing to do.
        /// </summary>
        private RequestDelegate Next { get; set; }

        /// <summary>
        /// Create an instance fo the actuarot middleware.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public ActuatorMiddleware(RequestDelegate next, IActuatorRepository service, ILoggerFactory loggerFactory)
        {
            this.Next = next;
            this.Service = service;
            this.Logger = loggerFactory.CreateLogger<ActuatorMiddleware>();
        }

        /// <summary>
        /// Invoke the middelware.
        /// </summary>
        /// <param name="context">The context to run.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            int port = context.Connection.LocalPort;
            string route = context.Request.Path.Value;
            if (this.Service != null && this.Service.ContainsRoute(route))
            {
                this.Logger.LogInformation($"Run Actuator for route: {route}");
                IActuator actuator = this.Service.GetActuatorByRoute(route);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(await actuator.Invoke()));
            }
            else
            {
                await this.Next.Invoke(context);
            }
        }
    }
}
