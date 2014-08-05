using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trasys.Dev.Tools.Mvvm
{
    /// <summary>
    /// Manage all view models of this project.
    /// </summary>
    public abstract class ViewModelLocatorBase<TFactory>
    {
        /// <summary>
        /// Gets the Model Factory Engine for this project.
        /// </summary>
        protected virtual TFactory Factory { get; set; }
    }   

}
