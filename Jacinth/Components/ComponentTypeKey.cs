using System;
using System.Reflection;

namespace Jacinth.Components
{
    /// <summary>
    /// Component Keys, which form the keys used to look up Components
    /// </summary>
    // These are implemented as a sealed class instead of a struct due to lifetime concerns
    //  - a ComponentKey should exist for the lifetime of the entire application
    public sealed class ComponentTypeKey : IEquatable<ComponentTypeKey>
    {
        /// <summary>
        /// Statically caches and accesses the Component Key associated with the type T
        /// </summary>
        private static class StaticKey<T>
            where T : Component
        {
            public static ComponentTypeKey Key { get; private set; }

            static StaticKey()
            {
                Key = typeof(T)
                    .GetCustomAttribute<KeyingComponentAttribute>(true)
                    .Key;
            }
        }

        // Cache a hash for quick access
        private readonly int _hash;

        /// <summary>
        /// Type that this ComponentKey is bound against, used for debugging purposes.
        /// This value is NOT enforced by Jacinth and may not accurately reflect the type actually decorated with the corresponding KeyingComponentAttribute
        /// </summary>
        public Type TargetType { get; private set; }

        internal ComponentTypeKey(Type targetType)
        {
            TargetType = targetType;

            // Do NOT check for the KeyingComponentAttribute being the correct instance here -
            // Doing so will reinstantiate the KeyingComponentAttribute on the given type and result in a StackOverflowException

            // Account for the target type being null
            _hash = targetType == null
                ? Guid.NewGuid().GetHashCode()  // The best we can do to create a unique Hash: get the hash of a guaranteed-unique object
                : TargetType.GetHashCode();     // The has code is a shortcut used in most of .NET - since we use referential equality, hash collisions are moot
        }

        /// <summary>
        /// Gets a ComponentTypeKey from a given type
        /// </summary>
        /// <typeparam name="T">The Type of Component to get a Key for</typeparam>
        /// <returns>The ComponentTypeKey associated with the given type</returns>
        public static ComponentTypeKey GetKey<T>()
            where T : Component
        {
            return StaticKey<T>.Key;
        }

        /// <summary>
        /// Generates a Hash Code for this instance
        /// </summary>
        public override int GetHashCode()
        {
            return _hash;
        }

        /// <summary>
        /// Determines equality between this instance and another
        /// </summary>
        public bool Equals(ComponentTypeKey other)
        {
            // Becuase they are tied to a type via an Attribute, logical and referential equality should always be the same for ComponentKeys
            return ReferenceEquals(this, other);
        }
    }
}
