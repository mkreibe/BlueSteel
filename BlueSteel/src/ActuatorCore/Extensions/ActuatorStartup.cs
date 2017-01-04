using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using BlueSteel.Actuators.Health;
using BlueSteel.Actuators.Env;
using BlueSteel.Actuators.Middleware;

namespace BlueSteel.Actuators.Extensions
{
    internal class ActuatorStartup
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
            services.AddSingleton<IActuatorRepository, ActuatorRepository>((provider) => {
                return new ActuatorRepository();
            });

            services.AddSingleton<IActuatorRouter, ActuatorRepository>((provider) => {
                return provider.GetService<IActuatorRepository>() as ActuatorRepository;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            // Add the actuator.
            app.UseActuator<HealthActuator>(Configuration.GetSection("Management:Actuators:Health"), (config) => new HealthActuator());
            app.UseActuator<EnvActuator>(Configuration.GetSection("Management:Actuators:Env"));
        }
    }
}
