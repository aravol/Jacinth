using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        /// <summary>
        /// Creates a new Processor within the specified World
        /// </summary>
        /// <param name="world">The World in which this Processor exists</param>
        protected Processor(JacinthWorld world) { _world = world; }

        /// <summary>
        /// Processes the entities in this Processor
        /// </summary>
        /// <param name="deltaTime">The time since the last execution on this Loop</param>
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
        where T : SubEntity
    {
        private readonly Dictionary<Entity, T> _activeEntities = new Dictionary<Entity, T>();

        private readonly object _entityUpdateLock = new object();
        private readonly HashSet<Entity> _entitiesToAdd = new HashSet<Entity>();
        private readonly HashSet<Entity> _entitiesToRemove = new HashSet<Entity>();

        /// <summary>
        /// Enumerable of all the Entities currently being processed by this Processor
        /// </summary>
        public IEnumerable<Entity> ActiveEntities
        {
            get { return _activeEntities.Keys; }
        }

        /// <summary>
        /// Enumerable of all the Subentities currently being processed by this Processor
        /// </summary>
        protected IEnumerable<T> ActiveSubEntities
        {
            get { return _activeEntities.Values; }
        }

        static Processor()
        {
            // Force the static constructors to activate against our specified SubEntity type to ensure it can register the appropriate factory method
            RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
        }

        /// <summary>
        /// Creates a new Processor within the specified World
        /// </summary>
        /// <param name="world">The World in which this Processor exists</param>
        protected Processor(JacinthWorld world) : base(world) { }

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
                    if (SubEntityFactory.TryGenerateSubEntity<T>(ent, out result))
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

        private void OnSubEntityOutdated(SubEntity subEntity)
        {
            QueueEntityRemove(subEntity.Entity);
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
