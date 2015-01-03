using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
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
        #region Values

        private readonly object _tableLock = new object();
        private readonly Dictionary<EntityComponentKey, Component> _componentTable = new Dictionary<EntityComponentKey, Component>();
        private readonly Dictionary<string, ProcessorLoop> _processorLoops = new Dictionary<string, ProcessorLoop>();
        private readonly List<Processor> _processors = new List<Processor>();
        #endregion

        #region Properties

        #region Internal

        /// <summary>
        /// Internal object for locking when the ComponentsTable is modified.
        /// </summary>
        internal object TableLock
        {
            get { return _tableLock; }
        }

        /// <summary>
        /// Internal storage for Components in Jacinth.
        ///  Highly subject to change, very internal.
        ///  Expect lots of documentation and frequent changes for optimization here.
        /// </summary>
        internal Dictionary<EntityComponentKey, Component> ComponentTable
        {
            get { return _componentTable; }
        }

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
        /// <para>WARNING: Expensive operation. Needs optimization.</para>
        /// </summary>
        public IEnumerable<Entity> Entities
        {
            get
            {
                return ComponentTable.Keys
                    .Select(t => t.Entity)
                    .Distinct();
            }
        }

        /// <summary>
        /// Gets all Components currently active in this World.
        /// </summary>
        public IEnumerable<Component> Components
        {
            get { return ComponentTable.Values; }
        }

        public IReadOnlyDictionary<string, ProcessorLoop> ProcessorLoops
        {
            get { return _processorLoops; }
        }

        public IEnumerable<Processor> Processors
        {
            get { return _processors; }
        }
        #endregion

        #region Constructors

        public JacinthWorld(bool initialize = false)
        {
            if (initialize) Initialize();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Creates an Entity in this World
        /// </summary>
        /// <returns>The newly created Entity</returns>
        public Entity CreateEntity()
        {
            var result = new Entity(this);

            result.ComponentAdded += (sender, e) =>
            {
                foreach (var p in Processors) p.QueueEntityAdd(result);
            };

            return result;
        }

        public void Initialize()
        {
            foreach (var procType in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof (Processor))
                    && t.IsAbstract == false
                    && t.ContainsGenericParameters == false))
                // TODO: Filter out Processors in the same inheritance chain
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
                foreach (var proc in Processors)
                    proc.QueueEntityAdd(ent);

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
        #endregion
    }
}
