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
        private readonly ComponentTypeKey _componentTypeKey;
        private readonly Component _component;
        private readonly bool _eraseAll;

        public Entity Entity { get { return _entity; } }
        public bool EraseAll { get { return _eraseAll; } }
        public ComponentTypeKey ComponentTypeKey { get { return _componentTypeKey; } }
        public Component Component { get { return _component; } }

        public ComponentRemovedEventArgs(Entity entity, ComponentTypeKey key = null, Component component = null)
        {
            _entity = entity;
            if (component == null || key == null)
            {
                _component = null;
                _componentTypeKey = null;
                _eraseAll = true;
            }
            else
            {
                _component = component;
                _componentTypeKey = key;
                _eraseAll = false;
            }
        }
    }
}
