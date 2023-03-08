namespace NetEti.Globals
{
    /// <summary>
    /// Lesen von Values (string) über Keys (string).
    /// </summary>
    /// <remarks>
    /// File: IGetStringValue.cs<br></br>
    /// Autor: Erik Nagel, NetEti<br></br>
    ///<br></br>
    /// 08.03.2012 Erik Nagel: erstellt.<br></br>
    /// 29.07.2018 Erik Nagel: Description eingeführt.<br></br>
    /// </remarks>
    public interface IGetStringValue
    {
        /// <summary>
        /// Liefert genau einen Wert zu einem Key. Wenn es keinen Wert zu dem
        /// Key gibt, wird defaultValue zurückgegeben.
        /// </summary>
        /// <param name="key">Der Zugriffsschlüssel (string)</param>
        /// <param name="defaultValue">Das default-Ergebnis (string)</param>
        /// <returns>Der Ergebnis-String</returns>
        string? GetStringValue(string key, string? defaultValue);

        /// <summary>
        /// Liefert ein string-Array zu einem Key. Wenn es keinen Wert zu dem
        /// Key gibt, wird defaultValue zurückgegeben.
        /// </summary>
        /// <param name="key">Der Zugriffsschlüssel (string)</param>
        /// <param name="defaultValues">Das default-Ergebnis (string[])</param>
        /// <returns>Das Ergebnis-String-Array</returns>
        string?[]? GetStringValues(string key, string?[]? defaultValues);

        /// <summary>
        /// Liefert einen beschreibenden Namen dieses StringValueGetters,
        /// z.B. Name plus ggf. Quellpfad.
        /// </summary>
        string Description { get; set; }
    }
}
