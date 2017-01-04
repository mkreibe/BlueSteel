using System;
using Microsoft.Extensions.Configuration;

namespace BlueSteel.Actuators.Extensions
{
    /// <summary>
    /// Defines the actuator service.
    /// </summary>
    public interface IActuatorRepository
    {
        /// <summary>
        /// Add the actuator to the service.
        /// </summary>
        /// <typeparam name="T">The actuators type.</typeparam>
        /// <param name="actuator">The actuator to add.</param>
        /// <param name="config">The configuration for the actuator.</param>
        void AddRoute<T>(T actuator, IConfigurationSection config) where T : IRoute;

        /// <summary>
        /// Call the actuator service.
        /// </summary>
        void CallActuator<T>(Action<T> operation) where T : class, IActuator;
    }
}
