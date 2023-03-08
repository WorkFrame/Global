namespace NetEti.Globals
{
    /// <summary>
    /// Statische Helper-Klasse - stellt eine statische Methode
    /// "GetEnumerableThreadSafeCopy" zur Verfügung, welche generische
    /// Listen sperrt und kopiert.
    /// </summary>
    /// <remarks>
    /// File: EnumerableThreadSaveCopy.cs
    /// Autor: Erik Nagel, NetEti
    ///
    /// 09.08.2016 Erik Nagel, NetEti: erstellt
    /// </remarks>
    /// <typeparam name="T">Typ eines konkreten IEnumerable-Elements.</typeparam> 
    public static class EnumerableThreadSafeCopy<T>
    {
        /// <summary>
        /// Kopiert thread-safe IEnumerable&lt;T&gt; source auf IEnumerable&lt;T&gt; target.
        /// </summary>
        /// <param name="source">IEnumerable vom Typ T.</param>
        /// <returns>Kopie von source als List vom Typ T.</returns>
        public static IEnumerable<T> GetEnumerableThreadSafeCopy(IEnumerable<T> source)
        {
            List<T> target;
            lock (source)
            {
                target = new List<T>(source);
            }
            return target;
        }
    }
}
