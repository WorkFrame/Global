using System.Collections.Generic;

namespace NetEti.Globals
{
    /// <summary>
    /// Statische Helper-Klasse - stellt statische generische Methoden
    /// zur Verfügung, welche Dictionaries sperren und kopieren.
    /// </summary>
    /// <remarks>
    /// File: DictionaryThreadSafeCopy.cs
    /// Autor: Erik Nagel, NetEti
    ///
    /// 09.08.2016 Erik Nagel, NetEti: erstellt
    /// </remarks>
    /// <typeparam name="T">Typ der IEnumerable-Keys.</typeparam> 
    /// <typeparam name="U">Typ der IEnumerable-Values.</typeparam> 
    public static class DictionaryThreadSafeCopy<T, U>
    {
        /// <summary>
        /// Kopiert thread-safe IDictionary&lt;T, U&gt; source auf IDictionary&lt;T, U&gt; target.
        /// </summary>
        /// <param name="source">IDictionary vom Typ T, U.</param>
        /// <returns>Kopie von source als Dictionary vom Typ T, U.</returns>
        public static IDictionary<T, U> GetDictionaryThreadSafeCopy(IDictionary<T, U> source)
        {
            Dictionary<T, U> target;
            lock (source)
            {
                target = new Dictionary<T, U>(source);
            }
            return target;
        }

        /// <summary>
        /// Kopiert thread-safe die Keys von IDictionary&lt;T, U&gt; source
        /// auf IEnumerable&lt;T&gt; target.
        /// </summary>
        /// <param name="source">IDictionary vom Typ T, U.</param>
        /// <returns>Kopie der Keys von source als List vom Typ T.</returns>
        public static IEnumerable<T> GetDictionaryKeysThreadSafeCopy(IDictionary<T, U> source)
        {
            List<T> target;
            lock (source)
            {
                target = new List<T>(source.Keys);
            }
            return target;
        }

        /// <summary>
        /// Kopiert thread-safe die Values von IDictionary&lt;T, U&gt; source
        /// auf IEnumerable&lt;U&gt; target.
        /// </summary>
        /// <param name="source">IDictionary vom Typ T, U.</param>
        /// <returns>Kopie der Values von source als List vom Typ U.</returns>
        public static IEnumerable<U> GetDictionaryValuesThreadSafeCopy(IDictionary<T, U> source)
        {
            List<U> target;
            lock (source)
            {
                target = new List<U>(source.Values);
            }
            return target;
        }

    }
}
