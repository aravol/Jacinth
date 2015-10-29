using System;
using Jacinth.Components;

namespace Jacinth.Entities
{
    /// <summary>
    /// Represents a specific subset of Components on a specific Entity, used to arrange data for a specific Processor
    /// </summary>
    public abstract class SubEntity
    {
        #region events

        internal event Action<SubEntity> Outdated;
        #endregion

        #region Properties

        /// <summary>
        /// The Entity for whyich this SubEntity represents a specific subset of Components
        /// </summary>
        public Entity Entity { get; }
        #endregion

        #region Constructor

        internal SubEntity(Entity entity)
        {
            Entity = entity;

            Entity.ComponentAdded += OnEntityComponentAdded;
            Entity.ComponentRemoved += OnEntityComponentRemoved;
            Entity.EntityDestroyed += OnEntityDestroyed;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Raises an event to indicate that this SubEntity is no longer valid and should be removed from all Processors utilizing it
        /// </summary>
        protected void RaiseOutdated()
        {
            Outdated?.Invoke(this);

            // Detach event listeners
            Outdated = null;
        }

        /// <summary>
        /// Handler called when a new Component is added to the Entity.
        /// </summary>
        protected virtual void OnEntityComponentAdded(Entity entity, ComponentTypeKey key, Component component) { }

        /// <summary>
        /// Handler called when a Component is removed from the Entity.
        /// </summary>
        protected virtual void OnEntityComponentRemoved(Entity entity, ComponentTypeKey key, Component component) { }

        /// <summary>
        /// Handler called when the underlying Entity is Destroyed
        /// </summary>
        protected virtual void OnEntityDestroyed(Entity entity) => RaiseOutdated();
        #endregion
    }

    #region Standard Sub entities

    /// <summary>
    /// Standard SubEntity which monitors a single Component
    /// </summary>
    /// <typeparam name="T1">The type of Component this SubEntity monitors</typeparam>
    public class SubEntity<T1> : SubEntity
        where T1 : Component
    {
        /// <summary>
        /// Gets the Component monitored by this SubEntity
        /// </summary>
        public T1 Component1 { get; }

        // Registers the appropriate Factory for this SubEntity type
        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory<SubEntity<T1>>(GenerateSubEntity);
        }

        /// <summary>
        /// Creates a new SubEntity against the given Entity with the given Component
        /// </summary>
        protected SubEntity(Entity entity, T1 component1)
            : base(entity)
        {
            Component1 = component1;
        }

        // Generation method for this type of SubEntity
        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1> subEntity)
        {
            T1 component1;

            if (entity.TryGetComponent(out component1))
            {
                subEntity = new SubEntity<T1>(entity, component1);
                return true;
            }

            subEntity = null;
            return false;
        }

