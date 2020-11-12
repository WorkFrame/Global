using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Security;

namespace NetEti.Globals
{
    /// <summary>
    /// <para xml:lang="de">
    /// Globale Typen, Konstanten und statische Funktionen
    /// </para>
    /// <para xml:lang="en">
    /// Global types, constants and static functions
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para xml:lang="de">
    /// File: Global.cs
    /// Autor: Erik Nagel, NetEti
    ///
    /// 12.03.2012 Erik Nagel: erstellt.
    /// 14.06.2012 Erik Nagel: Korrektur in GetUniversalName.
    /// 13.08.2012 Erik Nagel: IsDate(string inputDate) implementiert.
    /// 01.05.2014 Erik Nagel: DynamicIs(instance, typeof(T)) gestrichen, stattdessen kann
    ///                        typeof(T).IsAssignableFrom(instance.GetType()) eingesetzt werden.<br></br>
    /// </para>
    /// <para xml:lang="en">
    /// File: Global.cs
    /// Author: Erik Nagel, NetEti
    ///
    /// 12.03.2012 Erik Nagel: created.
    /// 14.06.2012 Erik Nagel: Correction in GetUniversalName.
    /// 13.08.2012 Erik Nagel: IsDate(string inputDate) implemented.
    /// 01.05.2014 Erik Nagel: DynamicIs(instance, typeof(T)) deleted, instead one can insert
    ///                        typeof(T).IsAssignableFrom(instance.GetType()).<br></br>
    /// </para>
    /// </remarks>
    public static class Global
    {
        #region public members

        /// <summary>
        /// <para xml:lang="de">
        /// Trenner für String-Inhalte, welcher einerseits selbst nicht in
        /// Strings vorkommt und andererseits wie ein Leerzeichen dargestellt wird.
        /// </para>
        /// <para xml:lang="en">
        /// Delimiter for string-contents. This delimiter doesn`t appear in
        /// strings but behaves like a space.
        /// </para>
        /// </summary>
        public const char SaveColumnDelimiter = ' '; // Hex FF

        /// <summary>
        /// <para xml:lang="de">
        /// Der Standard-Spaltentrenner in CSV-Dateien
        /// </para>
        /// <para xml:lang="en">
        /// Standard-column-delimiter in CSV-files
        /// </para>
        /// </summary>
        public const char StandardCsvColumnDelimiter = ';';

        /* Entfällt, da IsAssignableFrom besser is (findet auch implementierte Interfaces).
           Mir war IsAssignableFrom zum Zeitpunkt der Entwicklung von DynamicIs nicht bekannt.
           Not longer necessary, because IsAssignableFrom does a better job (recognizes implemented interfaces too).
           I wasn't aware of IsAssignableFrom at the moment, whe I developed DynamicIs.
        /// <summary>
        /// Überprüft ob ein Objekt von einem bestimmten Typ oder einem
        /// davon abgeleiteten Typ ist.
        /// Die hart verdrahtete Variante wäre:
        ///     'return (inQuestion-object is Referenz-Klasse);'
        ///     hier muss Referenz-Klasse zur Compile-Zeit bekannt sein.
        /// </summary>
        /// <param name="instanceInQuestion">Das zu prüfende Objekt</param>
        /// <param name="referenceType">Der Typ, auf den geprüft wird</param>
        /// <returns>True wenn 'objectInQuestion' zu 'referenceType' kompatibel ist</returns>
        public static bool DynamicIs(object instanceInQuestion, Type referenceType)
        {
          Type qType = instanceInQuestion.GetType();
          while ((!qType.Equals(referenceType)) && ((qType = qType.BaseType) != null)) { }
          return ((qType != null) && (qType.Equals(referenceType)));
        }
         */

        /// <summary>
        /// <para xml:lang="de">
        /// Wandelt einen übergebenen String in einen SecureString
        /// </para>
        /// <para xml:lang="en">
        /// Converts a given string to a SecureString
        /// </para>
        /// </summary>
        /// <param name="current">
        /// <para xml:lang="de">
        /// Ein String
        /// </para>
        /// <para xml:lang="en">
        /// A String
        /// </para>
        /// </param>
        /// <returns>
        /// <para xml:lang="de">
        /// Der entsprechende SecureString
        /// </para>
        /// <para xml:lang="en">
        /// The created SecureString
        /// </para>
        /// </returns>
        public static SecureString StringToSecureString(string current)
        {
            var secure = new SecureString();
            foreach (var c in current.ToCharArray()) secure.AppendChar(c);
            return secure;
        }

