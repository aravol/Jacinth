using System;
using Jacinth.Components;
using Jacinth.Entities;

namespace Jacinth.Processors
{
    /// <summary>
    /// Base class for a Processor which handles SubEntities one at a time
    /// </summary>
    /// <typeparam name="T">The type of SubEntity handled</typeparam>
    public abstract class SingleProcessorBase<T>
        : Processor<T>
        where T : SubEntity
    {
        /// <summary>
        /// Creates a new SingleProcessor in the given World
        /// </summary>
        protected SingleProcessorBase(JacinthWorld world) : base(world) { }

        /// <summary>
        /// Processes the entities in this Processor
        /// </summary>
        /// <param name="deltaTime">The time since the last execution on this Loop</param>
        protected internal sealed override void Process(TimeSpan deltaTime)
        {
            foreach (var subEntity in ActiveSubEntities)
                Process(deltaTime, subEntity);
        }

        /// <summary>
        /// Provides processing logic for each SubEntity in this Processor
        /// </summary>
        /// <param name="deltaTime">The time since the last execution on this Loop</param>
        /// <param name="subEntity">The SubEntity being handled</param>
        protected abstract void Process(TimeSpan deltaTime, T subEntity);
    }

    /// <summary>
    /// Base class for a Processor which handles a single Component one Entity at a time
    /// </summary>
    /// <typeparam name="T1">The type of Component handled</typeparam>
    public abstract class SingleProcessor<T1>
        : SingleProcessorBase<SubEntity<T1>>
        where T1 : Component
    {
        /// <summary>
        /// Creates a new SingleProcessor in the given World
        /// </summary>
        protected SingleProcessor(JacinthWorld world) : base(world) { }
    }

    /// <summary>
    /// Base class for a Processor which handles two Components one Entity at a time
    /// </summary>
    /// <typeparam name="T1">The first type of Component handled</typeparam>
    /// <typeparam name="T2">The second type of Component handled</typeparam>
    public abstract class SingleProcessor<T1, T2>
        : SingleProcessorBase<SubEntity<T1, T2>>
        where T1 : Component
        where T2 : Component
    {
        /// <summary>
        /// Creates a new SingleProcessor in the given World
        /// </summary>
        protected SingleProcessor(JacinthWorld world) : base(world) { }
    }

    /// <summary>
    /// Base class for a Processor which handles three Components one Entity at a time
    /// </summary>
    /// <typeparam name="T1">The first type of Component handled</typeparam>
    /// <typeparam name="T2">The second type of Component handled</typeparam>
    /// <typeparam name="T3">The third type of Component handled</typeparam>
    public abstract class SingleProcessor<T1, T2, T3>
        : SingleProcessorBase<SubEntity<T1, T2, T3>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        /// <summary>
        /// Creates a new SingleProcessor in the given World
        /// </summary>
        protected SingleProcessor(JacinthWorld world) : base(world) { }
    }

    /// <summary>
    /// Base class for a Processor which handles four Components one Entity at a time
    /// </summary>
    /// <typeparam name="T1">The first type of Component handled</typeparam>
    /// <typeparam name="T2">The second type of Component handled</typeparam>
    /// <typeparam name="T3">The third type of Component handled</typeparam>
    /// <typeparam name="T4">The fourth type of Component handled</typeparam>
    public abstract class SingleProcessor<T1, T2, T3, T4>
        : SingleProcessorBase<SubEntity<T1, T2, T3, T4>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {
        /// <summary>
        /// Creates a new SingleProcessor in the given World
        /// </summary>
        protected SingleProcessor(JacinthWorld world) : base(world) { }
    }

    /// <summary>
    /// Base class for a Processor which handles five Components one Entity at a time
    /// </summary>
    /// <typeparam name="T1">The first type of Component handled</typeparam>
    /// <typeparam name="T2">The second type of Component handled</typeparam>
    /// <typeparam name="T3">The third type of Component handled</typeparam>
    /// <typeparam name="T4">The fourth type of Component handled</typeparam>
    /// <typeparam name="T5">The fifth type of Component handled</typeparam>
    public abstract class SingleProcessor<T1, T2, T3, T4, T5>
        : SingleProcessorBase<SubEntity<T1, T2, T3, T4, T5>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
    {
        /// <summary>
        /// Creates a new SingleProcessor in the given World
        /// </summary>
        protected SingleProcessor(JacinthWorld world) : base(world) { }
    }
}
