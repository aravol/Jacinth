using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacinth.Entities
{
    /// <summary>
    /// Batches the creation of several Entities, so as to be added to the World en masse
    /// </summary>
    public sealed class EntityBatch : IDisposable
    {
        public JacinthWorld World { get; }
        public HashSet<Entity> Entities { get; }
        
        internal EntityBatch(JacinthWorld world)
        {
            World = world;
            Entities = new HashSet<Entity>();
        }
        
        /// <summary>
        /// Creates an Entity in the World and enables it when this batch is Disposed
        /// </summary>
        /// <returns>
        /// The newly created Entity.
        /// </returns>
        public Entity PrepareEntity()
        {
            var result = new Entity(World);
            result.Enabled += World.EntityOnEnabled;
            Entities.Add(result);
            return result;
        }

        /// <summary>
        /// Dispoaes this batch and enables all Entities created thusly
        /// </summary>
        public void Dispose()
        {
            foreach (var e in Entities) e.Enable();
        }
    }
}
