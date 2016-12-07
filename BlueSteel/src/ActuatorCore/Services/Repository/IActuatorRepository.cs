using Microsoft.Extensions.Configuration;

namespace BlueSteel.Services
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
        void AddActuator<T>(T actuator, IConfigurationSection config) where T : IActuator;

        /// <summary>
        /// Check if the actuator exists.
        /// </summary>
        /// <param name="route">The route to test for.</param>
        /// <returns>Returns true if the route exists in this service.</returns>
        bool ContainsRoute(string route);

        /// <summary>
        /// Get the actuator by route.
        /// </summary>
        /// <param name="route">The route to the actuator to return.</param>
        /// <returns>Returns the actuator.</returns>
        IActuator GetActuatorByRoute(string route);
    }
}
