using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jacinth.Entities;

namespace Jacinth.Processors
{
    /// <summary>
    /// Processes Entities, providing specific behaviours based on data in the constituent Components
    /// </summary>
    // Base calss for inclusion into collections of disparate types
    public abstract class Processor
    {
        private readonly JacinthWorld _world;

        /// <summary>
        /// The World this Processor is resident in
        /// </summary>
        public JacinthWorld World { get { return _world; } }

        /// <summary>
        /// The Loop controlling this Processor's execution timing and threading
        /// </summary>
        public ProcessorLoop Loop { get; internal set; }

        public abstract IEnumerable<Entity> ActiveEntities { get; }

        protected Processor(JacinthWorld world) { _world = world; }

        protected internal abstract void Process(TimeSpan deltaTime);

        /// <summary>
        /// Updates the Entities in this Processor, adding and removing any queued Entities appropriately
        /// </summary>
        internal abstract void UpdateEntities();

        /// <summary>
        /// Enqueues an Entity for possible addition to this Processor.
        ///  Validation filltering and Subset grabbing are performed during UpdateEntities
        /// </summary>
        /// <param name="entity">The Entity to add</param>
        internal abstract void QueueEntityAdd(Entity entity);

        /// <summary>
        /// Enqueues an Entity for removal from this Processor.
        ///  Does NOT provide error checking.
        /// </summary>
        /// <param name="entity">The Entity to remove</param>
        internal abstract void QueueEntityRemove(Entity entity);
    }

    /// <summary>
    /// Processes Entities, providing specific behaviours based on data in the constituent Components
    /// </summary>
    public abstract class Processor<T> : Processor
        where T : EntityComponentSubset
    {
        private readonly Dictionary<Entity, T> _activeEntities = new Dictionary<Entity, T>();

        private readonly object _entityUpdateLock = new object();
        private readonly HashSet<Entity> _entitiesToAdd = new HashSet<Entity>();
        private readonly HashSet<Entity> _entitiesToRemove = new HashSet<Entity>();

        /// <summary>
        /// Enumerable of all the Entities currently being processed by this Processor
        /// </summary>
        public sealed override IEnumerable<Entity> ActiveEntities
        {
            get { return _activeEntities.Keys; }
        }

        protected Processor(JacinthWorld world) : base(world) { }

        /// <summary>
        /// <para>When overriden in a derived class, verifies if an Entity is valid for this Processor. Invoked when an Entity changes Components</para>
        /// <para>May be called from outside the context of this Processor's Loop.</para>
        /// </summary>
        /// <param name="entity">The entity being tested for validity</param>
        /// <param name="entityComponentSubset">If the Entity is determined valid, this should output the specific type of EntityComponentSubset for this Processo.r</param>
        /// <returns>True if the Entity is valid for this Processor, False otherwise</returns>
        protected abstract bool TryGetSubEntity(Entity entity, out T entityComponentSubset);

        internal sealed override void UpdateEntities()
        {
            lock(_entityUpdateLock)
            {
                // Add all active Entities awaiting addition
                foreach(var ent in _entitiesToAdd
                    .Except(ActiveEntities)     // If something is already active to this Processor, don't bother with it
                    .Except(_entitiesToRemove)) // If the Entity has already been removed before it was properly added, don't bother with it
                {
                    T result;
                    if (TryGetSubEntity(ent, out result))
                        _activeEntities.Add(ent, result);
                }

                // Iterate and remove everything queued for removal - the EntityComponentSubset has already performed filtering for these
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
    }
}
