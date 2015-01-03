using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jacinth.Entities;
using Jacinth.Components;

namespace Jacinth
{
    /// <summary>
    /// Represents a key used to look up a Compoenent of a specific keyed type and a specific Entity
    /// </summary>
    internal struct EntityComponentKey :
        IEquatable<EntityComponentKey>
    {
        private readonly int _hashCode;
        private readonly Entity _entity;
        private readonly ComponentTypeKey _componentType;

        public Entity Entity { get { return _entity; } }
        public ComponentTypeKey ComponentType { get { return _componentType; } }

        public EntityComponentKey(Entity entity, ComponentTypeKey componentType)
        {
            _entity = entity;
            _componentType = componentType;

            _hashCode = entity.GetHashCode()
                ^ componentType.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public bool Equals(EntityComponentKey other)
        {
            return Entity.Equals(other.Entity)
                && ComponentType.Equals(other.ComponentType);
        }
    }
}
