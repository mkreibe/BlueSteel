using System.Runtime.Serialization;

namespace BlueSteel.Actuators.Health
{
    /// <summary>
    /// Defines the health statuses.
    /// </summary>
    public enum HealthStatusCode
    {
        /// <summary>
        /// Defines the unknown status.
        /// </summary>
        [EnumMember(Value = "UNKNOWN")]
        Unknown,

        /// <summary>
        /// Defines the up status.
        /// </summary>
        [EnumMember(Value = "UP")]
        Up,

        /// <summary>
        /// Defines the down status.
        /// </summary>
        [EnumMember(Value = "DOWN")]
        Down
    }
}
