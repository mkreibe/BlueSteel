using System;
using BlueSteel.Actuators.Health;

namespace BlueSteel.Services
{
    /// <summary>
    /// Defines the health service.
    /// </summary>
    public interface IHealthService : IActuatorService
    {
        /// <summary>
        /// The start time.
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Get the current health status.
        /// </summary>
        HealthStatus Status { get; }

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="code">The new status code.</param>
        /// <param name="details">The status details.</param>
        void SetStatus(HealthStatusCode code, string details);
    }
}
