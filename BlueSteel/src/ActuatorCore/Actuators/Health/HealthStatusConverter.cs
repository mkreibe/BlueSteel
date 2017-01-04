using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace BlueSteel.Actuators.Health
{
    /// <summary>
    /// Defines the health converter
    /// </summary>
    public class HealthStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            HealthStatus status = value as HealthStatus;

            StringEnumConverter converter = new StringEnumConverter();

            writer.WriteStartObject();
            writer.WritePropertyName("status");
            converter.WriteJson(writer, status.StatusCode, serializer);

            foreach(IService service in status.Services)
            {
                writer.WritePropertyName(service.Name);

                writer.WriteStartObject();
                writer.WritePropertyName("status");
                converter.WriteJson(writer, service.StatusCode, serializer);

                foreach(Dictionary<string, JToken> properties in
                    from prop in service.GetType().GetProperties()
                        let attr = prop.GetCustomAttribute<ServicePropertiesAttribute>()
                        let propType = prop.PropertyType
                    where
                        attr != null && typeof(Dictionary<string, JToken>).IsAssignableFrom(propType)
                    select (prop.GetValue(service) as Dictionary<string, JToken>))
                {
                    foreach(KeyValuePair<string, JToken> val in properties)
                    {
                        writer.WritePropertyName(val.Key);
                        serializer.Serialize(writer, val.Value);
                    }
                }

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}
