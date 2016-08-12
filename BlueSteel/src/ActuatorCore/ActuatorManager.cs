using System.Collections.Generic;
using System.Diagnostics;

namespace BlueSteel.Actuator
{
    /// <summary>
    /// Defines the actuator manager.
    /// </summary>
    public class ActuatorManager
    {
        /// <summary>
        /// Holds the property key.
        /// </summary>
        public const string PROPERTY_KEY = "bluesteel.actuatorsmanager";

        /// <summary>
        /// Create an instance of the actuator manager.
        /// </summary>
        /// <param name="section">The configuration.</param>
        internal ActuatorManager(List<ActuatorDefinition> actuators)
        {
            Debugger.Break();
        }
    }
}
