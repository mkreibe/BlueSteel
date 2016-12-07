using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueSteel.Actuators.Env
{
    /// <summary>
    /// Defines the environment variable actuator.
    /// </summary>
    public class EnvActuator : BaseActuator<Dictionary<string, string>>
    {
        public override Dictionary<string, string> Invoke()
        {
            Dictionary<string, string> envs = new Dictionary<string, string>();

            foreach(DictionaryEntry entry in Environment.GetEnvironmentVariables())
            {
                envs.Add(entry.Key.ToString(), entry.Value.ToString());
            }

            return envs;
        }
    }
}
