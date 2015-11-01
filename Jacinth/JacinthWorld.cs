using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jacinth.Components;
using Jacinth.Entities;
using Jacinth.Processors;

namespace Jacinth
{
    /// <summary>
    /// World object holding all Entities, Components, and Processors for Jacinth
    /// </summary>
    public sealed class JacinthWorld
    {
        #region Events

        /// <summary>
        /// Raised when an Entity is updated with new Components and requires possible addition to active Processors
        /// </summary>
        internal event Action<Entity> EntityUpdated;
        #endregion

        #region Values

        private readonly object _entityLock = new object();
        private readonly List<Entity> _entities = new List<Entity>();
        private readonly Dictionary<string, ProcessorLoop> _processorLoops = new Dictionary<string, ProcessorLoop>();
        private readonly List<Processor> _processors = new List<Processor>();
        #endregion

        #region Properties

        #region Internal
        
        internal Dictionary<string, ProcessorLoop> ProcessorLoopsInternal
        {
            get
            {
                // Do not allow access to the modifiable Dictionary when the world has already beein initialized
                if (Initialized) throw new InvalidOperationException();
                return _processorLoops;
            }
        }

        internal List<Processor> ProcessorsInternal
        {
            get
            {
                // Do not allow access to modifiable List when the world has already been initialized
                if (Initialized) throw new InvalidOperationException();
                return _processors;
            }
        }
        #endregion

        /// <summary>
        /// Gets whether this world has already been initialized and is running
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// <para>Gets all Entities currently active in this World.</para>
        /// </summary>
        public IEnumerable<Entity> Entities => _entities;

        /// <summary>
        /// <para>Gets all Components currently active in this World.</para>
        /// <para>WARNING: Expensive operation. May need optimization.</para>
        /// </summary>
        public IEnumerable<Component> Components => Entities.SelectMany(e => e.Components);

        /// <summary>
        /// Gets all Processor Loops active in this World in a Dictionary keyed by Name
        /// </summary>
        public IReadOnlyDictionary<string, ProcessorLoop> ProcessorLoops => _processorLoops;

        /// <summary>
        /// Gets all Processors active in this World
        /// </summary>
        public IEnumerable<Processor> Processors => _processors;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Jacinth World
        /// </summary>
        /// <param name="initialize">True to Initialize the World immediately, False otherwise</param>
        public JacinthWorld(bool initialize = true)
        {
            if (initialize) Initialize();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Creates an Entity in this World, but does not enable it.
        /// </summary>
        /// <returns>
        /// The newly created Entity. Must call <see cref="Jacinth.Entities.Entity.Enable"/> to use the Entity within the World
        /// </returns>
        public Entity CreateEntity()
        {
            var result = new Entity(this);
            result.Enabled += EntityOnEnabled;
            return result;
        }

        private void EntityOnEnabled(Entity entity)
        {
            lock (_entityLock)
                _entities.Add(entity);

            entity.ComponentAdded += OnComponentAdded;

            // Immediately call entity update events for the new Entity
            RaiseEntityUpdated(entity);
        }

        internal void RemoveEntity(Entity entity)
        {
            lock (_entityLock)
                _entities.Remove(entity);
        }

        private void OnComponentAdded(Entity entity, ComponentTypeKey key, Component component) => RaiseEntityUpdated(entity);

        /// <summary>
        /// Initializes the world and begins processing Entities
        /// </summary>
        public void Initialize()
        {
            if (Initialized) throw new InvalidOperationException();

            foreach (var procType in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof (Processor))
                    && t.IsAbstract == false
                    && t.ContainsGenericParameters == false))
            {
                var loopAtt = procType.GetCustomAttribute<JacinthProcessorAttribute>();
                var loopKey = loopAtt == null
                    ? ProcessorLoop.UpdateLoopName  // Default to the "Update" loop when none is declared
                    : loopAtt.Name;                 // Use the Name specified by the Attribiute when it is declared
                var proc = (Processor)(Activator.CreateInstance(procType, this));
                ProcessorsInternal.Add(proc);

                GetOrCreateLoop(loopKey).AddProcessor(proc);
            }

            // Perform first-time mass add of all entities to all processors
            foreach (var ent in Entities)
                RaiseEntityUpdated(ent);

            // Set the initialized flag to prevent modification to Loops and Processors
            Initialized = true;
        }
        
        /// <summary>
        /// Gets the ProcessorLoop, or creates and binds it if it is not yet created
        /// </summary>
        public ProcessorLoop GetOrCreateLoop(string name)
        {
            ProcessorLoop result;
            if (ProcessorLoops.TryGetValue(name, out result) == false)
                ProcessorLoopsInternal.Add(name, result = new ProcessorLoop(name, this));

            return result;
        }

        private void RaiseEntityUpdated(Entity target) => EntityUpdated?.Invoke(target);
        #endregion
    }
}
