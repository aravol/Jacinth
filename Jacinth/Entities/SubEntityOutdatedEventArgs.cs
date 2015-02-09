using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacinth.Entities
{
    /// <summary>
    /// Arguments for when a SubEntity goes out of date and needs to be cleaned up
    /// </summary>
    public class SubEntityOutdatedEventArgs
    {
        private readonly Entity _entity;

        /// <summary>
        /// The Entity being targeted
        /// </summary>
        public Entity Entity { get { return _entity; } }

        internal SubEntityOutdatedEventArgs(Entity entity)
        {
            _entity = entity;
        }
    }
}
