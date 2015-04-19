using System;

namespace Jacinth.Processors
{
    /// <summary>
    /// Makrs a class as a Jacinth Processor, allowing it to be picked up and instantiated by the Jacinth World
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JacinthProcessorAttribute : Attribute
    {
        private readonly string _name;

        /// <summary>
        /// <para>Gets the name of the Loop to hook this Processor into.</para>
        /// <para>Defaults to "Update".</para>
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <param name="name">The name of the Loop to subscribe this Processor to</param>
        public JacinthProcessorAttribute(string name = ProcessorLoop.UpdateLoopName)
        {
            _name = name;
        }
    }
}
