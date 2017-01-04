using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlueSteel.Actuators.Middleware
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
        /// Holds the route.
        /// </summary>
        private IActuatorRouter Router { get; set; }

        /// <summary>
        /// Holds the next thing to do.
        /// </summary>
        private RequestDelegate Next { get; set; }

        /// <summary>
        /// Create an instance fo the actuarot middleware.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public ActuatorMiddleware(RequestDelegate next, IActuatorRouter router, ILoggerFactory loggerFactory)
        {
            this.Next = next;
            this.Router = router;
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
            if (this.Router != null && this.Router.ContainsRoute(route))
            {
                this.Logger.LogInformation($"Run Actuator for route: {route}");
                IRoute actuator = this.Router.GetActuatorByRoute(route);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(await actuator.Invoke()));
            }
            else
            {
                await this.Next.Invoke(context);
            }
        }
    }
}
