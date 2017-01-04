using System;

namespace BlueSteel.Actuators
{
    /// <summary>
    /// Defines a service properties attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ServicePropertiesAttribute : Attribute
    {
        // No-Op
    }
}