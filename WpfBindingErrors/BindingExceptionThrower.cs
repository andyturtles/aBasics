/* *
 * https://github.com/bblanchon/WpfBindingErrors
 * http://blog.benoitblanchon.fr/wpf-binding-errors/
 * 
 * WPF Binding Error Testing
 * Copyright 2013 Benoit Blanchon
 * 
 * This has been inpired by  
 * http://tech.pro/tutorial/940/wpf-snippet-detecting-binding-errors
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace aBasics.WpfBindingErrors {

    /// <summary>Converts WPF binding error into BindingException</summary>
    public static class BindingExceptionThrower {

        static BindingErrorListener errorListener;
        static List<string> whitelist;

        /// <summary>Start listening WPF binding error</summary>
        public static void Attach() {
            errorListener = new BindingErrorListener();
            errorListener.ErrorCatched += OnErrorCatched;

            if ( File.Exists("WpfExceptionWhitelist.txt") ) whitelist = new List<string>(File.ReadAllLines("WpfExceptionWhitelist.txt"));
            else Log.Info("WpfExceptionWhitelist.txt nicht vorhanden");
        }

        /// <summary>Stop listening WPF binding error</summary>
        public static void Detach() {
            errorListener.ErrorCatched -= OnErrorCatched;
            errorListener.Dispose();
            errorListener = null;
        }

        /// <summary>Gets a value indicating whether this instance is attached.</summary>
        /// <value><c>true</c> if this instance is attached; otherwise, <c>false</c>.</value>
        public static bool IsAttached {
            get { return errorListener != null; }
        }

        [DebuggerStepThrough]
        static void OnErrorCatched(string message) {
            if ( IsOnWhiteList(message) ) Log.Info("BindingExceptionThrower.OnErrorCatched [WL]: " + message);
            else {
                Log.Error("BindingExceptionThrower.OnErrorCatched: " + message);
                
                // Im Release nur Loggen und Exceptions ausgeschaltet, weil WPF selbst Probleme hat die teilweise nicht reproduzierbar sind, aber immer mal wieder Exceptions werden
                // Siehe auch hier: https://social.msdn.microsoft.com/Forums/vstudio/en-US/8028a46b-3080-40ad-aed2-e9417eb25381/binding-to-menuitem-result-in-binding-errors?forum=wpf
#if DEBUG
                throw new BindingException(message);
#endif
            }
        }

        /// <summary>
        /// Prüft ob Schlüsselwörter der Whitelist im Messagetext enthalten sind,
        /// wenn ja wird kein Error sondern nur eine Debug Meldung geschrieben
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>true: onWhitelist</returns>
        private static bool IsOnWhiteList(string message) {
            if ( whitelist == null ) return false;

            foreach ( string s in whitelist ) {
                if ( message.Contains(s) ) return true;
            }

            return false;
        }

    }
}
