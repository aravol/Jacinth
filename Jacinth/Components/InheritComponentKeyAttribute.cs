using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacinth.Components
{
    /// <summary>
    /// Attribute to indicate the marked comopnent should use it's base class's ComponentTypeKey.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class InheritComponentKeyAttribute : Attribute
    {
        // Nothing to here but exist
    }
}
