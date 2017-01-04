using BlueSteel.Actuators.Health;

namespace BlueSteel.Actuators
{
    /// <summary>
    /// Defines a simple service.
    /// </summary>
    public class SimpleService : IService
    {
        /// <summary>
        /// Get the status code.
        /// </summary>
        public HealthStatusCode StatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Get the service name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}