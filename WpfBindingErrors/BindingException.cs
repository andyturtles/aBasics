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

namespace aBasics.WpfBindingErrors {

    /// <summary>
    /// Exception thrown by the BindingExceptionThrower each time a WPF binding error occurs
    /// </summary>
    [Serializable]
    public class BindingException : Exception {

        /// <summary>Initializes a new instance of the <see cref="BindingException"/> class.</summary>
        /// <param name="message">Die Meldung, in der der Fehler beschrieben wird.</param>
        public BindingException(string message) : base(message) {

        }

    }
}
