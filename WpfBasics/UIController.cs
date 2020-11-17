using System.ComponentModel;

namespace aBasics.WpfBasics {

    /// <summary>
    /// UIController
    /// </summary>
    public abstract class UIController : INotifyPropertyChanged {

        /// <summary>
        /// Initializes a new instance of the <see cref="UIController"/> class.
        /// </summary>
        public UIController() {
        }

        /// <summary>
        /// Tritt ein, wenn sich ein Eigenschaftswert ändert.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void NotifyPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        public abstract void NotifyPropertyChanged();

    }
}
