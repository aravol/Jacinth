using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jacinth.Components;

namespace Jacinth.Entities
{
    /// <summary>
    /// Arguments for when a Component is added to an Entity
    /// </summary>
    public class ComponentAddedEventArgs : EventArgs
    {
        private readonly Component _component;
        private readonly Entity _entity;

        /// <summary>
        /// Gets the Component being Added
        /// </summary>
        public Component Component { get { return _component; } }

        /// <summary>
        /// Gets the Entity to which a Component is being added
        /// </summary>
        public Entity Entity { get { return _entity; } }

        internal ComponentAddedEventArgs(Entity entity, Component component)
        {
            _entity = entity;
            _component = component;
        }
    }
}
