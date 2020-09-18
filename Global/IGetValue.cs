namespace NetEti.Globals
{
    /// <summary>
    /// Lesen von Values (Typ) über Keys (string).
    /// </summary>
    /// <remarks>
    /// File: IGetValue.cs<br></br>
    /// Autor: Erik Nagel, NetEti<br></br>
    ///<br></br>
    /// 23.04.2013 Erik Nagel: erstellt.<br></br>
    /// </remarks>
    public interface IGetValue
    {
        /// <summary>
        /// Liefert genau einen Wert zu einem Key. Wenn es keinen Wert zu dem
        /// Key gibt, wird defaultValue zurückgegeben.
        /// </summary>
        /// <param name="key">Der Zugriffsschlüssel (string)</param>
        /// <param name="defaultValue">Das default-Ergebnis (string)</param>
        /// <typeparam name="T">Typ des konkreten Values.</typeparam> 
        /// <returns>Der Ergebnis-String</returns>
        T GetValue<T>(string key, T defaultValue);
        
        /// <summary>
        /// Liefert ein string-Array zu einem Key. Wenn es keinen Wert zu dem
        /// Key gibt, wird defaultValue zurückgegeben.
        /// </summary>
        /// <param name="key">Der Zugriffsschlüssel (string)</param>
        /// <param name="defaultValues">Das default-Ergebnis (string[])</param>
        /// <typeparam name="T">Typ der konkreten Values.</typeparam> 
        /// <returns>Das Ergebnis-String-Array</returns>
        T[] GetValues<T>(string key, T[] defaultValues);
    }
}
