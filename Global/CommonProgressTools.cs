using System;
using System.ComponentModel;
namespace NetEti.Globals
{
    /// <summary>
    /// Gibt an, für welche Art Items die aktuelle Fortschrittsmeldung erfolgt,
    /// möglich sind: itemParts, Items und itemGroups.
    /// </summary>
    public enum ItemsTypes
    {
        /// <summary>
        /// Die Fortschrittsangabe bezieht sich auf Teile von Items,
        /// z.B. Bytes einer Datei.
        /// </summary>
        itemParts,

        /// <summary>
        /// Die Fortschrittsangabe bezieht sich auf Items, z.B. Dateien.
        /// </summary>
        items,

        /// <summary>
        /// Die Fortschrittsangabe bezieht sich auf eine Gruppe von Items,
        /// z.B. ein Unterverzeichnis oder ein Archiv.
        /// </summary>
        itemGroups
    }

    /// <summary>
    /// Der Handler Delegate-Typ für den CommonProgressChangedEventHandler
    /// </summary>
    /// <param name="sender">Die Event-Quelle</param>
    /// <param name="args">Detail-Informationen zum Verarbeitungsfortschritt</param>
    public delegate void CommonProgressChangedEventHandler(object sender, CommonProgressChangedEventArgs args);

    /// <summary>
    /// Der Handler Delegate-Typ für den CommonProgressFinishedEventHandler
    /// </summary>
    /// <param name="sender">Die Event-Quelle</param>
    /// <param name="threadException">Exception oder null</param>
    public delegate void CommonProgressFinishedEventHandler(object sender, Exception threadException);

    /// <summary>
    /// Enthält Informationen über den Verarbeitungsfortschritt (Bezeichner, Gesamtanzahl, Anzahl, Typ).
    /// </summary>
    /// <remarks>
    /// File: CommonProgressChangedEventArgs.cs
    /// Autor: Erik Nagel, NetEti
    ///
    /// 30.03.2012 Erik Nagel: erstellt.
    /// </remarks>
    public class CommonProgressChangedEventArgs : ProgressChangedEventArgs
    {
        /// <summary>
        /// Name des Items, das gerade bearbeitet wird.
        /// </summary>
        public string ItemName { get; private set; }

        /// <summary>
        /// Anzahl der insgesamt zu verarbeitenden Items.
        /// </summary>
        public long CountAll { get; private set; }

        /// <summary>
        /// Anzahl der erfolgreich verarbeiteten Items.
        /// </summary>
        public long CountSucceeded { get; private set; }

        /// <summary>
        /// Typ der verarbeiteten Items (enum ItemsType: itemParts, items, itemGroups).
        /// </summary>
        public ItemsTypes ItemsType { get; set; }

        /// <summary>
        /// Shell für den geerbten Konstruktor
        /// </summary>
        /// <param name="progressPercentage">prozentualer Fortschritt (0-100)</param>
        /// <param name="userState">ein beliebiges User-Objekt, kann null sein</param>
        public CommonProgressChangedEventArgs(int progressPercentage, Object userState) : base(progressPercentage, userState) { }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="itemName">Name des Items, das gerade bearbeitet wird</param>
        /// <param name="countAll">Anzahl der insgesamt zu verarbeitenden Items</param>
        /// <param name="countSucceeded">Anzahl der erfolgreich verarbeiteten Items</param>
        /// <param name="itemsType">Typ der verarbeiteten Items (enum ItemsType: itemParts, items, itemGroups)</param>
        /// <param name="userState">Ein beliebiges User-Objekt, kann null sein</param>
        public CommonProgressChangedEventArgs(string itemName, long countAll, long countSucceeded, ItemsTypes itemsType, Object userState)
          : base(CommonProgressChangedEventArgs.ComputePercentage(countAll, countSucceeded), userState)

        {
            this.ItemName = itemName;
            this.CountAll = countAll;
            this.CountSucceeded = countSucceeded;
            this.ItemsType = itemsType;
        }

        private static int ComputePercentage(long countAll, long countSucceeded)
        {
            int percent = 0;
            if (countAll > 0)
            {
                percent = (int)((countSucceeded * 100) / countAll);
            }
            return percent <= 100 ? percent : 100;
        }
    }
}
