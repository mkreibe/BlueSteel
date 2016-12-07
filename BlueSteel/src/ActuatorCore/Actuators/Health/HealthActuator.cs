using BlueSteel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueSteel.Actuators.Health
{
    /// <summary>
    /// Defines the health actuator.
    /// </summary>
    public class HealthActuator : BaseActuator<HealthData>
    {
        /// <summary>
        /// Holds the status code.
        /// </summary>
        private IHealthService healthService;

        /// <summary>
        /// Create a service.
        /// </summary>
        /// <param name="healthService">The health service.</param>
        public HealthActuator(IHealthService healthService)
        {
            this.healthService = healthService;
        }

        /// <summary>
        /// Get or set the flag including the up time.
        /// </summary>
        public bool IncludeUpTime
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the flag including the start time.
        /// </summary>
        public bool IncludeStartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Invoke the actuator.
        /// </summary>
        /// <returns>Returns the health data task.</returns>
        public override HealthData Invoke()
        {
            HealthData data = new HealthData() {
                Status = this.healthService.Status
            };

            if (this.IncludeStartTime)
            {
                data.Details.Add("StartTime", $"{this.healthService.StartTime:o}");
            }

            if (this.IncludeUpTime)
            {
                TimeSpan span = DateTime.UtcNow - this.healthService.StartTime;
                data.Details.Add("UpTime", $"{span.TotalMilliseconds:F0}");
            }

            return data;
        }
    }
}
