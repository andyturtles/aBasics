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

using System;
using System.Diagnostics;
using System.Text;

namespace aBasics.WpfBindingErrors {

    /// <summary>A TraceListener that raise an event each time a trace is written</summary>
    sealed class ObservableTraceListener : TraceListener {
        StringBuilder buffer = new StringBuilder();

        /// <summary>
        /// Schreibt beim Überschreiben in einer abgeleiteten Klasse die angegebene Meldung in den Listener, den Sie in der abgeleiteten Klasse erstellen.
        /// </summary>
        /// <param name="message">Eine zu schreibende Meldung.</param>
        public override void Write(string message) {
            buffer.Append(message);
        }

        /// <summary>
        /// Schreibt beim Überschreiben in einer abgeleiteten Klasse eine Meldung gefolgt von einem Zeilenabschluss in den Listener, den Sie in der abgeleiteten Klasse erstellen.
        /// </summary>
        /// <param name="message">Eine zu schreibende Meldung.</param>
        [DebuggerStepThrough]
        public override void WriteLine(string message) {
            buffer.Append(message);

            if ( TraceCatched != null ) {
                TraceCatched(buffer.ToString());
            }

            buffer.Clear();
        }

        public event Action<string> TraceCatched;

    }
}
