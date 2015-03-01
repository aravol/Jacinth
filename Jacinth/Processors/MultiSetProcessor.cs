using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jacinth.Entities;

namespace Jacinth.Processors
{
    /// <summary>
    /// Base class for a Processor which acts with multiple active Entity sets
    /// </summary>
    public abstract class MultiSetProcessor<T1, T2> : Processor
        where T1 : SubEntity
        where T2 : SubEntity
    {
        private SubEntitySet<T1> _subEntitySet1 = new SubEntitySet<T1>();
        private SubEntitySet<T2> _subEntitySet2 = new SubEntitySet<T2>();

        /// <summary>
        /// Gets all the SubEntities of the first type currently being processed by this Processor
        /// </summary>
        public IEnumerable<T1> ActiveSubEntities1
        {
            get { return _subEntitySet1.ActiveSubEntities; }
        }

        /// <summary>
        /// Gets all the SubEntities of the second type currently being processed by this Processor
        /// </summary>
        public IEnumerable<T2> ActiveSubEntities2
        {
            get { return _subEntitySet2.ActiveSubEntities; }
        }

        /// <summary>
        /// Creates a new MultiSetProcessor in the given Jacinth World
        /// </summary>
        /// <param name="world"></param>
        protected MultiSetProcessor(JacinthWorld world)
            : base(world)
        {
            world.EntityUpdated += _subEntitySet1.QueueEntityAdd;
            world.EntityUpdated += _subEntitySet2.QueueEntityAdd;
        }

        internal sealed override void UpdateEntities()
        {
            Parallel.Invoke(
                _subEntitySet1.UpdateEntities,
                _subEntitySet2.UpdateEntities);
        }
    }

    /// <summary>
    /// Base class for a Processor which acts with multiple active Entity sets
    /// </summary>
    public abstract class MultiSetProcessor<T1, T2, T3> : Processor
        where T1 : SubEntity
        where T2 : SubEntity
        where T3 : SubEntity
    {
        private SubEntitySet<T1> _subEntitySet1 = new SubEntitySet<T1>();
        private SubEntitySet<T2> _subEntitySet2 = new SubEntitySet<T2>();
        private SubEntitySet<T3> _subEntitySet3 = new SubEntitySet<T3>();

        /// <summary>
        /// Gets all the SubEntities of the first type currently being processed by this Processor
        /// </summary>
        public IEnumerable<T1> ActiveSubEntities1
        {
            get { return _subEntitySet1.ActiveSubEntities; }
        }

        /// <summary>
        /// Gets all the SubEntities of the second type currently being processed by this Processor
        /// </summary>
        public IEnumerable<T2> ActiveSubEntities2
        {
            get { return _subEntitySet2.ActiveSubEntities; }
        }

        /// <summary>
        /// Gets all the SubEntities of the third type currently being processed by this Processor
        /// </summary>
        public IEnumerable<T3> ActiveSubEntities3
        {
            get { return _subEntitySet3.ActiveSubEntities; }
        }

        /// <summary>
        /// Creates a new MultiSetProcessor in the given Jacinth World
        /// </summary>
        /// <param name="world"></param>
        protected MultiSetProcessor(JacinthWorld world)
            : base(world)
        {
            world.EntityUpdated += _subEntitySet1.QueueEntityAdd;
            world.EntityUpdated += _subEntitySet2.QueueEntityAdd;
            world.EntityUpdated += _subEntitySet3.QueueEntityAdd;
        }

        internal sealed override void UpdateEntities()
        {
            Parallel.Invoke(
                _subEntitySet1.UpdateEntities,
                _subEntitySet2.UpdateEntities,
                _subEntitySet3.UpdateEntities);
        }
    }

    /// <summary>
    /// Base class for a Processor which acts with multiple active Entity sets
    /// </summary>
    public abstract class MultiSetProcessor<T1, T2, T3, T4> : Processor
        where T1 : SubEntity
        where T2 : SubEntity
        where T3 : SubEntity
        where T4 : SubEntity
    {
        private SubEntitySet<T1> _subEntitySet1 = new SubEntitySet<T1>();
        private SubEntitySet<T2> _subEntitySet2 = new SubEntitySet<T2>();
        private SubEntitySet<T3> _subEntitySet3 = new SubEntitySet<T3>();
        private SubEntitySet<T4> _subEntitySet4 = new SubEntitySet<T4>();

        /// <summary>
        /// Gets all the SubEntities of the first type currently being processed by this Processor
        /// </summary>
        public IEnumerable<T1> ActiveSubEntities1
        {
            get { return _subEntitySet1.ActiveSubEntities; }
        }

        /// <summary>
        /// Gets all the SubEntities of the second type currently being processed by this Processor
        /// </summary>
        public IEnumerable<T2> ActiveSubEntities2
        {
            get { return _subEntitySet2.ActiveSubEntities; }
        }

        /// <summary>
        /// Gets all the SubEntities of the third type currently being processed by this Processor
        /// </summary>
        public IEnumerable<T3> ActiveSubEntities3
        {
            get { return _subEntitySet3.ActiveSubEntities; }
        }

        /// <summary>
        /// Gets all the SubEntities of the fourth type currently being processed by this Processor
        /// </summary>
        public IEnumerable<T4> ActiveSubEntities4
        {
            get { return _subEntitySet4.ActiveSubEntities; }
        }

        /// <summary>
        /// Creates a new MultiSetProcessor in the given Jacinth World
        /// </summary>
        /// <param name="world"></param>
        protected MultiSetProcessor(JacinthWorld world)
            : base(world)
        {
            world.EntityUpdated += _subEntitySet1.QueueEntityAdd;
            world.EntityUpdated += _subEntitySet2.QueueEntityAdd;
            world.EntityUpdated += _subEntitySet3.QueueEntityAdd;
            world.EntityUpdated += _subEntitySet4.QueueEntityAdd;
        }

        internal sealed override void UpdateEntities()
        {
            Parallel.Invoke(
                _subEntitySet1.UpdateEntities,
                _subEntitySet2.UpdateEntities,
                _subEntitySet3.UpdateEntities,
                _subEntitySet4.UpdateEntities);
        }
    }
}
