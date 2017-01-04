using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BlueSteel.Actuators
{
    /// <summary>
    /// Defines a simple service.
    /// </summary>
    public class ExtendedService : SimpleService
    {
        /// <summary>
        /// Get the extended properties.
        /// </summary>
        [ServiceProperties]
        public Dictionary<string, JToken> ExtendedProperties
        {
            get;
            set;
        }
    }
}