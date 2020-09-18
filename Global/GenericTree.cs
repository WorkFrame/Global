using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetEti.Globals
{
    /// <summary>
    /// Stellt einen generischen Baum zur Verfügung
    /// </summary>
    /// <remarks>
    /// File: GenericTree.cs
    /// Autor: Erik Nagel
    ///
    /// 01.12.2012 Erik Nagel: erstellt
    /// </remarks>
    /// <typeparam name="T">Typ des konkreten Trees (von GenericTree abgeleitet).</typeparam> 
    public class GenericTree<T> where T : GenericTree<T>
    {
        /// <summary>
        /// Liste der Kinder eines Knotens.
        /// </summary>
        public List<T> Children { get; set; }

        /// <summary>
        /// Der Besitzer des Knoten.
        /// </summary>
        public GenericTree<T> Mother;

        /// <summary>
        /// Konstruktor übernimmt das Mutter-Element.
        /// </summary>
        /// <param name="mother">Das Mutter-Element.</param>
        public GenericTree(GenericTree<T> mother)
        {
            this.Mother = mother;
            this.Children = new List<T>();
        }

        /// <summary>
        /// Geht rekursiv durch den Baum und ruft für jeden Knoten die Action auf.
        /// </summary>
        /// <param name="visitor">Der für jeden Knoten aufzurufende Callback vom Typ Func&lt;int, T, object, object&gt;</param>
        /// <returns>Das oberste UserObjekt für den Tree.</returns>
        public object Traverse(Func<int, T, object, object> visitor)
        {
            return this.traverse(0, visitor, null);
        }

        /// <summary>
        /// Rekursive Hilfsroutine für die öffentliche Routine 'Traverse'.
        /// </summary>
        /// <param name="depth">Die Hierarchie-Ebene.</param>
        /// <param name="visitor">Der aufrufende Knoten (rekursiv).</param>
        /// <param name="userParent">Ein User-Object, das rekursiv weitergeleitet und modifiziert wird.</param>
        /// <returns>Das oberste UserObjekt für den Tree.</returns>
        protected virtual object traverse(int depth, Func<int, T, object, object> visitor, object userParent)
        {
            object nextUserParent = visitor(depth, (T)this, userParent);
            foreach (T child in this.Children)
                child.traverse(depth + 1, visitor, nextUserParent);
            return nextUserParent;
        }

        /// <summary>
        /// Geht rekursiv durch den Baum und ruft für jeden Knoten die Action auf.
        /// </summary>
        /// <param name="visitor">Der für jeden Knoten aufzurufende Callback</param>
        public void Traverse(Action<int, T> visitor)
        {
            this.traverse(0, visitor);
        }

        /// <summary>
        /// Rekursive Hilfsroutine für die öffentliche Routine 'Traverse'.
        /// </summary>
        /// <param name="depth">Die Hierarchie-Ebene.</param>
        /// <param name="visitor">Der aufrufende Knoten (rekursiv).</param>
        protected virtual void traverse(int depth, Action<int, T> visitor)
        {
            visitor(depth, (T)this);
            foreach (T child in this.Children)
                child.traverse(depth + 1, visitor);
        }

        /// <summary>
        /// Hangelt sich durch den Baum nach oben (bis zur Root)
        /// und ruft für jeden Knoten die Action auf.
        /// </summary>
        /// <param name="visitor">Der aufrufende Knoten (rekursiv).</param>
        public void climb2Top(Action<T> visitor)
        {
            visitor((T)this);
            if (this.Mother != null)
            {
                Mother.climb2Top(visitor);
            }
        }

    }
}
