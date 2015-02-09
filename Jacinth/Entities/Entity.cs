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
        internal event EventHandler<ComponentAddedEventArgs> ComponentAdded;
        internal event EventHandler<ComponentRemovedEventArgs> ComponentRemoved;

        private readonly JacinthWorld _world;
        private readonly int _hashCode;
        private readonly Guid _id;

        private ConcurrentDictionary<ComponentTypeKey, Component> _components
            = new ConcurrentDictionary<ComponentTypeKey, Component>();

        /// <summary>
        /// Gets the World to which this Entity belongs
        /// </summary>
        public JacinthWorld World { get { return _world; } }

        /// <summary>
        /// <para>Gets all Components attached to this Entity.</para>
        /// </summary>
        public IEnumerable<Component> Components
        {
            get { return _components.Values; }
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

        /// <summary>
        /// Adds a new Component of the speicfied type to this Entity using the default Costructor
        /// </summary>
        /// <typeparam name="T">The type of Component to add to this Entity, used to determine the ComponentTypeKey</typeparam>
        /// <returns>The newly created Component</returns>
        public T AddComponent<T>()
            where T : Component, new()
        {
            var result = new T();
            AddComponent(result);
            return result;
        }

        /// <summary>
        /// Adds a new Component to this Entity
        /// </summary>
        /// <typeparam name="T">The type of Component to add to this Entity, used to determine the ComponentTypeKey</typeparam>
        /// <param name="component">The Component to be added</param>
        public void AddComponent<T>(T component)
            where T : Component
        {
            component.Entity = this;

            if (_components.TryAdd(ComponentTypeKey.GetKey<T>(), component))
            {
                RaiseComponentAdded(component);
            }
        }

        /// <summary>
        /// Gets the Comoponent keyed to the specified type on this Entity, or null if not found
        /// </summary>
        /// <typeparam name="T">The type of Component to add to this Entity, used to determine the ComponentTypeKey</typeparam>
        /// <returns>The Component if found, or null otherwise</returns>
        public T GetComponent<T>()
            where T : Component
        {
            T result;
            return TryGetComponent<T>(out result) ? null : result;
        }

        /// /// <summary>
        /// Gets the specified type of Component from this Entity, or creates and adds it if not found.
        /// </summary>
        /// <typeparam name="T">The type of Component to add to this Entity, used to determine the ComponentTypeKey</typeparam>
        /// <param name="created">True if the Component was created, False otherwise</param>
        /// <returns>The found or created Component</returns>
        public T GetOrCreateComponent<T>(out bool created)
            where T : Component, new()
        {
            var key = ComponentTypeKey.GetKey<T>();
            Component result;
            created = !_components.TryGetValue(key, out result);
            if (!created)
                return (T) result; // Hard cast to propogate an InvalidCastException if used incorrectly
            else
                return AddComponent<T>();
        }
               
        /// /// <summary>
        /// Gets the specified type of Component from this Entity, or creates and adds it if not found.
        /// </summary>
        /// <typeparam name="T">The type of Component to add to this Entity, used to determine the ComponentTypeKey</typeparam>
        /// <returns>The found or created Component</returns>
        public T GetOrCreateComponent<T>()
            where T : Component, new()
        {
            bool ignored;
            return GetOrCreateComponent<T>(out ignored);
        }

        /// <summary>
        /// Attempts to get the Component of the specified type on this Entity
        /// </summary>
        /// <typeparam name="T">The type of Component to add to this Entity, used to determine the ComponentTypeKey</typeparam>
        /// <param name="component">The Component, if found</param>
        /// <returns>True if the Component was found, False otherwise</returns>
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

        /// <summary>
        /// <para>Checks whether this Entity has a Component of the given Type.</para>
        /// <para>Due to the highly parallel nature of Jacinth, this method should NOT be used before gettig a Component - use TryGetComponent instead</para>
        /// </summary>
        /// <typeparam name="T">The type of Component to add to this Entity, used to determine the ComponentTypeKey</typeparam>
        /// <returns>True if the Component was found, False otherwise</returns>
        public bool HasComponent<T>()
            where T : Component
        {
            var key = ComponentTypeKey.GetKey<T>();
            return _components.ContainsKey(key);
        }

        /// <summary>
        /// Removes the Component of the specified type from this Entity
        /// </summary>
        /// <typeparam name="T">The type of Component to add to this Entity, used to determine the ComponentTypeKey</typeparam>
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

        /// <summary>
        /// Gets the Hash Code of this Entity, which is determined when the Entity is instantiated
        /// </summary>
        /// <returns>The Hash Code of this Entity</returns>
        public override int GetHashCode()
        {
            return _hashCode;
        }

        /// <summary>
        /// Checks whether this Entity is the same as another Entity
        /// </summary>
        /// <param name="other">The other Entity to compare against</param>
        /// <returns>True if these Entities are logcally the same, False otherwise</returns>
        public bool Equals(Entity other)
        {
            return this._id == other._id;
        }
    }
}
