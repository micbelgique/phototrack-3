using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trasys.Dev.Tools.Data
{
    /// <summary>
    /// Argument of ExceptionOccured event.
    /// </summary>
    public class ExceptionOccuredEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }
}
