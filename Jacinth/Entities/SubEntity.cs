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
    public abstract class SubEntity
    {
        private readonly Entity _entity;

        internal event EventHandler<SubEntityOutdatedEventArgs> Outdated;

        /// <summary>
        /// The Entity for whyich this SubEntity represents a specific subset of Components
        /// </summary>
        public Entity Entity { get { return _entity; } }

        internal SubEntity(Entity entity)
        {
            _entity = entity;

            _entity.ComponentAdded += OnEntityComponentAdded;
            _entity.ComponentRemoved += OnEntityComponentRemoved;
        }

        protected void RaiseOutdated(SubEntityOutdatedEventArgs args)
        {
            Entity.ComponentAdded -= OnEntityComponentAdded;
            Entity.ComponentRemoved -= OnEntityComponentRemoved;

            if (Outdated != null) Outdated(this, args);
        }

        /// <summary>
        /// Handler called when a new Component is added to the Entity. Defaults as a no-op.
        /// </summary>
        protected virtual void OnEntityComponentAdded(object sender, EventArgs e) { }

        /// <summary>
        /// Handler called when a Component is removed from the Entity. Defaults to a no-op.
        /// </summary>
        protected virtual void OnEntityComponentRemoved(object sender, ComponentRemovedEventArgs e) { }
    }

    #region Standard Sub entities

    public class SubEntity<T1> : SubEntity
        where T1 : Component
    {
        private readonly T1 _component1;

        public T1 Component1 { get { return _component1; } }

        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory<SubEntity<T1>>(GenerateSubEntity);
        }

        public SubEntity(Entity entity, T1 component1)
            : base(entity)
        {
            _component1 = component1;
        }

        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1> subEntity)
        {
            T1 component1;

            if (entity.TryGetComponent<T1>(out component1))
            {
                subEntity = new SubEntity<T1>(entity, component1);
                return true;
            }

            else
            {
                subEntity = null;
                return false;
            }
        }
    }

    public class SubEntity<T1, T2> : SubEntity
        where T1 : Component
        where T2 : Component
    {
        private readonly T1 _component1;
        private readonly T2 _component2;

        public T1 Component1 { get { return _component1; } }
        public T2 Component2 { get { return _component2; } }

        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory <SubEntity<T1, T2>>(GenerateSubEntity);
        }

        protected SubEntity(Entity entity, T1 component1, T2 component2)
            : base(entity)
        {
            _component1 = component1;
            _component2 = component2;
        }

        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1, T2> subEntity)
        {
            T1 component1;
            T2 component2;
            if(entity.TryGetComponent<T1>(out component1)
                && entity.TryGetComponent<T2>(out component2))
            {
                subEntity = new SubEntity<T1,T2>(entity, component1, component2);
                return true;
            }
            else
            {
                subEntity = null;
                return false;
            }
        }
    }

    public class SubEntity<T1, T2, T3> : SubEntity
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

        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory<SubEntity<T1, T2, T3>>(GenerateSubEntity);
        }

        protected SubEntity(Entity entity, T1 component1, T2 component2, T3 component3)
            : base(entity)
        {
            _component1 = component1;
            _component2 = component2;
            _component3 = component3;
        }

        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1, T2, T3> subEntity)
        {
            T1 component1;
            T2 component2;
            T3 component3;
            if(entity.TryGetComponent<T1>(out component1)
                && entity.TryGetComponent<T2>(out component2)
                && entity.TryGetComponent<T3>(out component3))
            {
                subEntity = new SubEntity<T1, T2, T3>(entity, component1, component2, component3);
                return true;
            }
            else
            {
                subEntity = null;
                return false;
            }
        }
    }

    public class SubEntity<T1, T2, T3, T4> : SubEntity
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

        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory<SubEntity<T1, T2, T3, T4>>(GenerateSubEntity);
        }

        protected SubEntity(Entity entity, T1 component1, T2 component2, T3 component3, T4 component4)
            : base(entity)
        {
            _component1 = component1;
            _component2 = component2;
            _component3 = component3;
            _component4 = component4;
        }

        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1, T2, T3, T4> subEntity)
        {
            T1 component1;
            T2 component2;
            T3 component3;
            T4 component4;
            if (entity.TryGetComponent<T1>(out component1)
                && entity.TryGetComponent<T2>(out component2)
                && entity.TryGetComponent<T3>(out component3)
                && entity.TryGetComponent<T4>(out component4))
            {
                subEntity = new SubEntity<T1, T2, T3, T4>(entity, component1, component2, component3, component4);
                return true;
            }
            else
            {
                subEntity = null;
                return false;
            }
        }
    }

    public class SubEntity<T1, T2, T3, T4, T5> : SubEntity
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

        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory<SubEntity<T1, T2, T3, T4, T5>>(GenerateSubEntity);
        }

        protected SubEntity(Entity entity, T1 component1, T2 component2, T3 component3, T4 component4, T5 component5)
            : base(entity)
        {
            _component1 = component1;
            _component2 = component2;
            _component3 = component3;
            _component4 = component4;
            _component5 = component5;
        }

        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1, T2, T3, T4, T5> subEntity)
        {
            T1 component1;
            T2 component2;
            T3 component3;
            T4 component4;
            T5 component5;

            if (entity.TryGetComponent<T1>(out component1)
                && entity.TryGetComponent<T2>(out component2)
                && entity.TryGetComponent<T3>(out component3)
                && entity.TryGetComponent<T4>(out component4)
                && entity.TryGetComponent<T5>(out component5))
            {
                subEntity = new SubEntity<T1, T2, T3, T4, T5>(entity, component1, component2, component3, component4, component5);
                return true;
            }
            else
            {
                subEntity = null;
                return false;
            }
        }
    }
    #endregion
}
