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

namespace aBasics.WpfBindingErrors {

    /// <summary>Raises an event each time a WPF Binding error occurs.</summary>
    public sealed class BindingErrorListener : IDisposable {
        readonly ObservableTraceListener traceListener;

        static BindingErrorListener() {
            PresentationTraceSources.Refresh();
        }

        /// <summary>Initializes a new instance of the <see cref="BindingErrorListener"/> class.</summary>
        public BindingErrorListener() {
            traceListener = new ObservableTraceListener();
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            PresentationTraceSources.DataBindingSource.Listeners.Add(traceListener);
        }

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose() {
            PresentationTraceSources.DataBindingSource.Listeners.Remove(traceListener);
            traceListener.Dispose();
        }

        /// <summary>Event raised each time a WPF binding error occurs</summary>
        public event Action<string> ErrorCatched {
            add { traceListener.TraceCatched += value; }
            remove { traceListener.TraceCatched -= value; }
        }

    }
}
