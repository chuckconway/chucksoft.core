using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chucksoft.Core
{
   public interface IId<T>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        T Id{ get; set; }
    }
}
