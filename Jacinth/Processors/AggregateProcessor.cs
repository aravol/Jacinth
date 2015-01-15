using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jacinth.Components;
using Jacinth.Entities;

namespace Jacinth.Processors
{
    public abstract class AggregateProcessor<T1> : Processor<SubEntity<T1>>
        where T1 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }

    public abstract class AggregateProcessor<T1, T2> : Processor<SubEntity<T1, T2>>
        where T1 : Component
        where T2 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }

    public abstract class AggregateProcessor<T1, T2, T3> : Processor<SubEntity<T1, T2, T3>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }

    public abstract class AggregateProcessor<T1, T2, T3, T4> : Processor<SubEntity<T1, T2, T3, T4>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }

    public abstract class AggregateProcessor<T1, T2, T3, T4, T5> : Processor<SubEntity<T1, T2, T3, T4, T5>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
    }
}
