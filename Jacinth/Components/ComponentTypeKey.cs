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
            // ReSharper disable once StaticMemberInGenericType
            // Intentionally trying to get one Key per generic type, so that generic lookups are a matter of method-call -> member-access
            public static ComponentTypeKey Key { get; private set; }

            static StaticKey()
            {
                var targetType = typeof(T);

                // Walk the inheritance tree as far as requested by Inherit Component Key flags
                while (targetType.GetCustomAttribute<InheritComponentKeyAttribute>(false) != null)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    // This should never be a null because base Component type Component does not have the InheritComponentKeyAttribute
                    targetType = targetType.BaseType;
                }

                Key = new ComponentTypeKey(targetType);
            }
        }

        // Cache a hash for quick access
        private readonly int _hash;

        /// <summary>
        /// Type that this ComponentKey is bound against, used for debugging purposes.
        /// </summary>
        public Type TargetType { get; private set; }

        internal ComponentTypeKey(Type targetType)
        {
            TargetType = targetType;

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
        /// Gets a ComponentTypeKey for a given type
        /// </summary>
        /// <param name="targetType">The Type of Component to get a Key for</param>
        /// <returns>The ComponentTypeKey associated with the given type</returns>
        public static ComponentTypeKey GetKey(Type targetType)
        {
            // Look up the given key via Reflection
            return typeof(StaticKey<>)
                .MakeGenericType(targetType)
                .GetProperty("Key")
                .GetValue(null)
                as ComponentTypeKey;
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
