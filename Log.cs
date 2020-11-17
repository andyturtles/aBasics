using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace aBasics {

    /// <summary>
    /// Klasse mit statischen Methodem zum loggen...
    /// </summary>
    public static class Log {

        /// <summary>
        /// Die verschiedenen Level mit denen geloggt werden kann, 
        /// es wird nur geschrieben wenn das aktuell gesetze Level kleiner/gleich dem der gewählten Methode ist.
        /// Bsp. Level ist "INFO": Es werden alle Logs geschrieben ausser Debug.
        /// 
        /// Die Reihenfolge gibt die Wichtigkeit wieder, hier mal Anhaltspunkte für die Verwendung:
        /// 
        /// ERROR: the system is in distress, customers are probably being affected (or will soon be) and the fix probably requires human intervention.
        /// 
        /// WARNING: an unexpected technical or business event happened, customers may be affected, but probably no immediate human intervention is required. Basically any issue that needs to be tracked but may not require immediate intervention.
        /// 
        /// INFO: things we want to see at high volume in case we need to forensically analyze an issue. System lifecycle events (system start, stop) go here. "Session" lifecycle events (login, logout, etc.) go here. Significant boundary events should be considered as well (e.g. database calls, remote API calls). Typical business exceptions can go here (e.g. login failed due to bad credentials).
        /// 
        /// DEBUG: just about everything that doesn't make the "info" cut... any message that is helpful in tracking the flow through the system and isolating issues, especially during the development and QA phases. 
        /// 
        /// Quelle: https://stackoverflow.com/questions/7839565/logging-levels-logback-rule-of-thumb-to-assign-log-levels
        /// </summary>
        public enum LEVEL {
            /// <summary>Es wird überhauptnicht geloggt</summary>
            NO_LOG		= -1,
            
            /// <summary>Es werden nur Fehler geloggt</summary>
            ERROR		= 0,

            /// <summary>Es werden nur Warnungen, Sicherheitsrelevante Aktivitäten und Fehler geloggt</summary>
            WARNING		= 2,

            /// <summary>Es werden Info-Meldungen, Warnungen, Sicherheitsrelevante Aktivitäten und Fehler geloggt</summary>
            INFO		= 4,

            /// <summary>Es werden Debug Hinweise, Info-Meldungen, Warnungen, Sicherheitsrelevante Aktivitäten und Fehler geloggt</summary>
            DEBUG		= 6
        }

        
        private static Log.LEVEL level		        = Log.LEVEL.ERROR;
        private static string logdir	            = "C:\\logs\\";				// Verzeichniss
        private static readonly object lockDummy    = new object();
        private static Encoding encodingToUse       = Encoding.Default;

        internal static string logPostfix           = "";

        private static string defaultLogFileName = "App-Log.txt";
        /// <summary>Gets the default name of the log file.</summary>
        /// <value>The default name of the log file.</value>
        internal static string DefaultLogFileName {
            get { return defaultLogFileName; }
        }
         
        /// <summary>
        /// Setzt den Filenamen
        /// </summary>
        /// <param name="name">The name.</param>
        public static void SetLogFilename(string name){
            lock ( lockDummy ) {
                defaultLogFileName = name;
            }
        }

        /// <summary>
        /// Gets or sets the encoding to use.
        /// </summary>
        /// <value>The encoding to use.</value>
        public static Encoding EncodingToUse {
            get { return encodingToUse; }
            set { encodingToUse = value; }
        }

        /// <summary>
        /// Replaces the replacements in message.
        /// </summary>
        /// <param name="msg">message</param>
        /// <param name="replacments">replacments</param>
        /// <returns>new message</returns>
        private static string ReplaceReplacementsInMessage(string msg, object[] replacments) {
            string toRepl = "{}";
            if ( replacments == null ) return msg;
            if ( replacments.Length < 1 ) return msg;
            
            foreach ( object o in replacments ) {
                string s;
                if ( o == null )    s = "-NULL-";
                else                s = o.ToString();
                if ( !msg.Contains(toRepl) ) break;
                msg = BasicTools.ReplaceTextFirstOccurrence(msg, s, toRepl);
            }

            return msg;
        }

        #region ERROR ...
        /// <summary>
        /// Log schreiben, Level Error, keine Exception übergeben
        /// </summary>
        /// <param name="message">Meldung</param>
        public static void Error(string message){
            Error(message, null, true);
        }

        /// <summary>
        /// Log schreiben, Level Error, keine Exception übergeben
        /// Es können Platzhalter in der Meldung verwendet werden '{}',
        /// dieser werden der Reihe nach durch die Objekte im 'replacments'-Array ersetzt
        /// </summary>
        /// <param name="message">Meldung</param>
        /// <param name="replacments">objekte die verwendet werden um die Platzhalter '{}' in der Meldung zu ersetzen</param>
        public static void Error(string message, params object[] replacments){
            if ( level >= Log.LEVEL.ERROR ) {
                message = ReplaceReplacementsInMessage(message, replacments);
                Error(message, null, true);
            }
        }

        /// <summary>
        /// Log schreiben, Level Error, Exception übergeben,
        /// die Quelle und Meldung der Exception wird an die Message angehängt.
        /// </summary>
        /// <param name="message">Meldung</param>
        /// <param name="ex">Exception, oder null</param>
        public static void Error(string message, Exception ex) {
            Error(message, ex, true);
        }

        /// <summary>
        /// Log schreiben, Level Error, Exception übergeben,
        /// die Quelle und Meldung der Exception wird an die Message angehängt.
        /// </summary>
        /// <param name="message">Meldung</param>
        /// <param name="ex">Exception, oder null</param>
        /// <param name="recurseInnerException">if set to <c>true</c> [recurse inner exception].</param>
        public static void Error(string message, Exception ex, bool recurseInnerException = true) {
            if ( level >= Log.LEVEL.ERROR ) {
                lock ( lockDummy ) {
                    message += GetExcpetionText(ex);
                    WriteToFile(Log.LEVEL.ERROR, message);
                    if ( recurseInnerException && ( ex != null ) && ( ex.InnerException != null ) ) {
                        Error("InnerException", ex.InnerException);
                    }
                }
            }
        }
        #endregion ERROR

        #region WARNING ...
        /// <summary>
        /// Log schreiben, Level Warning, keine Exception übergeben
        /// </summary>
        /// <param name="message">Meldung</param>
        public static void Warning(string message){
            Warning(message, null, null);
        }

        /// <summary>
        /// Log schreiben, Level Warning, keine Exception übergeben
        /// Es können Platzhalter in der Meldung verwendet werden '{}',
        /// dieser werden der Reihe nach durch die Objekte im 'replacments'-Array ersetzt
        /// </summary>
        /// <param name="message">Meldung</param>
        /// <param name="replacments">objekte die verwendet werden um die Platzhalter '{}' in der Meldung zu ersetzen</param>
        public static void Warning(string message, params object[] replacments){
            Warning(message, null, replacments);
        }

        /// <summary>
        /// Log schreiben, Level Error, Exception übergeben,
        /// die Quelle und Meldung der Exception wird an die Message angehängt.
        /// </summary>
        /// <param name="message">Meldung</param>
        /// <param name="ex">Exception, oder null</param>
        /// <param name="replacments">objekte die verwendet werden um die Platzhalter '{}' in der Meldung zu ersetzen</param>
        public static void Warning(string message, Exception ex, params object[] replacments) {
            if ( level >= Log.LEVEL.WARNING ) {
                lock ( lockDummy ) {
                    message = ReplaceReplacementsInMessage(message, replacments);
                    message += GetExcpetionText(ex);
                    WriteToFile(Log.LEVEL.WARNING, message);
                    if ( ( ex != null ) && ( ex.InnerException != null ) ) {
                        Warning("InnerException", ex.InnerException);
                    }
                }
            }
        }
        #endregion WARNING

        /// <summary>
        /// Gets the excpetion text.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>Text Representation der Exception</returns>
        public static string GetExcpetionText(Exception ex) {
            if ( ex == null ) return "";
            string txt = "";
            if ( ex.Message != null )       txt += "\n\t\t\t\t FEHLER: " + ex.Message;
            if ( ex.Source != null )        txt += "\n\t\t\t\t QUELLE: " + ex.Source;
            if ( ex.StackTrace != null )    txt += "\n\t\t\t\t STACK: " + ex.StackTrace.Replace("\n", "\n\t\t\t\t\t\t");

            //if ( ex is AutomaticDbSaveException ) {
            //    List<ADbOCheckError> errLst = ( (AutomaticDbSaveException)ex ).CheckErrorList;
            //    if ( ( errLst != null ) && ( errLst.Count > 0 ) ) {
            //        foreach ( ADbOCheckError adbErr in errLst ) {
            //            txt += "\n\t\t\t\t ADBOCHECKERROR: " + adbErr.ToString();
            //        }
            //    }
            //}

            return txt;
        }

        /// <summary>
        /// Log schreiben, Level Debug
        /// Es können Platzhalter in der Meldung verwendet werden '{}',
        /// dieser werden der Reihe nach durch die Objekte im 'replacments'-Array ersetzt
        /// </summary>
        /// <param name="message">Meldung</param>
        /// <param name="replacments">objekte die verwendet werden um die Platzhalter '{}' in der Meldung zu ersetzen</param>
        public static void Debug(string message, params object[] replacments) {
            if ( level >= Log.LEVEL.DEBUG ) {
                message = ReplaceReplacementsInMessage(message, replacments);
                Debug(message);
            }
        }

        /// <summary>
        /// Debugs the specified log file name.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public static void Debug(string message, Exception ex) {
            if ( level >= Log.LEVEL.DEBUG ) {
                message += GetExcpetionText(ex);
                Debug(message);
            }
        }

        /// <summary>
        /// Log schreiben, Level Debug
        /// </summary>
        /// <param name="message">Meldung</param>
        public static void Debug(string message) {
            if ( level >= Log.LEVEL.DEBUG ) {
                lock ( lockDummy ) {
                    WriteToFile(Log.LEVEL.DEBUG, message);
                }
            }
        }

        /// <summary>
        /// Log schreiben, Level Debug
        /// Es können Platzhalter in der Meldung verwendet werden '{}',
        /// dieser werden der Reihe nach durch die Objekte im 'replacments'-Array ersetzt
        /// </summary>
        /// <param name="message">Meldung</param>
        /// <param name="replacments">objekte die verwendet werden um die Platzhalter '{}' in der Meldung zu ersetzen</param>
        public static void Info(string message, params object[] replacments) {
            if ( level >= Log.LEVEL.INFO ) {
                message = ReplaceReplacementsInMessage(message, replacments);
                Info(message);
            }
        }

        /// <summary>
        /// Log schreiben, Level Info
        /// </summary>
        /// <param name="message">Meldung</param>
        public static void Info(string message) {
            if ( level >= Log.LEVEL.INFO ) {
                lock ( lockDummy ) {
                    WriteToFile(Log.LEVEL.INFO, message);
                }
            }
        }


        /// <summary>
        /// Interne Methode zum schreiben in das LogFile
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="Message">Meldung</param>
        private static void WriteToFile(Log.LEVEL level, string Message) {
            string logname = BasicTools.GetFilenameUsableDate("-", "", false) + "_" + defaultLogFileName;

            if ( !String.IsNullOrEmpty(logPostfix) ) Message += " | " + logPostfix;

            try {       // wenn Verzeichniss nicht existiert, versuchen anzulegen
                if ( !Directory.Exists(logdir) ) Directory.CreateDirectory(logdir);
            }
            catch ( Exception ex ) {
                throw new Exception("Der Ordner für LogFiles '" + logdir + "' exisitiert nicht und kann auch nicht erstellt werden!", ex);
            }

            StreamWriter w          = null;
            try {
                string logfile      = Path.Combine(logdir, logname);
                w                   = GetWriter(logfile, Message);
                if ( w == null ) return;

                DateTime now        = DateTime.Now;

                w.Write(now.Hour.ToString().PadLeft(2, '0') + ":" + now.Minute.ToString().PadLeft(2, '0') + ":" + now.Second.ToString().PadLeft(2, '0') + "." + now.Millisecond.ToString().PadLeft(3, '0'));
                w.Write("    ");
                w.Write(level.ToString() + ":    ");
                w.WriteLine(Message);
                w.Flush();
            }
            finally {
                if ( w != null ) w.Close();
            }
        }

        /// <summary>
        /// Erzeugen der Writers für das Schreiben des Logs ins Log-File
        /// Wenn mehrere Prozesse / Anwendungen gleichzeitig in ein File schreiben wollen,
        /// dann kann es zu einer IOException kommen 
        /// ( Der Prozess kann nicht auf die Datei "bla.blub" zugreifen, da sie von einem anderen Prozess verwendet wird. )
        /// Hier wird diese Exception gefangen und gewartet und dann nochmal proiert, maximal 5 mal
        /// wenn es dann immernoch nicht klappt, dann schreiben wir das gazne ins Windows Event Log und brechen ab
        /// </summary>
        /// <param name="logfile">The logfile.</param>
        /// <param name="msgToLog">The MSG to log.</param>
        /// <returns>StreamWriter</returns>
        private static StreamWriter GetWriter(string logfile, string msgToLog) {
            StreamWriter w  = null;
            bool newFile    = !File.Exists(logfile);
            int cnt         = 0;
            while ( w == null ) {
                try {
                    w = new StreamWriter(logfile, true, encodingToUse);
                }
                catch ( IOException ) {
                    w = null;
                }
                if ( cnt++ > 5 ) {
                    LogToWindowsEventLog("Log Writer konnte nach 4 Versuchen nicht erstellt werden, breche ab, Meldung folgt.", EventLogEntryType.Error);
                    LogToWindowsEventLog(msgToLog, EventLogEntryType.Warning);
                    break;
                }
                if ( w == null ) Thread.Sleep(100);
            }
            if ( newFile && ( w != null ) ) w.WriteLine("_v2_"); // Kennzeichnung das neue Version (Tausch Debug/Info) verwendet wird
            return w;
        }

        /// <summary>
        /// Logs to windows event log.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="typ">The typ.</param>
        internal static void LogToWindowsEventLog(string msg, EventLogEntryType typ) {
            if ( !EventLog.SourceExists(defaultLogFileName) ) EventLog.CreateEventSource(defaultLogFileName, "Application");
            EventLog.WriteEntry(defaultLogFileName, msg, typ);
        }

        #region Set's File & Level...
        /// <summary>
        /// Setzt das LogLevel
        /// </summary>
        /// <param name="lev">Level</param>
        public static void SetLevel(Log.LEVEL lev){
            level = lev;
        }

        /// <summary>
        /// Setzt das LogLevel anhand eines Namens 
        /// Mögliche Werte 'NO_LOG', 'ERROR', 'DEBUG' und 'INFO'
        /// </summary>
        /// <param name="levelName">Level als String</param>
        /// <param name="defaultLevel">Level welches gesetzt wird wenn der übergebene String nicht eindeutig einem Level zuzuordnen ist</param>
        public static void SetLevel(string levelName, Log.LEVEL defaultLevel) {
            if ( String.IsNullOrEmpty(levelName) ) {
                SetLevel(defaultLevel);
                return;
            }
            if      ( levelName.ToUpper() == "NO_LOG" )     SetLevel(Log.LEVEL.NO_LOG);
            else if ( levelName.ToUpper() == "DEBUG" )      SetLevel(Log.LEVEL.DEBUG);
            else if ( levelName.ToUpper() == "INFO" )       SetLevel(Log.LEVEL.INFO);
            else if ( levelName.ToUpper() == "WARNING" )    SetLevel(Log.LEVEL.WARNING);
            else if ( levelName.ToUpper() == "ERROR" )      SetLevel(Log.LEVEL.ERROR);
            else                                            SetLevel(defaultLevel);
        }

        /// <summary>
        /// Setzt das Verzeichnis
        /// </summary>
        /// <param name="dir">The dir.</param>
        public static void SetLogFileDirectory(string dir){
            lock ( lockDummy ) {
                logdir = dir;
            }
        }

        /// <summary>
        /// Liefert das aktuelle Verzeichnis
        /// </summary>
        /// <returns>Directory</returns>
        public static string GetLogFileDirectory(){
            return logdir;
        }

        #endregion

    }
}


