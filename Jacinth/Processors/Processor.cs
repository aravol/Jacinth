using System;
using System.Collections.Generic;
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
    }

    /// <summary>
    /// <para>Processes Entities, providing specific behaviours based on data in the constituent Components.</para>
    /// <para>Garuantees static constructors will be called for type T becfore usage in the class</para>
    /// </summary>
    public abstract class Processor<T> : Processor
        where T : SubEntity
    {
        private readonly SubEntitySet<T> _subEntitySet = new SubEntitySet<T>();

        /// <summary>
        /// Gets all the SubEntities currently being processed by this Processor
        /// </summary>
        public IEnumerable<T> ActiveSubEntities
        {
            get { return _subEntitySet.ActiveSubEntities; }
        }

        /// <summary>
        /// Creates a new Processor within the specified World
        /// </summary>
        /// <param name="world">The World in which this Processor exists</param>
        protected Processor(JacinthWorld world)
            : base(world)
        {
            world.EntityUpdated += _subEntitySet.QueueEntityAdd;
        }

        /// <summary>
        /// Updates the Entities in this Processor, adding and removing any queued Entities appropriately
        /// </summary>
        internal sealed override void UpdateEntities()
        {
            _subEntitySet.UpdateEntities();
        }
    }
}
