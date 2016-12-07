using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueSteel.Actuators
{
    /// <summary>
    /// Defines the actuator.
    /// </summary>
    /// <typeparam name="T">The actuator return type.</typeparam>
    public abstract class BaseActuator<T> : IActuator where T : new()
    {
        /// <summary>
        /// Holds the route.
        /// </summary>
        private string route;

        /// <summary>
        /// Get or set the route to this object.
        /// </summary>
        public string Route
        {
            get
            {
                if(this.route == null)
                {
                    string name = this.GetType().Name;

                    if(name.EndsWith("Actuator"))
                    {
                        name = name.Substring(0, name.Length - 8);
                    }

                    this.Route = name.ToLower();
                }

                return this.route;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                // make sure the value starts with a '/'.
                string route = value;
                if(!route.StartsWith("/"))
                {
                    route = "/" + route;
                }

                this.route = route;
            }
        }

        /// <summary>
        /// Invoke the actuator.
        /// </summary>
        /// <returns>Returns the task.</returns>
        public abstract T Invoke();

        /// <summary>
        /// Invoke the actuator.
        /// </summary>
        /// <returns>Returns the task.</returns>
        async Task<object> IActuator.Invoke()
        {
            return await Task.Run<T>(() => this.Invoke());
        }
    }
}
