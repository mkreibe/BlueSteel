using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using BlueSteel.Actuators.Extensions;

namespace BlueSteel.Actuators.Middleware
{

    /// <summary>
    /// Defin es the actuator service implementation.
    /// </summary>
    internal class ActuatorRepository : IActuatorRepository, IActuatorRouter
   
    {
        /// <summary>
        /// Holds the actuators.
        /// </summary>
        private Dictionary<string, IRoute> actuators; 

        /// <summary>
        /// Create an instance of the service.
        /// </summary>
        public ActuatorRepository()
        {
            this.actuators = new Dictionary<string, IRoute>();
        }

        /// <summary>
        /// Add the actuator to the service.
        /// </summary>
        /// <typeparam name="T">The actuators type.</typeparam>
        /// <param name="actuator">The actuator to add.</param>
        /// <param name="config">The configuration for the actuator.</param>
        public void AddRoute<T>(T actuator, IConfigurationSection config) where T : IRoute
        {
            this.actuators.Add(actuator.Route, actuator);
        }

        /// <summary>
        /// Call the actuator service.
        /// </summary>
        public void CallActuator<T>(Action<T> operation) where T : class, IActuator
        {
            if(operation == null) {
                throw new NullReferenceException(nameof(operation));
            }

            foreach(T t in from a in this.actuators.Values
                           where typeof(T).IsAssignableFrom(a.GetType())
                           select a as T)
            {
                operation(t);
            }
        }

        /// <summary>
        /// Check if the actuator exists.
        /// </summary>
        /// <param name="route">The route to test for.</param>
        /// <returns>Returns true if the route exists in this service.</returns>
        public bool ContainsRoute(string route)
        {
            return this.actuators.ContainsKey(route);
        }

        /// <summary>
        /// Get the actuator by route.
        /// </summary>
        /// <param name="route">The route to the actuator to return.</param>
        /// <returns>Returns the actuator.</returns>
        public IRoute GetActuatorByRoute(string route)
        {
            return this.actuators[route] as IRoute;
        }
    }
}
