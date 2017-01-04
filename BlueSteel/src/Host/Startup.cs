using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BlueSteel.Host.Data;
using BlueSteel.Actuators;
using BlueSteel.Actuators.Extensions;
using BlueSteel.Actuators.Env;
using BlueSteel.Actuators.Health;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BlueSteel.Host
{
    /// <summary>
    /// Defines the start of the application.
    /// </summary>
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IValueRepository, MemoryValueRepository>((provider) =>
            {
                MemoryValueRepository repo = new MemoryValueRepository()
                {
                    [1] = "value1",
                    [2] = "value2",
                    [42] = "Answer to the Ultimate Question of Life, the Universe, and Everything"
                };

                return repo;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // Update the health status.
            app.UpdateActuator<HealthActuator>((actuator) => {
                actuator.AddService(new SimpleService
                {
                    StatusCode = HealthStatusCode.Up,
                    Name = "Simple"
                });

                actuator.AddService(new ExtendedService
                {
                    StatusCode = HealthStatusCode.Up,
                    Name = "Extended",
                    ExtendedProperties = new Dictionary<string, JToken>()
                    {
                        ["extended"] = JValue.CreateString("value")
                    }
                });
            });
        }
    }
}
