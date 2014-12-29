using System;
using System.Collections.Generic;
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
    public sealed class Entity : IDisposable
    {
        private readonly JacinthWorld _world;
        private readonly int _hashCode;
        private readonly Guid _id;

        public event EventHandler<ComponentRemovedEventArgs> ComponentRemoved;
        public event EventHandler ComponentAdded;

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
            get
            {
                return World.ComponentTable.Keys
                    .Where(t => t.Item1 == this)
                    .Select(k => World.ComponentTable[k]);
            }
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

            lock (World.TableLock)
                World.ComponentTable.Add(
                    Tuple.Create(this, ComponentTypeKey.GetKey<T>()),
                    component);

            RaiseComponentAdded();
        }

        public T GetComponent<T>()
            where T : Component
        {
            return (T)(World.ComponentTable[Tuple.Create(this, ComponentTypeKey.GetKey<T>())]);
        }

        public T GetOrCreateComponent<T>()
            where T : Component, new()
        {
            var key = Tuple.Create(this, ComponentTypeKey.GetKey<T>());
            Component result;
            if (World.ComponentTable.TryGetValue(key, out result))
                return (T) result; // Hard cast to propogate an InvalidCastException if used incorrectly
            else
                return AddComponent<T>();
        }

        public bool HasComponent<T>()
            where T : Component
        {
            var key = Tuple.Create(this, ComponentTypeKey.GetKey<T>());
            return World.ComponentTable.ContainsKey(key);
        }

        public void RemoveComponent<T>()
            where T : Component
        {
            var typeKey = ComponentTypeKey.GetKey<T>();
            var key = Tuple.Create(this, typeKey);

            lock(World.TableLock)
                World.ComponentTable.Remove(key);

            RaiseComponentRemoved(typeKey);
        }

        private void RaiseComponentRemoved(ComponentTypeKey key)
        {
            Task.Run(() => ComponentRemoved.Invoke(this, new ComponentRemovedEventArgs(this, key)));
        }

        private void RaiseComponentAdded()
        {
            Task.Run(() => ComponentAdded.Invoke(this, EventArgs.Empty));
        }

        public void Dispose()
        {
            foreach(var key in World
                .ComponentTable
                .Keys
                .Where(k => k.Item1 == this)
                .ToArray())
            {
                lock (World.TableLock)
                    World.ComponentTable.Remove(key);

                RaiseComponentRemoved(key.Item2);
            }
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is Entity
                && ((Entity)(obj))._id == this._id;
        }
    }
}
