using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jacinth.Components
{
    /// <summary>
    /// Marks a Component as a Keying Component,
    /// which allows it to be used as a key for looking up Component on an Entity
    /// and prevents it from colliding with sibling-typed Components
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class KeyingComponentAttribute
        : Attribute
    {
        public ComponentTypeKey Key { get; private set; }

        /// <summary>
        /// Overridden. Provides a unique identifier for checking uniqueness against another KeyingComponentAttribute.
        /// </summary>
        public override object TypeId
        {
            get { return Key; }
        }

        /// <summary>
        /// Specifies a Keying Component using the targeted type.
        /// </summary>
        /// <param name="targetType">The targeted type. For debug purposes, this should be the type to which this Attribute is applied</param>
        public KeyingComponentAttribute(Type targetType)
        {
            // Do NOT Enforce the targetType as having this exact instance attached
            // Doing so will reinstantiate the attribute and result in a StackOverflowException

            Key = new ComponentTypeKey(targetType);
        }
    }
}