        /// <summary>
        /// <para xml:lang="de">
        /// Konvertiert einen übergebenen absoluten Dateipfad in sein
        /// UNC-Äquivalent, wenn möglich.
        /// </para>
        /// <para xml:lang="en">
        /// Converts a given absolute file path into it's
        /// UNC-equivalent, if possible.
        /// </para>
        /// </summary>
        /// <param name="sFilePath">
        /// <para xml:lang="de">
        /// Der absolute (Netzwerk-)Pfad
        /// </para>
        /// <para xml:lang="en">
        /// The absolute (Network-)Path
        /// </para>
        /// </param>
        /// <returns>
        /// <para xml:lang="de">
        /// Der UNC-Pfad
        /// </para>
        /// <para xml:lang="en">
        /// The unc-path
        /// </para>
        /// </returns>
        public static string GetUniversalName(string sFilePath)
        {
            if (String.IsNullOrEmpty(sFilePath) || sFilePath.IndexOf(":", StringComparison.CurrentCulture) > 1
                || sFilePath.StartsWith("\\\\", StringComparison.CurrentCulture))
            {
                return sFilePath.Trim();
            }
            if (sFilePath.StartsWith("\\", StringComparison.CurrentCulture))
            {
                return (new Uri(sFilePath)).ToString().Trim();
            }
            string logicalDriveName = (sFilePath + "  ").Substring(0, 2).ToUpper(System.Globalization.CultureInfo.CurrentCulture);
            string uncName = logicalDriveName.Trim();
            string orgName = uncName;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
              "SELECT RemoteName FROM win32_NetworkConnection WHERE LocalName = '"
               + logicalDriveName + "'");
            foreach (ManagementObject managementObject in searcher.Get())
            {
                string foundName = (managementObject["RemoteName"] as string).Trim();
                if (!foundName.Equals(orgName))
                {
                    uncName = managementObject["RemoteName"] as string;
                    break;
                }
            }
            // Ergebnisse werden gecached, da der Zugriff über
            // ManagementObjectSearcher länger dauert.
            // Results are cached, because access via ManagementObjectSearcher is time consuming.
            if (!uncName.Equals(orgName))
            {
                if (Global._alreadySearchedDrives.ContainsKey(logicalDriveName))
                {
                    Global._alreadySearchedDrives.Remove(logicalDriveName);
                }
                Global._alreadySearchedDrives.Add(logicalDriveName, uncName);
            }
            string sRemoteName = uncName + (sFilePath + "  ").Substring(2).Trim();
            // sFilePath = (new Uri(sRemoteName)).ToString().Trim();
            sFilePath = sRemoteName;
            return sFilePath.Trim();
        }

