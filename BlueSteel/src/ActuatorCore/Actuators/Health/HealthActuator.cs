using System.Collections.Generic;

namespace BlueSteel.Actuators.Health
{
    /// <summary>
    /// Defines the health actuator.
    /// </summary>
    public class HealthActuator : BaseActuator<HealthStatus>
    {

        /// <summary>
        /// Get the services collection.
        /// </summary>
        internal Dictionary<string, IService> Services
        {
            get;
        } = new Dictionary<string, IService>();

        /// <summary>
        /// Invoke the actuator.
        /// </summary>
        /// <returns>Returns the health status task.</returns>
        internal override HealthStatus InvokeRoute()
        {
            return new HealthStatus(this.Services.Values);
        }

        /// <summary>
        /// Add the service.
        /// </summary>
        /// <param name="service">The service to add.</param>
        public void AddService(IService service)
        {
            this.Services.Add(service.Name, service);
        }
    }
}
