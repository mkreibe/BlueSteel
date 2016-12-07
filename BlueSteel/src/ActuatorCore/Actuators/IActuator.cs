using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueSteel
{
    /// <summary>
    /// Defines the actuator.
    /// </summary>
    /// <typeparam name="T">The actuator return type.</typeparam>
    public interface IActuator
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
