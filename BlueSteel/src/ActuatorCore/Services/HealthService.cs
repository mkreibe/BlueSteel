using System;
using BlueSteel.Actuators.Health;

namespace BlueSteel.Services
{
    /// <summary>
    /// Defines the health service implementation.
    /// </summary>
    public class HealthService : IHealthService
    {
        /// <summary>
        /// The start time.
        /// </summary>
        public DateTime StartTime
        {
            get;
        } = DateTime.UtcNow;

        /// <summary>
        /// Get the current health status.
        /// </summary>
        public HealthStatus Status
        {
            get;
            set;
        } = new HealthStatus() { Code = HealthStatusCode.Unknown };

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="code">The new status code.</param>
        /// <param name="details">The status details.</param>
        public void SetStatus(HealthStatusCode code, string details)
        {
            this.Status = new HealthStatus()
            {
                Code = code,
                Description = details
            };
        }
    }
}
