using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BlueSteel.Services
{

    /// <summary>
    /// Defin es the actuator service implementation.
    /// </summary>
    internal class ActuatorRepository : IActuatorRepository
    {
        /// <summary>
        /// Holds the actuators.
        /// </summary>
        private Dictionary<string, IActuator> actuators;

        /// <summary>
        /// Create an instance of the service.
        /// </summary>
        public ActuatorRepository()
        {
            this.actuators = new Dictionary<string, IActuator>();
        }

        /// <summary>
        /// Add the actuator to the service.
        /// </summary>
        /// <typeparam name="T">The actuators type.</typeparam>
        /// <param name="actuator">The actuator to add.</param>
        /// <param name="config">The configuration for the actuator.</param>
        public void AddActuator<T>(T actuator, IConfigurationSection config) where T : IActuator
        {
            this.actuators.Add(actuator.Route, actuator);
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
        public IActuator GetActuatorByRoute(string route)
        {
            return this.actuators[route];
        }
    }
}
