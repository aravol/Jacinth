using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jacinth.Components;
using Jacinth.Processors;

namespace Jacinth.Entities
{
    /// <summary>
    /// Represents a specific subset of Components on a specific Entity, used to arrange data for a specific Processor
    /// </summary>
    public abstract class EntityComponentSubset
    {
        private readonly Entity _entity;
        private readonly Processor _processor;

        public Entity Entity { get { return _entity; } }
        public Processor Processor { get { return _processor; } }

        internal EntityComponentSubset(Processor processor, Entity entity)
        {
            _processor = processor;
            _entity = entity;

            entity.ComponentRemoved += entity_ComponentRemoved;
        }

        protected void entity_ComponentRemoved(object sender, ComponentRemovedEventArgs e)
        {
            if (IsComponentRelevant(e.ComponentKey))
            {
                // Queue this Subset for removal and stop listening
                Processor.QueueEntityRemove(this.Entity);
                _entity.ComponentRemoved -= entity_ComponentRemoved;
            }
        }

        protected abstract bool IsComponentRelevant(ComponentTypeKey key);
    }

    public class EntityComponentSubset<T1> : EntityComponentSubset
        where T1 : Component
    {
        private readonly T1 _component1;

        public T1 Component1 { get { return _component1; } }

        public EntityComponentSubset(Processor processor, Entity entity)
            : base(processor, entity)
        {
            _component1 = entity.GetComponent<T1>();
        }

        protected override bool IsComponentRelevant(ComponentTypeKey key)
        {
            return ComponentTypeKey.GetKey<T1>() == key;
        }
    }

    public class EntityComponentSubset<T1, T2> : EntityComponentSubset
        where T1 : Component
        where T2 : Component
    {
        private readonly T1 _component1;
        private readonly T2 _component2;

        public T1 Component1 { get { return _component1; } }
        public T2 Component2 { get { return _component2; } }

        public EntityComponentSubset(Processor processor, Entity entity)
            : base(processor, entity)
        {
            _component1 = entity.GetComponent<T1>();
            _component2 = entity.GetComponent<T2>();
        }

        protected override bool IsComponentRelevant(ComponentTypeKey key)
        {
            return ComponentTypeKey.GetKey<T1>() == key
                || ComponentTypeKey.GetKey<T2>() == key;
        }
    }

    public class EntityComponentSubset<T1, T2, T3> : EntityComponentSubset
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        private readonly T1 _component1;
        private readonly T2 _component2;
        private readonly T3 _component3;

        public T1 Component1 { get { return _component1; } }
        public T2 Component2 { get { return _component2; } }
        public T3 Component3 { get { return _component3; } }

        public EntityComponentSubset(Processor processor, Entity entity)
            : base(processor, entity)
        {
            _component1 = entity.GetComponent<T1>();
            _component2 = entity.GetComponent<T2>();
            _component3 = entity.GetComponent<T3>();
        }

        protected override bool IsComponentRelevant(ComponentTypeKey key)
        {
            return ComponentTypeKey.GetKey<T1>() == key
                || ComponentTypeKey.GetKey<T2>() == key
                || ComponentTypeKey.GetKey<T3>() == key;
        }
    }

    public class EntityComponentSubset<T1, T2, T3, T4> : EntityComponentSubset
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {
        private readonly T1 _component1;
        private readonly T2 _component2;
        private readonly T3 _component3;
        private readonly T4 _component4;

        public T1 Component1 { get { return _component1; } }
        public T2 Component2 { get { return _component2; } } 
        public T3 Component3 { get { return _component3; } }
        public T4 Component4 { get { return _component4; } }

        public EntityComponentSubset(Processor processor, Entity entity)
            : base(processor, entity)
        {
            _component1 = entity.GetComponent<T1>();
            _component2 = entity.GetComponent<T2>();
            _component3 = entity.GetComponent<T3>();
            _component4 = entity.GetComponent<T4>();
        }

        protected override bool IsComponentRelevant(ComponentTypeKey key)
        {
            return ComponentTypeKey.GetKey<T1>() == key
                || ComponentTypeKey.GetKey<T2>() == key
                || ComponentTypeKey.GetKey<T3>() == key
                || ComponentTypeKey.GetKey<T4>() == key;
        }
    }

    public class EntityComponentSubset<T1, T2, T3, T4, T5> : EntityComponentSubset
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
    {
        private readonly T1 _component1;
        private readonly T2 _component2;
        private readonly T3 _component3;
        private readonly T4 _component4;
        private readonly T5 _component5;

        public T1 Component1 { get { return _component1; } }
        public T2 Component2 { get { return _component2; } }
        public T3 Component3 { get { return _component3; } }
        public T4 Component4 { get { return _component4; } }
        public T5 Component5 { get { return _component5; } }

        public EntityComponentSubset(Processor processor, Entity entity)
            : base(processor, entity)
        {
            _component1 = entity.GetComponent<T1>();
            _component2 = entity.GetComponent<T2>();
            _component3 = entity.GetComponent<T3>();
            _component4 = entity.GetComponent<T4>();
            _component5 = entity.GetComponent<T5>();
        }

        protected override bool IsComponentRelevant(ComponentTypeKey key)
        {
            return ComponentTypeKey.GetKey<T1>() == key
                || ComponentTypeKey.GetKey<T2>() == key
                || ComponentTypeKey.GetKey<T3>() == key
                || ComponentTypeKey.GetKey<T4>() == key
                || ComponentTypeKey.GetKey<T5>() == key;
        }
    }
}
