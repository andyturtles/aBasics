using System;
using System.Windows.Input;

namespace aBasics.WpfBasics {

    /// <summary>
    /// RelayCommand
    /// </summary>
    public class RelayCommand : RelayCommand<object> {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) : base(execute, canExecute) { }
    }

    /// <summary>
    /// RelayCommand
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand {
        private Action<T> execute;
        private Func<T, bool> canExecute;

        /// <summary>
        /// Tritt ein, wenn Änderungen auftreten, die sich auf die Ausführung des Befehls auswirken.
        /// </summary>
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null) {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Definiert die Methode, die bestimmt, ob der Befehl im aktuellen Zustand ausgeführt werden kann.
        /// </summary>
        /// <param name="parameter">Vom Befehl verwendete Daten.Wenn der Befehl keine Datenübergabe erfordert, kann das Objekt auf null festgelegt werden.</param>
        /// <returns>
        /// true, wenn der Befehl ausgeführt werden kann, andernfalls false.
        /// </returns>
        public bool CanExecute(object parameter) {
            return this.canExecute == null || this.canExecute((T)parameter);
        }

        /// <summary>
        /// Definiert die Methode, die aufgerufen wird, wenn der Befehl aufgerufen wird.
        /// </summary>
        /// <param name="parameter">Vom Befehl verwendete Daten.Wenn der Befehl keine Datenübergabe erfordert, kann das Objekt auf null festgelegt werden.</param>
        public void Execute(object parameter) {
            this.execute((T)parameter);
        }

    }
}
