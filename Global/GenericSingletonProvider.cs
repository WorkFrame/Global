using System.Collections;
using System.Reflection;

namespace NetEti.Globals
{
    /// <summary>
    /// Stellt generische Klassen als Singletons zur Verfügung. Hauptvorteil ist, dass die Nutzklassen selbst
    ///           keine statischen Elemente mehr enthalten müssen und somit prinzipiell vererbbar sind.
    /// </summary>
    /// <remarks>
    /// File: GenericSingletonProvider.cs<br></br>
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
    /// 01.05.2014 Erik Nagel: Aufruf von Global.DynamicIs(instance, typeof(T)) geändert in
    ///                        typeof(T).IsAssignableFrom(instance.GetType()).<br></br>
    /// 19.11.2016 Erik Nagel: Prüfung auf öffentlichen Konstruktor deaktiviert.
    /// </remarks>
    public static class GenericSingletonProvider
    {
        private static Hashtable Selfs = new Hashtable();
        private static object Lock = new Object();

        /// <summary>
        /// Liefert die einzige Instanz der gewünschten Klasse zurück.
        /// Beim ersten Aufruf wird diese Instanz voher erzeugt.
        /// </summary>
        /// <typeparam name="T">Typ der angeforderten Klasse</typeparam>
        /// <param name="newInstance">True, wenn die retournierte Instanz gerade neu erzeugt wurde.</param>
        /// <returns>Eine Instanz der angeforderten Klasse als Singleton</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "It's a generic factory without input parameters")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "only this one and only for value-type (boolean)")]
        public static T GetInstance<T>(out bool newInstance)
          where T : class // Jede Klasse ist erlaubt
        {
            // Auf öffentlichen Konstruktor prüfen
            ConstructorInfo? checkCtor = (typeof(T)).GetConstructor(Type.EmptyTypes);

            /*
             * Falls es einen solchen Konstruktor gibt,
             * schmeißen wir einen Fehler, da es dem 
             * Charakter eines Singleton widerspricht.
             */
            if (checkCtor != null)
            {
                throw new InvalidOperationException("Die als Singleton übergebene Klasse darf keinen öffentlichen Konstruktor besitzen.");
            }

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

                foreach (object instance in Selfs.Values)
                {
                    //if (Global.DynamicIs(instance, typeof(T)))
                    if (typeof(T).IsAssignableFrom(instance.GetType()))
                    {
                        // Instanz existiert bereits
                        newInstance = false;
                        return (T)instance;
                    }
                }

                // Neue Instanz über Reflektion erstellen
                ConstructorInfo? ctorInfo;

                // Geschützte Konstruktoren auslesen
                ctorInfo = typeof(T).GetConstructor(
                            BindingFlags.NonPublic |
                            BindingFlags.Instance,
                            null,
                            Type.EmptyTypes,
                            null
                          );
                if (ctorInfo == null)
                {
                    throw new InvalidOperationException("Die als Singleton übergebene Klasse muss einen privaten Standard-Konstruktor besitzen.");
                }

                // Konstruktor ohne Parameter aufrufen
                T? _Instanz = null;
                try
                {
                    _Instanz = (T)ctorInfo.Invoke(new object[] { });
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException != null)
                    {
                        throw ex.InnerException;
                    }
                    else
                    {
                        throw;
                    }
                }

                if (_Instanz == null)
                {
                    throw new InvalidOperationException("Die übergebene Klasse konnte nicht instanziiert werden.");
                }

                // Instanz der Hashtabelle zuführen
                Selfs.Add(typeof(T).GUID, _Instanz);
                newInstance = true;

                T? result = (T?)Selfs[typeof(T).GUID];
                if (result == null)
                {
                    throw new InvalidOperationException("Interner Fehler beim Array-Zugriff.");
                }

                return result;
            }
        }

        /// <summary>
        /// Liefert die einzige Instanz der gewünschten Klasse zurück.
        /// Beim ersten Aufruf wird diese Instanz voher erzeugt.
        /// </summary>
        /// <typeparam name="T">Typ der angeforderten Klasse</typeparam>
        /// <returns>Eine Instanz der angeforderten Klasse als Singleton</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "It's a generic factory without input parameters")]
        public static T GetInstance<T>()
          where T : class
        {
            bool _Trash;
            return GetInstance<T>(out _Trash);
        }
    }
}
