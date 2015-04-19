using Jacinth.Entities;

namespace Jacinth.Components
{
    /// <summary>
    /// Represents a Component, which attaches to an Entity to provide data for a specific behaviour
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// The Entity this Component is attached to
        /// </summary>
        public Entity Entity { get; internal set; }
    }
}
