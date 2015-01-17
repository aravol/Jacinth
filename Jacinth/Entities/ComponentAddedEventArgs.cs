using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jacinth.Components;

namespace Jacinth.Entities
{
    public class ComponentAddedEventArgs : EventArgs
    {
        private readonly Component _component;
        private readonly Entity _entity;

        public Component Component { get { return _component; } }
        public Entity Entity { get { return _entity; } }

        public ComponentAddedEventArgs(Entity entity, Component component)
        {
            _entity = entity;
            _component = component;
        }
    }
}
