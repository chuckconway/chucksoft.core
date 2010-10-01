using System.Collections.Generic;
using System.Web.Mvc;

namespace Chucksoft.Core.Web.Mvc.State
{
    public class SessionStateLessProvider : ITempDataProvider
    {
        /// <summary>
        /// Loads the temporary data.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <returns>The temporary data.</returns>
        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            return new Dictionary<string, object>();
        } 

        /// <summary>
        /// Saves the temporary data.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="values">The values.</param>
        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values){}
    }
}