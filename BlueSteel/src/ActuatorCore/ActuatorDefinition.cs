using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueSteel.Actuator
{
    /// <summary>
    /// Defines the actuator settings.
    /// </summary>
    public class ActuatorDefinition
    {
        private string path;
        private Dictionary<string, object> settings;
        private string type;

        public ActuatorDefinition(string type, string path, Dictionary<string, object> settings)
        {
            this.type = type;
            this.path = path;
            this.settings = settings;
        }
    }
}
