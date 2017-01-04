using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace BlueSteel.Actuators.Health
{
    /// <summary>
    /// Holds the health status.
    /// </summary>
    [JsonConverter(typeof(HealthStatusConverter))]
    public class HealthStatus
    {

        /// <summary>
        /// Create an instance of the status.
        /// </summary>
        public HealthStatus()
        {
            // No-Op
        }

        /// <summary>
        /// Create an instance of the status.
        /// </summary>
        public HealthStatus(IEnumerable<IService> services) : this()
        {
            if(services != null)
            {
                List<IService> local = this.Services as List<IService>;
                local.AddRange(services);
            }
        }

        /// <summary>
        /// Get the code.
        /// </summary>
        public HealthStatusCode StatusCode
        {
            get
            {
                // If there are no services, then the system is up, otherwise
                // it is down if any service is down.
                return this.Services.Any((service) => service.StatusCode == HealthStatusCode.Down) ?
                    HealthStatusCode.Down :
                    HealthStatusCode.Up;
            }
        }

        /// <summary>
        /// Get the 
        public IEnumerable<IService> Services
        {
            get;
        } = new List<IService>();
    }
}
