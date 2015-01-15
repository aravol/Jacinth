using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacinth.Entities
{
    /// <summary>
    /// Arguments for the event called when a SubEntity goes out of date and needs to be cleaned up
    /// </summary>
    public class SubEntityOutdatedEventArgs
    {
        private readonly Entity _entity;

        public Entity Entity { get { return _entity; } }

        public SubEntityOutdatedEventArgs(Entity entity)
        {
            _entity = entity;
        }
    }
}
