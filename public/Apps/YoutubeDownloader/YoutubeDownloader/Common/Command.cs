
namespace YoutubeDownloader.Common {
    using System;
    using System.Windows.Input;

    public class Command : BindableBase, ICommand {
        private readonly Action action;

        private bool isEnabled;

        public Command(Action invocationAction, bool isEnabled = true)
            : base() {
            this.action = invocationAction.CheckNonNull("invocationAction");
            this.isEnabled = isEnabled;
        }

        public event EventHandler Executed;

        public event EventHandler CanExecuteChanged;

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

        public bool CanExecute(object parameter) {
            return this.IsEnabled;
        }

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
