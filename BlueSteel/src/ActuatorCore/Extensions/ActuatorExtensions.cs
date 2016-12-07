using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using BlueSteel.Middleware;
using BlueSteel.Services;
using Microsoft.AspNetCore.Hosting;

namespace BlueSteel.Extensions
{
    /// <summary>
    /// Defines the service configuration extensions.
    /// </summary>
    public static class ActuatorExtensions
    {
        /// <summary>
        /// Use the management subsystem.
        /// </summary>
        /// <param name="builder">Defines the application builder.</param>
        /// <returns></returns>
        public static IWebHostBuilder UseManagementHost(this IWebHostBuilder builder, IWebHost managementHost)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if(managementHost != null)
            {
                // TODO: We need to switch to a management port for these artifacts.
                string data = builder.GetSetting("");
                IServiceProvider provider = managementHost.Services;
                provider as ServiceProviderServiceExtensions

                // Start the management port.
                managementHost.Start();
            }

            return builder;
        }

        /// <summary>
        /// Build the management.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebHost BuildManagement<T>(this IWebHostBuilder builder) where T : class
        {
            return builder
                .UseStartup<T>()
                .Build();
        }

        /// <summary>
        /// Add the actuator to the service collection.
        /// </summary>
        /// <param name="collection">The collection to manipulate.</param>
        /// <returns></returns>
        public static IServiceCollection AddActuators(this IServiceCollection collection, IConfigurationSection section, params Type[] supportingServices)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach(Type implementation in supportingServices) {
                Type service = implementation.GetTypeInfo().ImplementedInterfaces.First();
                collection.AddSingleton(service, implementation);
            }

            return collection.AddSingleton<IActuatorRepository, ActuatorRepository>();
        }

        /// <summary>
        /// Get the actuator service.
        /// </summary>
        /// <typeparam name="T">The service to return.</typeparam>
        /// <param name="builder">The builder to use.</param>
        /// <returns>Returns the service</returns>
        public static void GetActuatorService<T>(this IApplicationBuilder builder, Action<T> operation) where T : IActuatorService
        {
            T service = builder.ApplicationServices.GetService<T>();
            if(service != null)
            {
                operation(service);
            }
        }

        /// <summary>
        /// Use the actuators.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="config">The configuration for the provided actuator.</param>
        /// <param name="factory">The factory used to build the service.</param>
        /// <returns>Returns the builder.</returns>
        public static IApplicationBuilder UseActuator<T>(this IApplicationBuilder app, IConfigurationSection config, Func<IConfigurationSection, T> factory = null) where T : IActuator
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

            IActuatorRepository service = app.ApplicationServices.GetService<IActuatorRepository>();
            service.AddActuator<T>(factory(config), config);

            return app.UseMiddleware<ActuatorMiddleware>();
        }
    }
}