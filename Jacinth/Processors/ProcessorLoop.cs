using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Jacinth.Processors
{
    /// <summary>
    /// Represents the execution loop of a given Processor, determining when it should fire and update Entities
    /// </summary>
    public sealed class ProcessorLoop
    {
        /// <summary>
        /// Name of the Update Loop,
        /// used as a default when Loop is not specified for a Processor
        /// </summary>
        public const string UpdateLoopName = "Update";

        private readonly string _name;
        private readonly JacinthWorld _world;
        private readonly List<Processor> _processors = new List<Processor>();
        private readonly Stopwatch _deltaWatch = Stopwatch.StartNew();

        private Task _entityUpdates = Task.Run(() => { });  // Nop-op default for when this loops is first created

        /// <summary>
        /// Gets the World to which this Loop is attached
        /// </summary>
        public JacinthWorld World { get { return _world; } }

        /// <summary>
        /// Gets the Name of this loop, which is how it is keyed
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets or sets whether to execute each Processor in this Loop one-at-a-time or in parallel
        /// </summary>
        public bool SynchronousExecution { get; set; }

        /// <summary>
        /// Gets all Processors currently subscribed to this ProcessorLoop
        /// </summary>
        public IEnumerable<Processor> Processors { get { return _processors; } } 

        internal ProcessorLoop(string name, JacinthWorld world)
        {
            _name = name;
            _world = world;
        }

        /// <summary>
        /// Execute all Processors associated with this Loop
        /// </summary>
        public void Execute()
        {
            // Wait for any Entity Updates to finish processing
            _entityUpdates.Wait();

            // Take the current delta time and restart the stopwatch
            var deltaTime = _deltaWatch.Elapsed;    // By caching this value, there will be a slight inaccuracy due to the actual time taken by each processor,
                                                    // but we can more accurately garuantee the accuracy between multiple executions of the loop itself
            _deltaWatch.Restart();

            foreach(var proc in SynchronousExecution
                ? Processors
                : (IEnumerable<Processor>)(Processors.AsParallel()))
            {
                proc.Process(deltaTime);
            }

            // Begin processing Entity updates asynchronously
            _entityUpdates = Task.Run((Action)(UpdateEntities));
        }

        // Updates all Entities in all Processors attached to this Loop
        internal void UpdateEntities()
        {
            foreach (var proc in SynchronousExecution
                ? Processors
                : (IEnumerable<Processor>)(Processors.AsParallel()))
            {
                proc.UpdateEntities();
            }
        }

        internal void AddProcessor(Processor proc)
        {
            if (World.Initialized) throw new InvalidOperationException();

            _processors.Add(proc);
        }
    }
}