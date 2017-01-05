using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BlueSteel.Actuators.Middleware;

namespace BlueSteel.Actuators.Extensions
{
    /// <summary>
    /// Defines the service configuration extensions.
    /// </summary>
    public static class ActuatorExtensions
    {
        /// <summary>
        /// Use the management default port.
        /// </summary>
        public const int DEFAULT_MANAGEMENT_PORT = 5001;

        /// <summary>
        /// Use the management subsystem.
        /// </summary>
        /// <param name="builder">Defines the application builder.</param>
        /// <returns></returns>
        public static IWebHostBuilder UseManagementHost(this IWebHostBuilder builder, string url = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if(string.IsNullOrWhiteSpace(url))
            {
                int nextPort = DEFAULT_MANAGEMENT_PORT;

                string env = Environment.GetEnvironmentVariable("MANAGEMENT_PORT");
                if(string.IsNullOrWhiteSpace(env))
                {
                    // Check for the 'PORT' variable and increment that by one as the default
                    // management port.
                    env = Environment.GetEnvironmentVariable("PORT");
                    if(!string.IsNullOrWhiteSpace(env) && int.TryParse(env, out nextPort))
                    {
                        ++nextPort;
                    }
                }
                else
                {
                    int.TryParse(env, out nextPort);
                }

                url = $"http://*:{nextPort}/";
            }

            var management = new WebHostBuilder()
                .UseUrls(url)
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<ActuatorStartup>()
                .Build();

            if(management != null)
            {
                // TODO: We need to switch to a management port for these artifacts.
                string data = builder.GetSetting("");
                IServiceProvider provider = management.Services;
                IActuatorRepository repo = provider.GetService(typeof(IActuatorRepository)) as IActuatorRepository;
                IActuatorRouter router = provider.GetService(typeof(IActuatorRouter)) as IActuatorRouter;

                builder.ConfigureServices((services) => {
                    services.AddSingleton<IActuatorRepository>((p) => repo);
                    services.AddSingleton<IActuatorRouter>((p) => router);
                });

                // Start the management port.
                management.Start();
            }

            return builder;
        }

        /// <summary>
        /// Get the actuator service.
        /// </summary>
        /// <typeparam name="T">The service to return.</typeparam>
        /// <param name="builder">The builder to use.</param>
        /// <returns>Returns the service</returns>
        public static void UpdateActuator<T>(this IApplicationBuilder builder, Action<T> operation) where T : class, IActuator
        {
            IActuatorRepository repo = builder.ApplicationServices.GetRequiredService<IActuatorRepository>();
            if(repo != null) {
                repo.CallActuator<T>(operation);
            }
        }

        /// <summary>
        /// Use the actuators.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="config">The configuration for the provided actuator.</param>
        /// <param name="factory">The factory used to build the service.</param>
        /// <returns>Returns the builder.</returns>
        public static IApplicationBuilder UseActuator<T>(this IApplicationBuilder app, IConfigurationSection config, Func<IConfigurationSection, T> factory = null) where T : IRoute
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (factory == null)
            {
                factory = (c) => {
                    T instance = default(T);

                    Type type = typeof(T);

                    ConstructorInfo constructor = null;
                    List<object> parameters = new List<object>();

                    foreach(ConstructorInfo con in type.GetConstructors()) {
                        parameters.Clear();
                        constructor = con;

                        foreach (ParameterInfo param in con.GetParameters())
                        {
                            object serviceParam = app.ApplicationServices.GetService(param.ParameterType);
                            if(serviceParam != null)
                            {
                                parameters.Add(serviceParam);
                            }
                            else
                            {
                                // We don't know how to process the parameter, so exit.
                                constructor = null;
                                break;
                            }
                        }

                        if (constructor != null && parameters.Count == constructor.GetParameters().Count())
                        {
                            break;
                        }
                    }

                    if(constructor != null && parameters.Count == constructor.GetParameters().Count())
                    {
                        instance = (T)constructor.Invoke(parameters.ToArray());
                    }

                    if(instance != null)
                    {
                        foreach (PropertyInfo prop in type.GetProperties())
                        {
                            string val = config[prop.Name];
                            if (!string.IsNullOrWhiteSpace(val))
                            {
                                object v = Convert.ChangeType(val, prop.PropertyType);
                                prop.SetValue(instance, v);
                            }
                        }
                    }

                    return instance;
                };
            }

            app.ApplicationServices
                .GetService<IActuatorRepository>()
                .AddRoute<T>(factory(config), config);

            return app.UseMiddleware<ActuatorMiddleware>();
        }
    }
}