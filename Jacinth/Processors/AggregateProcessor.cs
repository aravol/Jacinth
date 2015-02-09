using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jacinth.Components;
using Jacinth.Entities;

namespace Jacinth.Processors
{
    /// <summary>
    /// Base class for a Processor which handles a single Component
    /// </summary>
    /// <typeparam name="T1">The type of Component being handled</typeparam>
    public abstract class AggregateProcessor<T1> : Processor<SubEntity<T1>>
        where T1 : Component
    {
        /// <summary>
        /// Instantiates a new Aggregate Processor in the given World
        /// </summary>
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }
    
    /// <summary>
    /// Base class for a Processor which handles two Components
    /// </summary>
    /// <typeparam name="T1">The first type of Component being handled</typeparam>
    /// <typeparam name="T2">The second type of Component being handled</typeparam>
    public abstract class AggregateProcessor<T1, T2> : Processor<SubEntity<T1, T2>>
        where T1 : Component
        where T2 : Component
    {
        /// <summary>
        /// Instantiates a new Aggregate Processor in the given World
        /// </summary>
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }
    
    /// <summary>
    /// Base class for a Processor which handles three Components
    /// </summary>
    /// <typeparam name="T1">The first type of Component being handled</typeparam>
    /// <typeparam name="T2">The second type of Component being handled</typeparam>
    /// <typeparam name="T3">The third type of Component being handled</typeparam>
    public abstract class AggregateProcessor<T1, T2, T3> : Processor<SubEntity<T1, T2, T3>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        /// <summary>
        /// Instantiates a new Aggregate Processor in the given World
        /// </summary>
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }

    /// <summary>
    /// Base class for a Processor which handles three Components
    /// </summary>
    /// <typeparam name="T1">The first type of Component being handled</typeparam>
    /// <typeparam name="T2">The second type of Component being handled</typeparam>
    /// <typeparam name="T3">The third type of Component being handled</typeparam>
    /// <typeparam name="T4">The fourth type of Component being handled</typeparam>
    public abstract class AggregateProcessor<T1, T2, T3, T4> : Processor<SubEntity<T1, T2, T3, T4>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {
        /// <summary>
        /// Instantiates a new Aggregate Processor in the given World
        /// </summary>
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }

    /// <summary>
    /// Base class for a Processor which handles three Components
    /// </summary>
    /// <typeparam name="T1">The first type of Component being handled</typeparam>
    /// <typeparam name="T2">The second type of Component being handled</typeparam>
    /// <typeparam name="T3">The third type of Component being handled</typeparam>
    /// <typeparam name="T4">The fourth type of Component being handled</typeparam>
    /// <typeparam name="T5">The fifth type of Component being handled</typeparam>
    public abstract class AggregateProcessor<T1, T2, T3, T4, T5> : Processor<SubEntity<T1, T2, T3, T4, T5>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
    {
        /// <summary>
        /// Instantiates a new Aggregate Processor in the given World
        /// </summary>
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }
}
