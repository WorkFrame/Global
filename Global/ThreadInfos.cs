using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetEti.Globals
{
    /// <summary>
    /// Liefert textuelle Infos über den aktuellen Thread für Debug-Zwecke.
    /// </summary>
    /// <remarks>
    /// File: ThreadInfos.cs
    /// Autor: Erik Nagel
    ///
    /// 28.09.2013 Erik Nagel: erstellt
    /// </remarks>
    public static class ThreadInfos
    {
        /// <summary>
        /// Liefert textuelle Infos über den aktuellen Thread für Debug-Zwecke.
        /// </summary>
        /// <returns>String mit Informationen zum aktuellen Thread.</returns>
        public static string GetThreadInfos()
        {
          //Process p = Process.GetCurrentProcess();
#pragma warning disable 618
          int mySystemThreadId = AppDomain.GetCurrentThreadId();
#pragma warning restore 618
          int myThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
          return String.Format("{0:d4}/{1:d5}", myThreadId, + mySystemThreadId);
        }
  
    }
}
