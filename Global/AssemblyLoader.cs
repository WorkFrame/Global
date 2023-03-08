using System.Reflection;

namespace NetEti.Globals
{
    /// <summary>
    /// Stellt Methoden für das dynamische Laden von
    /// Assemblies und das Instanziieren darin enthaltener Klassen
    /// zur Verfügung.
    /// </summary>
    /// <remarks>
    /// File: AssemblyLoader.cs
    /// Autor: Erik Nagel
    ///
    /// 10.04.2013 Erik Nagel: erstellt
    /// </remarks>
    public class AssemblyLoader
    {
        #region public members

        /// <summary>
        /// Singleton-Provider - übernimmt Pfade zu Verzeichnissen, in denen zusätzlich
        /// nach Assemblies gesucht werden soll.
        /// </summary>
        /// <returns>Singleton-Instanz von AssemblyLoader</returns>
        public static AssemblyLoader GetAssemblyLoader()
        {
            AssemblyLoader loader = NestedInstance.itsMe;
            loader._mainAssemblyDirectory = "";
            return (loader);
        }

        /// <summary>
        /// Liefert eine Liste mit allen in der AppDomain geladenen Assemblies.
        /// </summary>
        /// <returns>Liste mit allen in der AppDomain geladenen Assemblies.</returns>
        public static List<Assembly> GetLoadedAssemblies()
        {
            Assembly[] appAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            return new List<Assembly>(appAssemblies);
            //for (int i = 0; i < appAssemblies.Length; i++)
            //    Console.WriteLine(
            //        "{0}: {1}\n", i + 1, appAssemblies[i].FullName);
        }

        /// <summary>
        /// Lädt ein Objekt vom übergebenen Typ aus der angegebenen Assembly dynamisch.
        /// Alle von der angegebenen Assembly referenzierten Assemblies werden zusätzlich
        /// auch in assemblyDirectories gesucht.
        /// </summary>
        /// <param name="assemblyPathName">Die Assembly, die das zu ladende Objekt publiziert.</param>
        /// <param name="objectType">Der Typ des aus der Assembly zu instanzierenden Objekts</param>
        /// <returns>Instanz aus der übergebenen Assembly vom übergebenen Typ oder null</returns>
        public object? DynamicLoadObjectOfTypeFromAssembly(string assemblyPathName, Type objectType)
        {
            object? candidate = null;
            Assembly? assembly = DynamicLoadAssembly(assemblyPathName);
            if (assembly != null)
            {
                Type[] exports = assembly.GetExportedTypes();
                foreach (Type type in exports)
                {
                    try
                    {
                        candidate = Activator.CreateInstance(type);
                    }
                    catch { }
                    if (candidate != null && objectType.IsAssignableFrom(candidate.GetType()))
                    {
                        break;
                    }
                    else
                    {
                        candidate = null;
                    }
                }
            }
            return candidate;
        }

        /// <summary>
        /// Lädt die Assembly vom übergebenen Pfad.
        /// </summary>
        /// <param name="assemblyPath">Pfad der zu ladenden Assembly.</param>
        /// <returns>Geladene Assembly oder null</returns>
        public Assembly? DynamicLoadAssembly(string assemblyPath)
        {
            if (File.Exists(assemblyPath))
            {
                this._mainAssemblyDirectory = Path.GetDirectoryName(assemblyPath);
            }
            return this.dynamicLoadAssembly(assemblyPath);
        }

        #endregion public members

        #region private members

        private string? _mainAssemblyDirectory;

        /// <summary>
        /// Lädt die Assembly vom übergebenen Pfad.
        /// </summary>
        /// <param name="assemblyPath">Pfad der zu ladenden Assembly.</param>
        /// <returns>Geladene Assembly oder null</returns>
        private Assembly? dynamicLoadAssembly(string assemblyPath)
        {
            Assembly? candidate = null;
            if (!File.Exists(assemblyPath))
            {
                assemblyPath = Path.Combine(this._mainAssemblyDirectory?? "", Path.GetFileName(assemblyPath));
            }
            if (File.Exists(assemblyPath))
            {
                try
                {
                    // Gibt die Dll im Filesystem direkt nach Laden wieder frei.
                    candidate = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(assemblyPath));
                    // Blockt die Dll im Filesystem nach Laden solange, wie die Applikation läuft.
                    // candidate = Assembly.LoadFrom(slavePathName);
                }
                catch { }
            }
            return candidate;
        }

        /// <summary>
        /// Privater Konstruktor.
        /// </summary>
        private AssemblyLoader()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(myResolveEventHandler);
        }

        private Assembly? myResolveEventHandler(object? sender, ResolveEventArgs args)
        {
            return DynamicLoadAssembly(args.Name.Split(',')[0] + ".dll");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "due to lazyness of the singleton instance")]
        private class NestedInstance
        {
            internal static readonly AssemblyLoader itsMe;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "due to lazyness of the singleton instance")]
            static NestedInstance()
            {
                itsMe = new AssemblyLoader();
            }
        }

        #endregion private members

    }
}
