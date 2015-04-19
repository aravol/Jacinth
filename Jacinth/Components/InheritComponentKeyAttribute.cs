using System;

namespace Jacinth.Components
{
    /// <summary>
    /// Attribute to indicate the marked comopnent should use it's base class's ComponentTypeKey.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InheritComponentKeyAttribute : Attribute
    {
        // Nothing to do here but exist
    }
}
