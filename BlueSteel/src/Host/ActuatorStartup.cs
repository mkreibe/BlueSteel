using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using BlueSteel.Actuators.Health;
using BlueSteel.Actuators.Env;
using BlueSteel.Services;

namespace BlueSteel.Extensions
{
    public class ActuatorStartup
    {
        public ActuatorStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add the actuators with the specified helper services. In this case the health service
            services.AddActuators(Configuration.GetSection("Management"), typeof(HealthService));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Add the actuator.
            app.UseActuator<HealthActuator>(Configuration.GetSection("Management:Actuators:Health"));
            app.UseActuator<EnvActuator>(Configuration.GetSection("Management:Actuators:Health"));
        }
    }
}
