using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace BlueSteel.Actuators.Health
{
    /// <summary>
    /// Defines the health status.
    /// </summary>
    public class HealthStatus
    {
        /// <summary>
        /// Get or set the code.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public HealthStatusCode Code
        {
            get;
            set;
        } = HealthStatusCode.Unknown;

        /// <summary>
        /// Get or set the status description.
        /// </summary>
        public string Description
        {
            get;
            set;
        } = String.Empty;
    }
}