        /// <summary>
        /// <para xml:lang="de">
        /// Vergleicht zwei Versionsstrings, die aus durch '.' separierten Zahlen bestehen,
        /// z.B.: 1.0.0.10 mit 1.0.0.9.
        /// </para>
        /// <para xml:lang="en">
        /// Compares two version-strings, consisting of '.' separated lines,
        /// i.e.: 1.0.0.10 mit 1.0.0.9.
        /// </para>
        /// </summary>
        /// <param name="version1">
        /// <para xml:lang="de">
        /// Versionsstring im Format 1.0.0.10.
        /// </para>
        /// <para xml:lang="en">
        /// Version-string of format 1.0.0.10.
        /// </para>
        /// </param>
        /// <param name="version2">
        /// <para xml:lang="de">
        /// Versionsstring im Format 1.0.0.9.
        /// </para>
        /// <para xml:lang="en">
        /// Version-string of format 1.0.0.9.
        /// </para>
        /// </param>
        /// <returns>
        /// <para xml:lang="de">
        /// Liefert 1 wenn Version 1 größer als Version 2 ist, 0 bei Gleichheit, ansonsten -1.
        /// </para>
        /// <para xml:lang="en">
        /// Returns 1 if version 1 is greater than version 2, zero if equal, -1 otherways
        /// </para>
        /// </returns>
        public static int CompareVersion(string version1, string version2)
        {
            string[] ver1 = version1.Split('.');
            string[] ver2 = version2.Split('.');
            // 26.07.2011 Nagel: no catch
            //try
            //{
            for (int i = 0; i < (ver1.Length < ver2.Length ? ver1.Length : ver2.Length); i++)
            {
                if (Convert.ToInt32(ver1[i], System.Globalization.CultureInfo.CurrentCulture)
                    > Convert.ToInt32(ver2[i], System.Globalization.CultureInfo.CurrentCulture))
                {
                    return 1;
                }
                else
                {
                    if (Convert.ToInt32(ver1[i], System.Globalization.CultureInfo.CurrentCulture)
                        < Convert.ToInt32(ver2[i], System.Globalization.CultureInfo.CurrentCulture))
                    {
                        return -1;
                    }
                }
            }
            //}
            //catch (System.FormatException)
            //{
            //    return 0;
            //}
            //catch (System.OverflowException)
            //{
            //    return 0;
            //}
            if (ver1.Length > ver2.Length)
            {
                return 1;
            }
            else
            {
                if (ver1.Length < ver2.Length)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        } // public static int CompareVersion(string version1, string version2)

        /// <summary>
        /// <para xml:lang="de">
        /// Wandelt eine Zeichenkette, die einen Unicode-Hexcode darstellt z.B.: 20AC (=> €) in das entsprechende Zeichen um. 
        /// </para>
        /// <para xml:lang="en">
        /// Converts a string, representing a hex-unicode character like 20AC, to ascii €.
        /// </para>
        /// </summary>
        /// <param name="unicodeHexcode">
        /// <para xml:lang="de">
        /// Der vierstellige Hexcode des Unicode-Zeichensatzes. Führende 0en müssen nicht angegeben werden.
        /// </para>
        /// <para xml:lang="en">
        /// Hex-unicode consisting of four characters like 20AC. Leading zeroes are optional.
        /// </para>
        /// </param>
        /// <param name="defaultValue">
        /// <para xml:lang="de">
        /// Der Standardwert, der zurückgegebenen wird, wenn es sich nicht um einen maximal vierselligen Hexcode handelt.
        /// </para>
        /// <para xml:lang="en">
        /// Default returned, if there was no conversion possible.
        /// </para>
        /// </param>
        /// <returns>
        /// <para xml:lang="de">
        /// Das Zeichen des Unicode-Zeichensatzes, das durch den Hexcode klassifiziert wird.
        /// </para>
        /// <para xml:lang="en">
        /// Ascii-equivalent to the given unicode-sequence.
        /// </para>
        /// </returns>
        public static string UnicodeHexcodeToChar(string unicodeHexcode, string defaultValue)
        {
            string hexcode = unicodeHexcode;
            while (hexcode.Length < 4)
            {
                hexcode = "0" + hexcode;
            }
            if (hexcode.Length == 4)
            {
                Int32 intValue = 0;
                for (int i = 0; i < unicodeHexcode.Length; i++)
                {
                    string zeichen = unicodeHexcode.Substring(i, 1);
                    Int32 wertigkeit = Int32.Parse(zeichen, System.Globalization.NumberStyles.HexNumber);
                    intValue += Convert.ToInt32(wertigkeit * Math.Pow(16, 3 - i));
                }
                return ((char)intValue).ToString();
            }
            return defaultValue;
        }

        /// <summary>
        /// <para xml:lang="de">
        /// Returnt true, wenn der übergebene String ein gültiges Datum ist.
        /// </para>
        /// <para xml:lang="en">
        /// Returns true, if the given string is a valid date.
        /// </para>
        /// </summary>
        /// <param name="inputDate">
        /// <para xml:lang="de">
        /// Zu prüfender Datums-String
        /// </para>
        /// <para xml:lang="en">
        /// Date-string to be checked.
        /// </para>
        /// </param>
        /// <returns>
        /// <para xml:lang="de">
        /// Returnt true, wenn der übergebene String ein gültiges Datum ist.
        /// </para>
        /// <para xml:lang="en">
        /// Returns true, if the given string is a valid date.
        /// </para>
        /// </returns>
        public static bool IsDate(string inputDate)
        {
            DateTime dt;
            return DateTime.TryParse(inputDate, out dt);
        }

        #endregion public members

        #region private members

        private static Dictionary<string, string> _alreadySearchedDrives;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Standard-initialization doesn't work here")]
        static Global()
        {
            _alreadySearchedDrives = new Dictionary<string, string>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType != DriveType.Network)
                {
                    string driveKey = d.Name.TrimEnd(new char[] { '\\' }).ToUpper(System.Globalization.CultureInfo.CurrentCulture);
                    Global._alreadySearchedDrives.Add(driveKey, driveKey);
                }
            }
        }

        #endregion private members

    }
}
