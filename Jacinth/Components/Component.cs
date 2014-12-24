using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jacinth.Entities;

namespace Jacinth.Components
{
    /// <summary>
    /// Represents a Component, which attaches to an Entity to provide data for a specific behaviour
    /// </summary>
    public abstract class Component
    {
        public Entity Entity { get; internal set; }
    }
}
