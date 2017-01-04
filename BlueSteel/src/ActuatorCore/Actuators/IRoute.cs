using System.Threading.Tasks;

namespace BlueSteel.Actuators
{
    /// <summary>
    /// Defines the actuator.
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        /// Get the actuator route.
        /// </summary>
        string Route { get; set; }

        /// <summary>
        /// Invoke the actuator.
        /// </summary>
        /// <returns>Returns the task.</returns>
        Task<object> Invoke();
    }
}
