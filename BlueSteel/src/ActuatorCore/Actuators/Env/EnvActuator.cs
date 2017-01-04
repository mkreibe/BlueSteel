using System;
using System.Collections;
using System.Collections.Generic;

namespace BlueSteel.Actuators.Env
{
    /// <summary>
    /// Defines the environment variable actuator.
    /// </summary>
    public class EnvActuator : BaseActuator<Dictionary<string, string>>
    {
        internal override Dictionary<string, string> InvokeRoute()
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
