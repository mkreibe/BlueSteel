using BlueSteel.Actuators.Health;

namespace BlueSteel.Actuators
{
    /// <summary>
    /// Defines an service.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Get the status code.
        /// </summary>
        HealthStatusCode StatusCode
        {
            get;
        }

        /// <summary>
        /// Get the service name.
        /// </summary>
        string Name
        {
            get;
        }
    }
}