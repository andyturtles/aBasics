using System;
using System.Collections.Generic;

namespace aBasics {

    /// <summary>
    /// Tools Klasse, für weiterführende Libs
    /// </summary>
    public class BasicTools {

        /// <summary>
        /// Ersetzt das ERSTE Vorkommen von 'stringToReplace' in 'original' mit 'replace'
        /// </summary>
        /// <param name="original">The original Text.</param>
        /// <param name="replace">The replacement.</param>
        /// <param name="stringToReplace">The string to replace.</param>
        /// <returns>Text mit Ersetzungen</returns>
        public static string ReplaceTextFirstOccurrence(string original, string replace, string stringToReplace) {
            int i = original.IndexOf(stringToReplace, StringComparison.Ordinal);
            if ( i >= 0 ) {
                string sub1 = original.Substring(0, i + stringToReplace.Length);
                string sub2 = original.Substring(i    + stringToReplace.Length);
                sub1 = sub1.Replace(stringToReplace, replace);
                return sub1 + sub2;
            }

            return original;
        }

        /// <summary>
        /// Splits the console arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Dictionary key=val</returns>
        public static Dictionary<string, string> SplitConsoleArgs(string[] args) {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach ( string part in args ) {
                string[] spl = part.Split('=');
                dict.Add(spl[0], spl[1]);
            }
            return dict;
        }

        #region File use Date

        /// <summary>
        /// Liefert einen String der das Datum enthält im Format YYYY*MM*TT
        /// Basis ist DateTime.Now
        /// Es werden keine Zeichen eingefügt
        /// Keine Uhrzeit
        /// </summary>
        /// <returns>sting</returns>
        public static string GetFilenameUsableDate() {
            return GetFilenameUsableDate(false);
        }

        /// <summary>
        /// Liefert einen String der das Datum (und die Zeit) enthält im Format YYYY*MM*TT usw...
        /// Basis ist DateTime.Now
        /// Es werden keine Zeichen eingefügt
        /// </summary>
        /// <param name="alsoTime">auch Zeit einbinden</param>
        /// <returns>sting</returns>
        public static string GetFilenameUsableDate(bool alsoTime) {
            return GetFilenameUsableDate("", "", alsoTime);
        }

        /// <summary>
        /// Liefert einen String der das Datum (und die Zeit) enthält im Format YYYY*MM*TT usw...
        /// Basis ist DateTime.Now
        /// </summary>
        /// <param name="trennZeichenDatum">Zeichen die Zwischen das Datum kommen</param>
        /// <param name="trennZeichenZeit">Zeichen die Zwischen die Zeit kommen</param>
        /// <param name="alsoTime">auch Zeit einbinden</param>
        /// <returns>sting</returns>
        public static string GetFilenameUsableDate(string trennZeichenDatum, string trennZeichenZeit, bool alsoTime) {
            return GetFilenameUsableDate(DateTime.Now, trennZeichenDatum, trennZeichenZeit, alsoTime);
        }

        /// <summary>
        /// Liefert einen String der das Datum (und die Zeit) enthält im Format YYYY*MM*TT usw...
        /// </summary>
        /// <param name="dateTime">Datum und Uhrzeit</param>
        /// <param name="trennZeichenDatum">Zeichen die Zwischen das Datum kommen</param>
        /// <param name="trennZeichenZeit">Zeichen die Zwischen die Zeit kommen</param>
        /// <param name="alsoTime">auch Zeit einbinden</param>
        /// <returns>sting</returns>
        public static string GetFilenameUsableDate(DateTime dateTime, string trennZeichenDatum, string trennZeichenZeit, bool alsoTime) {
            string s = "";
            s += dateTime.Year                             + trennZeichenDatum;
            s += dateTime.Month.ToString().PadLeft(2, '0') + trennZeichenDatum;
            s += dateTime.Day.ToString().PadLeft(2, '0');

            if ( alsoTime ) {
                s += "_";
                s += dateTime.Hour.ToString().PadLeft(2, '0')   + trennZeichenZeit;
                s += dateTime.Minute.ToString().PadLeft(2, '0') + trennZeichenZeit;
                s += dateTime.Second.ToString().PadLeft(2, '0');
            }

            return s;
        }

        /// <summary>
        /// Parses the filename usable date.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="trennZeichenDatum">The trenn zeichen datum.</param>
        /// <param name="trennZeichenZeit">The trenn zeichen zeit.</param>
        /// <param name="alsoTime">if set to <c>true</c> [also time].</param>
        /// <returns>DateTime</returns>
        public static DateTime ParseFilenameUsableDate(string input, string trennZeichenDatum = "", string trennZeichenZeit = "", bool alsoTime = false) {
            string s                                          = input;
            if ( !String.IsNullOrEmpty(trennZeichenDatum) ) s = s.Replace(trennZeichenDatum, "");
            if ( !String.IsNullOrEmpty(trennZeichenZeit) ) s  = s.Replace(trennZeichenZeit, "");
            s = s.Replace("_", "");

            int jahr  = Int32.Parse(s.Substring(0, 4));
            int monat = Int32.Parse(s.Substring(4, 2));
            int tag   = Int32.Parse(s.Substring(6, 2));

            if ( alsoTime ) {
                int stunde  = Int32.Parse(s.Substring(8, 2));
                int minute  = Int32.Parse(s.Substring(10, 2));
                int sekunde = Int32.Parse(s.Substring(12, 2));
                return new DateTime(jahr, monat, tag, stunde, minute, sekunde);
            }

            return new DateTime(jahr, monat, tag);
        }

        #endregion File use Date

    }

}
