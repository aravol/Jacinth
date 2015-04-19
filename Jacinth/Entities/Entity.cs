using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jacinth.Components;

namespace Jacinth.Entities
{
    /// <summary>
    /// Basic working object of Jacinth, contains Components to define data and attaches to Processors to define behavior
    /// </summary>
    public sealed class Entity : IEquatable<Entity>
    {
        internal event Action<Entity, ComponentTypeKey, Component> ComponentAdded;
        internal event Action<Entity, ComponentTypeKey, Component> ComponentRemoved;
        internal event Action<Entity> EntityDestroyed;

        // ReSharper disable once InconsistentNaming
        // Named as per a private field due to an accessor
        private event Action<Entity> _enabled;

        /// <summary>
        /// Event fired when this Entity is enabled.
        ///  Immediately calls any new handlers added to it if this Entity is already enabled
        /// </summary>
        public event Action<Entity> Enabled
        {
            add
            {
                // Invoke immediately if Entity is already enabled
                if (IsEnabled)
                    value(this);
                else
                    _enabled += value;
            }

            remove { _enabled -= value; }
        }

        private readonly JacinthWorld _world;
        private readonly int _hashCode;
        private readonly Guid _id;

        private readonly ConcurrentDictionary<ComponentTypeKey, Component> _components
            = new ConcurrentDictionary<ComponentTypeKey, Component>();

        private bool _isEnabled;

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
        /// Gets whether this Entity is enabled and ready to be used within the World
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
        }

        /// <summary>
        /// Do not allow objects outside of Jacinth to create Entities
        /// </summary>
        internal Entity(JacinthWorld world)
        {
            _isEnabled = false;
            _world = world;
            _id = Guid.NewGuid();
            _hashCode = _id.GetHashCode();
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
            var key = ComponentTypeKey.GetKey<T>();

            if (_components.TryAdd(key, component))
            {
                RaiseComponentAdded(key, component);
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
            return TryGetComponent(out result) ? null : result;
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

            var result = _components.TryGetValue(key, out rawComponent);

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
        /// Removes the Component of the specified type from this Entity.
        /// Destroys this Entity if there are no Components left on this Entity.
        /// </summary>
        /// <typeparam name="T">The type of Component to add to this Entity, used to determine the ComponentTypeKey</typeparam>
        public void RemoveComponent<T>()
            where T : Component
        {
            var key = ComponentTypeKey.GetKey<T>();
            Component component;

            if (_components.TryRemove(key, out component))
            {
                if (_components.Any())
                    RaiseComponentRemoved(key, component);
                else
                    Destroy();
            }
        }

        private void RaiseComponentRemoved(ComponentTypeKey key, Component component)
        {
            Task.Run(() =>
            {
                if(ComponentRemoved != null)
                    ComponentRemoved(this, key, component);
            });
        }
        private void RaiseComponentAdded(ComponentTypeKey key, Component component)
        {
            Task.Run(() =>
            {
                if(ComponentAdded != null)
                    ComponentAdded(this, key, component);
            });
        }

        /// <summary>
        /// Removes all Components from this Entity an removes it from the World
        /// </summary>
        public void Destroy()
        {
            Task.Run(() =>
            {
                if (EntityDestroyed != null) EntityDestroyed(this);
            });

            // Detach all event listeners
            EntityDestroyed = null;
            ComponentAdded = null;
            ComponentRemoved = null;
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
            return _id == other._id;
        }

        /// <summary>
        /// Activates this Entity for use in the World.
        /// <exception cref="System.InvalidOperationException">
        /// Throws an Exception if this Entity has already been Enabled
        /// </exception>
        /// </summary>
        public void Enable()
        {
            if(IsEnabled) throw new InvalidOperationException("Attempted to Enable and Entity which was already Enabled");

            // Set enabled flag now in case Enabled event handlers attempt to attach anything
            _isEnabled = true;

            if (_enabled == null) return;

            _enabled(this);
            _enabled = null;    // Clear the Enabled event after using it - we won't need it again
        }
    }
}
