using System.Collections.Generic;

namespace BlueSteel.Actuators.Health
{
    /// <summary>
    /// Holds the health data.
    /// </summary>
    public class HealthData
    {
        /// <summary>
        /// Holds the health status.
        /// </summary>
        public HealthStatus Status
        {
            get;
            set;
        } = new HealthStatus();

        /// <summary>
        /// Get the details.
        /// </summary>
        public Dictionary<string, object> Details
        {
            get;
        } = new Dictionary<string, object>();
    }
}
