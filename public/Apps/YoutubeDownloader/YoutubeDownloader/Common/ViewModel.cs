
namespace Tabs.Common {
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Base for all view model types (including page models) that adds support for commands to the existing <see cref="BindableBase"/> capabilities for property notifications.
    /// </summary>
    public abstract class ViewModel : BindableBase {
        /// <summary>
        /// Maps implementing method name to <see cref="Command"/> for this <see cref="ViewModel"/> instance.
        /// </summary>
        private Dictionary<string, Command> methodCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        protected ViewModel() {
        }

        /// <summary>
        /// Creates or retrieves a <see cref="Command"/> that invokes the specified action method.
        /// </summary>
        /// <param name="commandAction">An <see cref="Action"/> that defines the logic of the command. Must be a static or instance method, not a lambda function.</param>
        /// <returns>A new or existing <see cref="Command"/> based on the specified action method.</returns>
        protected Command BindCommandTo(Action commandAction) {
            commandAction.CheckNonNull("commandAction");

            MethodInfo commandMethod = commandAction.GetMethodInfo();
            if (commandMethod == null || commandMethod.Name[0] == '<') {
                throw new ArgumentException("BindCommandTo requires an Action argument that is a static or instance method, not a lambda function", "commandAction");
            }

            if (this.methodCommands == null) {
                this.methodCommands = new Dictionary<string, Command>();
            }

            string commandKey = commandMethod.Name;
            Command result = null;
            if (!this.methodCommands.TryGetValue(commandKey, out result)) {
                result = new Command(commandAction);
                this.methodCommands.Add(commandKey, result);
            }

            return result;
        }
    }
}
