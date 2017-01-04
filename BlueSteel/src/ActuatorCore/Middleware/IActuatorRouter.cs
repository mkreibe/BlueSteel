namespace BlueSteel.Actuators.Middleware
{
    /// <summary>
    /// Defines the actuator service.
    /// </summary>
    public interface IActuatorRouter
    {

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
        IRoute GetActuatorByRoute(string route);

    }
}
