using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jacinth.Components;

namespace Jacinth.Entities
{
    /// <summary>
    /// Args encapsulating the event fired when a Component is removed from an Entity
    /// </summary>
    public class ComponentRemovedEventArgs : EventArgs
    {
        private readonly Entity _entity;
        private readonly ComponentTypeKey _componentKey;

        public Entity Entity { get { return _entity; } }
        public ComponentTypeKey ComponentKey { get { return _componentKey; } }

        public ComponentRemovedEventArgs(Entity entity, ComponentTypeKey key)
        {
            _entity = entity;
            _componentKey = key;
        }
    }
}
