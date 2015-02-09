using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jacinth.Components;

namespace Jacinth.Entities
{
    /// <summary>
    /// Arguments for when a Component is removed from an Entity
    /// </summary>
    public class ComponentRemovedEventArgs : EventArgs
    {
        private readonly Entity _entity;
        private readonly ComponentTypeKey _componentTypeKey;
        private readonly Component _component;
        private readonly bool _eraseAll;

        /// <summary>
        /// Gets the Entity from which a Component has been Removed
        /// </summary>
        public Entity Entity { get { return _entity; } }

        /// <summary>
        /// Gets whether all Components are being removed from the target Entity
        /// </summary>
        public bool EraseAll { get { return _eraseAll; } }

        /// <summary>
        /// Gets the ComponentTypeKey of the removed Component, or null if removing all Components
        /// </summary>
        public ComponentTypeKey ComponentTypeKey { get { return _componentTypeKey; } }

        /// <summary>
        /// Gets the Component being removed, or null if removing all Components
        /// </summary>
        public Component Component { get { return _component; } }

        internal ComponentRemovedEventArgs(Entity entity, ComponentTypeKey key = null, Component component = null)
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
