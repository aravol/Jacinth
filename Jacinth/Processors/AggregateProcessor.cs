using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jacinth.Components;
using Jacinth.Entities;

namespace Jacinth.Processors
{
    public abstract class AggregateProcessor<T1> : Processor<EntityComponentSubset<T1>>
        where T1 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }

        protected sealed override bool TryGetSubEntity(Entity entity, out EntityComponentSubset<T1> entityComponentSubset)
        {
            lock (World.TableLock)
            {
                if (entity.HasComponent<T1>())
                {
                    entityComponentSubset = new EntityComponentSubset<T1>(this, entity);
                    return true;
                }
            }

            entityComponentSubset = null;
            return false;
        }
    }

    public abstract class AggregateProcessor<T1, T2> : Processor<EntityComponentSubset<T1, T2>>
        where T1 : Component
        where T2 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
        
        protected sealed override bool TryGetSubEntity(Entity entity, out EntityComponentSubset<T1, T2> entityComponentSubset)
        {
            lock (World.TableLock)
            {
                if (entity.HasComponent<T1>()
                    && entity.HasComponent<T2>())
                {
                    entityComponentSubset = new EntityComponentSubset<T1, T2>(this, entity);
                    return true;
                }
            }

            entityComponentSubset = null;
            return false;
        }
    }

    public abstract class AggregateProcessor<T1, T2, T3> : Processor<EntityComponentSubset<T1, T2, T3>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
        
        protected sealed override bool TryGetSubEntity(Entity entity, out EntityComponentSubset<T1, T2, T3> entityComponentSubset)
        {
            lock (World.TableLock)
            {
                if (entity.HasComponent<T1>()
                    && entity.HasComponent<T2>()
                    && entity.HasComponent<T3>())
                {
                    entityComponentSubset = new EntityComponentSubset<T1, T2, T3>(this, entity);
                    return true;
                }
            }

            entityComponentSubset = null;
            return false;
        }
    }

    public abstract class AggregateProcessor<T1, T2, T3, T4> : Processor<EntityComponentSubset<T1, T2, T3, T4>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
        
        protected override sealed bool TryGetSubEntity(Entity entity,
            out EntityComponentSubset<T1, T2, T3, T4> entityComponentSubset)
        {
            lock (World.TableLock)
            {
                if (entity.HasComponent<T1>()
                    && entity.HasComponent<T2>()
                    && entity.HasComponent<T3>()
                    && entity.HasComponent<T4>())
                {
                    entityComponentSubset = new EntityComponentSubset<T1, T2, T3, T4>(this, entity);
                    return true;
                }
            }

            entityComponentSubset = null;
            return false;
        }
    }

    public abstract class AggregateProcessor<T1, T2, T3, T4, T5> : Processor<EntityComponentSubset<T1, T2, T3, T4, T5>>
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
    {
        protected AggregateProcessor(JacinthWorld world) : base(world) { }
        
        protected override sealed bool TryGetSubEntity(Entity entity,
            out EntityComponentSubset<T1, T2, T3, T4, T5> entityComponentSubset)
        {
            lock (World.TableLock)
            {
                if (entity.HasComponent<T1>()
                    && entity.HasComponent<T2>()
                    && entity.HasComponent<T3>()
                    && entity.HasComponent<T4>()
                    && entity.HasComponent<T5>())
                {
                    entityComponentSubset = new EntityComponentSubset<T1, T2, T3, T4, T5>(this, entity);
                    return true;
                }
            }

            entityComponentSubset = null;
            return false;
        }
    }
}
