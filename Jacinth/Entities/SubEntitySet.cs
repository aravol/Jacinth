using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Jacinth.Entities
{
    /// <summary>
    /// Represents a collection of SubEntities for a specific Processor
    /// </summary>
    internal abstract class SubEntitySet
    {
        internal abstract void UpdateEntities();
        internal abstract void QueueEntityAdd(Entity entity);
        internal abstract void QueueEntityRemove(Entity entity);
    }

    /// <summary>
    /// Represents a collection of SubEntities for a specific Processor
    /// </summary>
    internal class SubEntitySet<T> : SubEntitySet
        where T : SubEntity
    {
        #region Values

        private readonly Dictionary<Entity, T> _activeEntities = new Dictionary<Entity, T>();

        private readonly object _entityUpdateLock = new object();
        private readonly HashSet<Entity> _entitiesToAdd = new HashSet<Entity>();
        private readonly HashSet<Entity> _entitiesToRemove = new HashSet<Entity>();
        #endregion

        #region Properties

        public IEnumerable<Entity> ActiveEntities => _activeEntities.Keys;

        public IEnumerable<T> ActiveSubEntities => _activeEntities.Values;
        #endregion

        #region Constructors

        static SubEntitySet()
        {
            // Force the static constructors to activate against our specified SubEntity type to ensure it can register the appropriate factory method
            RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
        }
        #endregion

        #region Methods

        internal sealed override void UpdateEntities()
        {
            lock (_entityUpdateLock)
            {
                // Add all active Entities awaiting addition
                foreach (var ent in _entitiesToAdd
                    .Except(ActiveEntities)     // If something is already active to this Processor, don't bother with it
                    .Except(_entitiesToRemove)) // If the Entity has already been removed before it was properly added, don't bother with it
                {
                    T result;
                    if (SubEntityFactory.TryGenerateSubEntity(ent, out result))
                    {
                        result.Outdated += OnSubEntityOutdated;
                        _activeEntities.Add(ent, result);
                    }
                }

                // Iterate and remove everything queued for removal - the SubEntity has already performed filtering for these
                foreach (var ent in _entitiesToRemove
                    .Intersect(ActiveEntities)) // Only remove items actually active to this Processor
                {
                    _activeEntities.Remove(ent);
                }

                // Clear the update sets when we're done with them
                _entitiesToAdd.Clear();
                _entitiesToRemove.Clear();
            }
        }

        private void OnSubEntityOutdated(SubEntity subEntity) => QueueEntityRemove(subEntity.Entity);

        internal sealed override void QueueEntityAdd(Entity entity)
        {
            lock (_entityUpdateLock)
                _entitiesToAdd.Add(entity);
        }

        internal sealed override void QueueEntityRemove(Entity entity)
        {
            lock (_entityUpdateLock)
                _entitiesToRemove.Add(entity);
        }
        #endregion
    }
}
