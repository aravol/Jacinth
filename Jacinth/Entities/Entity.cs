using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jacinth.Components;

namespace Jacinth.Entities
{
    /// <summary>
    /// Basic working object of Jacinth, contains Components to define data and attaches to Processors to define behavior
    /// </summary>
    public sealed class Entity : IEquatable<Entity>
    {
        private readonly JacinthWorld _world;
        private readonly int _hashCode;
        private readonly Guid _id;

        private ConcurrentDictionary<ComponentTypeKey, Component> _components
            = new ConcurrentDictionary<ComponentTypeKey, Component>();

        internal event EventHandler<ComponentAddedEventArgs> ComponentAdded;
        internal event EventHandler<ComponentRemovedEventArgs> ComponentRemoved;

        /// <summary>
        /// The World to which this Entity belongs
        /// </summary>
        public JacinthWorld World { get { return _world; } }

        /// <summary>
        /// <para>Gets all Components attached to this Entity.</para>
        /// <para>WANRING: This is an expensive operation, and unlikely to be optimized in the future. Avoid where possible.</para>
        /// </summary>
        public IEnumerable<Component> Components
        {
            get {                 return _components.Values;            }
        }

        /// <summary>
        /// Do not allow objects outside of Jacinth to create Entities
        /// </summary>
        internal Entity(JacinthWorld world)
        {
            _world = world;
            _id = Guid.NewGuid();
            _hashCode = _id.GetHashCode();

            // Attach no-op event handlers to prevent event raise errors
            ComponentAdded += (sender, args) => { };
            ComponentRemoved += (sender, args) => { };
        }

        // TODO: Has, Get, Create, and Remove accessors for Components using Keys

        public T AddComponent<T>()
            where T : Component, new()
        {
            var result = new T();
            AddComponent(result);
            return result;
        }

        public void AddComponent<T>(T component)
            where T : Component
        {
            component.Entity = this;

            if (_components.TryAdd(ComponentTypeKey.GetKey<T>(), component))
            {
                RaiseComponentAdded(component);
            }
        }

        public T GetComponent<T>()
            where T : Component
        {
            T result;
            return TryGetComponent<T>(out result) ? null : result;
        }

        public T GetOrCreateComponent<T>()
            where T : Component, new()
        {
            var key = ComponentTypeKey.GetKey<T>();
            Component result;
            if (_components.TryGetValue(key, out result))
                return (T) result; // Hard cast to propogate an InvalidCastException if used incorrectly
            else
                return AddComponent<T>();
        }

        public bool TryGetComponent<T>(out T component)
            where T : Component
        {
            var key = ComponentTypeKey.GetKey<T>();
            Component rawComponent;
            bool result;
            
            result = _components.TryGetValue(key, out rawComponent);

            component = rawComponent as T;
            return result
                && component != null;
        }

        public bool HasComponent<T>()
            where T : Component
        {
            var key = ComponentTypeKey.GetKey<T>();
            return _components.ContainsKey(key);
        }

        public void RemoveComponent<T>()
            where T : Component
        {
            var key = ComponentTypeKey.GetKey<T>();
            Component component;

            if (_components.TryRemove(key, out component))
                RaiseComponentRemoved(key, component);
        }

        private void RaiseComponentRemoved(ComponentTypeKey key, Component component)
        {
            Task.Run(() => ComponentRemoved.Invoke(this, new ComponentRemovedEventArgs(this, key, component)));
        }

        private void RaiseComponentRemoved()
        {
            Task.Run(() => ComponentRemoved.Invoke(this, new ComponentRemovedEventArgs(this)));
        }

        private void RaiseComponentAdded(Component component)
        {
            Task.Run(() => ComponentAdded.Invoke(this, new ComponentAddedEventArgs(this, component)));
        }

        /// <summary>
        /// Removes all Components from this Entity an removes it from the World
        /// </summary>
        public void Destroy()
        {


            RaiseComponentRemoved();
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public bool Equals(Entity other)
        {
            return this._id == other._id;
        }
    }
}
