using System;
using System.Collections;
using System.Reflection;

namespace NetEti.Globals
{
    /// <summary>
    /// Stellt generische Klassen mit private Standard-Konstruktor als Nicht-Singletons zur Verfügung.
    /// </summary>
    /// <remarks>
    /// File: GenericNonSingletonProvider.cs<br></br>
    /// Quelle: josupeit.com/Weblog/Informatik und Technik/C# und .NET im Allgemeinen/Implementierung des Singleton
    ///         mit kleineren Anpassungen von Erik Nagel, NetEti<br></br>
    ///<br></br>
    /// 08.03.2012 Erik Nagel: erstellt<br></br>
    /// 08.03.2012 Erik Nagel: Es werden jetzt nicht nur dann existierende Instanzen zurückgegeben,
    ///                        wenn sie genau den generischen Typ T haben, sondern auch, wenn sie von T
    ///                        abgeleitet sind. Dadurch kann zum Beispiel innerhalb eines Frameworks auf
    ///                        dort bekannte Basisklassen von unbekannten aber abgeleiteten Klassen als
    ///                        Singletons zugegriffen werden, auch wenn die von den Basisklassen abgeleiteten
    ///                        Klassen außerhalb des Frameworks erst instanziiert werden.<br></br>
    /// </remarks>
    public static class GenericNonSingletonProvider
    {
        private static Hashtable Selfs = new Hashtable();
        private static object Lock = new Object();

        /// <summary>
        /// Liefert eine neue Instanz der gewünschten Klasse zurück.
        /// </summary>
        /// <typeparam name="T">Typ der angeforderten Klasse</typeparam>
        /// <param name="newInstance">Immer True.</param>
        /// <returns>Eine Instanz der angeforderten Klasse.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "It's a generic factory without input parameters")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "only this one and only for value-type (boolean)")]
        public static T GetInstance<T>(out bool newInstance)
          where T : class // Jede Klasse ist erlaubt
        {
            // Auf öffentlichen Konstruktor prüfen
            ConstructorInfo checkCtor = (typeof(T)).GetConstructor(Type.EmptyTypes);

            //if (checkCtor != null)
            //  throw new InvalidOperationException("Die übergebene Klasse darf keinen öffentlichen Konstruktor besitzen.");

            // Threadsynchronisation
            lock (Lock)
            {
                /* direkte Übereinstimmung
                if (Selfs.ContainsKey(typeof(T).GUID))
                {
                    // Instanz existiert bereits
                    New = false;
                    return (T)Selfs[typeof(T).GUID];
                }
                */

                /*
                foreach (object instance in Selfs.Values)
                {
                  if (Global.DynamicIs(instance, typeof(T)))
                  {
                    // Instanz existiert bereits
                    newInstance = false;
                    return (T)instance;
                  }
                }
                */

                // Neue Instanz über Reflektion erstellen
                ConstructorInfo ctorInfo;

                // Geschützte Konstruktoren auslesen
                ctorInfo = typeof(T).GetConstructor(
                            BindingFlags.NonPublic |
                            BindingFlags.Instance,
                            null,
                            Type.EmptyTypes,
                            null
                          );

                // Konstruktor ohne Parameter aufrufen
                T _Instanz = (T)ctorInfo.Invoke(new object[] { });

                // Instanz der Hashtabelle zuführen
                // Selfs.Add(typeof(T).GUID, _Instanz);
                newInstance = true;

                return _Instanz;
            }
        }

        /// <summary>
        /// Liefert eine Instanz der gewünschten Klasse zurück.
        /// </summary>
        /// <typeparam name="T">Typ der angeforderten Klasse</typeparam>
        /// <returns>Eine neue Instanz der angeforderten Klasse.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "It's a generic factory without input parameters")]
        public static T GetInstance<T>()
          where T : class
        {
            bool _Trash;
            return GetInstance<T>(out _Trash);
        }
    }
}