        /// <summary>
        /// Handler called when a Component is removed from the Entity.
        /// </summary>
        protected override void OnEntityComponentRemoved(Entity entity, ComponentTypeKey key, Component component)
        {
            if (key == ComponentTypeKey.GetKey<T1>())
            { RaiseOutdated(); }
        }
    }
    
    /// <summary>
    /// Standard SubEntity which monitors a pair of Components
    /// </summary>
    /// <typeparam name="T1">The first type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T2">The second type of Component this SubEntity monitors</typeparam>
    public class SubEntity<T1, T2> : SubEntity
        where T1 : Component
        where T2 : Component
    {
        /// <summary>
        /// Gets the first Component monitored by this SubEntity
        /// </summary>
        public T1 Component1 { get; }
        
        /// <summary>
        /// Gets the second Component monitored by this SubEntity
        /// </summary>
        public T2 Component2 { get; }

        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory <SubEntity<T1, T2>>(GenerateSubEntity);
        }

        /// <summary>
        /// Creates a new SubEntity against the given Entity with the given Components
        /// </summary>
        protected SubEntity(Entity entity, T1 component1, T2 component2)
            : base(entity)
        {
            Component1 = component1;
            Component2 = component2;
        }

        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1, T2> subEntity)
        {
            T1 component1;
            T2 component2;
            if(entity.TryGetComponent(out component1)
                && entity.TryGetComponent(out component2))
            {
                subEntity = new SubEntity<T1,T2>(entity, component1, component2);
                return true;
            }
            subEntity = null;
            return false;
        }

        /// <summary>
        /// Handler called when a Component is removed from the Entity.
        /// </summary>
        protected override void OnEntityComponentRemoved(Entity entity, ComponentTypeKey key, Component component)
        {
            if (key == ComponentTypeKey.GetKey<T1>()
                || key == ComponentTypeKey.GetKey<T2>())
            { RaiseOutdated(); }
        }
    }
    
    /// <summary>
    /// Standard SubEntity which monitors three Components
    /// </summary>
    /// <typeparam name="T1">The first type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T2">The second type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T3">The third type of Component this SubEntity monitors</typeparam>
    public class SubEntity<T1, T2, T3> : SubEntity
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {        
        /// <summary>
        /// Gets the first Component monitored by this SubEntity
        /// </summary>
        public T1 Component1 { get; }
        
        /// <summary>
        /// Gets the second Component monitored by this SubEntity
        /// </summary>
        public T2 Component2 { get; }
        
        /// <summary>
        /// Gets the fourth Component monitored by this SubEntity
        /// </summary>
        public T3 Component3 { get; }

        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory<SubEntity<T1, T2, T3>>(GenerateSubEntity);
        }

        /// <summary>
        /// Creates a new SubEntity against the given Entity with the given Components
        /// </summary>
        protected SubEntity(Entity entity, T1 component1, T2 component2, T3 component3)
            : base(entity)
        {
            Component1 = component1;
            Component2 = component2;
            Component3 = component3;
        }

        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1, T2, T3> subEntity)
        {
            T1 component1;
            T2 component2;
            T3 component3;
            if(entity.TryGetComponent(out component1)
                && entity.TryGetComponent(out component2)
                && entity.TryGetComponent(out component3))
            {
                subEntity = new SubEntity<T1, T2, T3>(entity, component1, component2, component3);
                return true;
            }
            subEntity = null;
            return false;
        }
        
        /// <summary>
        /// Handler called when a Component is removed from the Entity.
        /// </summary>
        protected override void OnEntityComponentRemoved(Entity entity, ComponentTypeKey key, Component component)
        {
            if (key == ComponentTypeKey.GetKey<T1>()
                || key == ComponentTypeKey.GetKey<T2>()
                || key == ComponentTypeKey.GetKey<T3>())
            { RaiseOutdated(); }
        }
    }

    /// <summary>
    /// Standard SubEntity which monitors four Components
    /// </summary>
    /// <typeparam name="T1">The first type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T2">The second type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T3">The third type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T4">The fourth type of Component this SubEntity monitors</typeparam>
    public class SubEntity<T1, T2, T3, T4> : SubEntity
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {        
        /// <summary>
        /// Gets the first Component monitored by this SubEntity
        /// </summary>
        public T1 Component1 { get; }

        /// <summary>
        /// Gets the second Component monitored by this SubEntity
        /// </summary>
        public T2 Component2 { get; }

        /// <summary>
        /// Gets the third Component monitored by this SubEntity
        /// </summary>
        public T3 Component3 { get; }

        /// <summary>
        /// Gets the fourth Component monitored by this SubEntity
        /// </summary>
        public T4 Component4 { get; }

        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory<SubEntity<T1, T2, T3, T4>>(GenerateSubEntity);
        }

        /// <summary>
        /// Creates a new SubEntity against the given Entity with the given Components
        /// </summary>
        protected SubEntity(Entity entity, T1 component1, T2 component2, T3 component3, T4 component4)
            : base(entity)
        {
            Component1 = component1;
            Component2 = component2;
            Component3 = component3;
            Component4 = component4;
        }

        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1, T2, T3, T4> subEntity)
        {
            T1 component1;
            T2 component2;
            T3 component3;
            T4 component4;
            if (entity.TryGetComponent(out component1)
                && entity.TryGetComponent(out component2)
                && entity.TryGetComponent(out component3)
                && entity.TryGetComponent(out component4))
            {
                subEntity = new SubEntity<T1, T2, T3, T4>(entity, component1, component2, component3, component4);
                return true;
            }
            subEntity = null;
            return false;
        }
        
        /// <summary>
        /// Handler called when a Component is removed from the Entity.
        /// </summary>
        protected override void OnEntityComponentRemoved(Entity entity, ComponentTypeKey key, Component component)
        {
            if (key == ComponentTypeKey.GetKey<T1>()
                || key == ComponentTypeKey.GetKey<T2>()
                || key == ComponentTypeKey.GetKey<T3>()
                || key == ComponentTypeKey.GetKey<T4>())
            { RaiseOutdated(); }
        }
    }
    
    /// <summary>
    /// Standard SubEntity which monitors five Components
    /// </summary>
    /// <typeparam name="T1">The first type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T2">The second type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T3">The third type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T4">The fourth type of Component this SubEntity monitors</typeparam>
    /// <typeparam name="T5">The fifth type of Component this SubEntity monitors</typeparam>
    public class SubEntity<T1, T2, T3, T4, T5> : SubEntity
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
    {
        /// <summary>
        /// Gets the first Component monitored by this SubEntity
        /// </summary>
        public T1 Component1 { get; }

        /// <summary>
        /// Gets the second Component monitored by this SubEntity
        /// </summary>
        public T2 Component2 { get; }

        /// <summary>
        /// Gets the third Component monitored by this SubEntity
        /// </summary>
        public T3 Component3 { get; }

        /// <summary>
        /// Gets the fourth Component monitored by this SubEntity
        /// </summary>
        public T4 Component4 { get; }
        
        /// <summary>
        /// Gets the fifth Component monitored by this SubEntity
        /// </summary>
        public T5 Component5 { get; }

        static SubEntity()
        {
            SubEntityFactory.RegisterSubEntityFactory<SubEntity<T1, T2, T3, T4, T5>>(GenerateSubEntity);
        }

        /// <summary>
        /// Creates a new SubEntity against the given Entity with the given Components
        /// </summary>
        protected SubEntity(Entity entity, T1 component1, T2 component2, T3 component3, T4 component4, T5 component5)
            : base(entity)
        {
            Component1 = component1;
            Component2 = component2;
            Component3 = component3;
            Component4 = component4;
            Component5 = component5;
        }

        private static bool GenerateSubEntity(Entity entity, out SubEntity<T1, T2, T3, T4, T5> subEntity)
        {
            T1 component1;
            T2 component2;
            T3 component3;
            T4 component4;
            T5 component5;

            if (entity.TryGetComponent(out component1)
                && entity.TryGetComponent(out component2)
                && entity.TryGetComponent(out component3)
                && entity.TryGetComponent(out component4)
                && entity.TryGetComponent(out component5))
            {
                subEntity = new SubEntity<T1, T2, T3, T4, T5>(entity, component1, component2, component3, component4, component5);
                return true;
            }
            subEntity = null;
            return false;
        }
        
        /// <summary>
        /// Handler called when a Component is removed from the Entity.
        /// </summary>
        protected override void OnEntityComponentRemoved(Entity entity, ComponentTypeKey key, Component component)
        {
            if (key == ComponentTypeKey.GetKey<T1>()
                || key == ComponentTypeKey.GetKey<T2>()
                || key == ComponentTypeKey.GetKey<T3>()
                || key == ComponentTypeKey.GetKey<T4>()
                || key == ComponentTypeKey.GetKey<T5>())
            { RaiseOutdated(); }
        }
    }
    #endregion
}
