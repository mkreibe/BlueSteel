using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using BlueSteel.Actuator;

namespace BlueSteel.Extensions
{
    /// <summary>
    /// Defines the actuator application builder extensions.
    /// </summary>
    public static class ActuatorApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the actuators.
        /// </summary>
        /// <param name="builder">Defines the action builder.</param>
        /// <param name="config">The configuration to set.</param>
        public static void UseActuators(this IApplicationBuilder builder, IConfigurationSection config)
        {
            bool enable = false;
            List<ActuatorDefinition> actuators = new List<ActuatorDefinition>();

            foreach (var item in from sysConfig in config.GetChildren() select new { Name = sysConfig.Key, StringValue = sysConfig.Value, Children = sysConfig.GetChildren() })
            {
                switch (item.Name)
                {
                    case "Enable":
                        {
                            /// Check if the enable flag is set.
                            if (!Boolean.TryParse(item.StringValue, out enable))
                            {
                                enable = false;
                            }
                            break;
                        }
                    case "Routes":
                        {
                            /* Decode this giant!
                             * ----------------------------
                             *   "Routes": [
                             *     {
                             *       "Type": "HealthActuator",  // Required!
                             *       "Path": "/health",         // Optional
                             *       "Settings": {              // Optional
                             *         "IncludeVersion": true,  // These should be switches, string or numeric values only.
                             *         "IncludeUpTime": true,
                             *         "IncludeAccesses": true
                             *       }
                             *     }
                             *   ]
                             *   
                             *   
                             */

                            foreach (var route in from obj in item.Children select new { RouteSettings = obj.GetChildren() })
                            {
                                string type = null;
                                string path = null;
                                Dictionary<string, object> settings = new Dictionary<string, object>();
                                foreach(var routeSetting in from setting in route.RouteSettings select new { Name = setting.Key, StringValue = setting.Value, Children = setting.GetChildren() })
                                {
                                    switch(routeSetting.Name)
                                    {
                                        case "Type": type = routeSetting.StringValue; break;
                                        case "Path": path = routeSetting.StringValue; break;
                                        case "Settings":
                                            {
                                                foreach(var settingItem in from setting in routeSetting.Children select new { Name = setting.Key, StringValue = setting.Value})
                                                {
                                                    object value = null;

                                                    // try to parse an bool, then an int... if it is not those then set the value to the string... and if the string
                                                    // is empty or whitespace, set it to null.
                                                    bool boolVal;
                                                    long intVal;
                                                    if (bool.TryParse(settingItem.StringValue, out boolVal))
                                                    {
                                                        value = boolVal;
                                                    }
                                                    else if (long.TryParse(settingItem.StringValue, out intVal))
                                                    {
                                                        value = intVal;
                                                    }
                                                    else if (!string.IsNullOrWhiteSpace(settingItem.StringValue))
                                                    {
                                                        value = settingItem.StringValue;
                                                    }

                                                    if(value != null) {
                                                        settings.Add(settingItem.Name, value);
                                                    }
                                                }
                                                break;
                                            }
                                        default: throw new NotSupportedException($"Unsupported configuration key: {routeSetting.Name}");
                                    }
                                }

                                actuators.Add(new ActuatorDefinition(type, path, settings));
                            }

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            if (enable && (actuators.Count > 0))
            {
                builder.Properties.Add(ActuatorManager.PROPERTY_KEY, new ActuatorManager(actuators));
            }
        }
    }
}
