// <copyright file="Command.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace YoutubeDownloader.Common {
    using System;
    using System.Windows.Input;

    /// <summary>
    /// A view model command that can be enabled or disabled, and performs a given <see cref="Action"/> when invoked.
    /// Optionally includes display text that can be used to caption the command when used with controls like app
    /// bar commands, context menu items, buttons, etc.
    /// </summary>
    public class Command : BindableBase, ICommand {
        /// <summary>
        /// The action to invoke when this command is executed.
        /// </summary>
        private readonly Action action;

        /// <summary>
        /// Controls whether or not a call to Execute will actually invoke the Action.
        /// </summary>
        private bool isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="invocationAction">The <see cref="Action"/> to perform when the command is invoked.</param>
        /// <param name="isEnabled">A Boolean that specifies whether or not the command is initially enabled.</param>
        public Command(Action invocationAction, bool isEnabled = true)
            : base() {
            this.action = invocationAction.CheckNonNull("invocationAction");
            this.isEnabled = isEnabled;
        }

        /// <summary>
        /// Raised to indicate that a call to the <see cref="Execute"/> method caused the logic associated with this <see cref="Command"/> to execute because <see cref="IsEnabled"/> was <c>true</c>.
        /// </summary>
        public event EventHandler Executed;

        /// <summary>
        /// Raised when the value that would be returned by <see cref="CanExecute"/> changes. 
        /// </summary>
        /// <remarks>
        /// This event is provided for compatibility with <see cref="ICommand.CanExecuteChanged"/>. 
        /// For data binding scenarios, property change notifications from the <see cref="IsEnabled"/> property should be used instead.
        /// </remarks>
        /// <seealso cref="IsEnabled"/>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Gets or sets a value indicating whether this command is currently enabled.
        /// </summary>
        public bool IsEnabled {
            get {
                return this.isEnabled;
            }

            set {
                if (this.SetProperty(ref this.isEnabled, value)) {
                    EventHandler canExecuteChangedHandler = this.CanExecuteChanged;
                    if (canExecuteChangedHandler != null) {
                        canExecuteChangedHandler(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Indicates whether this <see cref="Command"/> will respond to a call to <see cref="Execute"/> by invoking its associated logic.
        /// </summary>
        /// <param name="parameter">The parameter argument is present for compatibility with <see cref="ICommand.CanExecute"/> and is ignored.</param>
        /// <returns><c>true</c> if a call to <see cref="Execute"/> would result in this <see cref="Command"/> invoking its associated logic.</returns>
        /// <remarks>
        /// This method is provided for compatibility with <see cref="ICommand"/>. 
        /// </remarks>
        /// <seealso cref="IsEnabled"/>
        public bool CanExecute(object parameter) {
            return this.IsEnabled;
        }

        /// <summary>
        /// If <see cref="IsEnabled"/> is <c>true</c>, executes the logic associated with this command.
        /// </summary>
        /// <param name="parameter">The parameter argument is present for compatibility with <see cref="ICommand.Execute"/> and is ignored.</param>
        /// <remarks>
        /// If invocation actually proceeds, the <see cref="Executed"/> event is raised.
        /// </remarks>
        /// <seealso cref="Executed"/>
        /// <seealso cref="CanExecute"/>
        /// <seealso cref="IsEnabled"/>
        public void Execute(object parameter = null) {
            if (this.isEnabled) {
                this.action.Invoke();
                EventHandler invokedHandler = this.Executed;
                if (invokedHandler != null) {
                    invokedHandler(this, EventArgs.Empty);
                }
            }
        }
    }
}
