<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader.Common
{
    public class BindableBase: INotifyPropertyChanged
    {
        /// <summary>
        /// Multicast event for property change notifications.
=======
﻿
namespace YoutubeDownloader.Common {
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged {
        /// <summary>
        /// Raised to indicate that the value of one of this instance's properties has been changed.
>>>>>>> f146f91c5c7cfa85454dc6ae1a419f1180d64703
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
<<<<<<< HEAD
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
=======
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
            bool isDifferent = !object.Equals(storage, value);
            if (isDifferent) {
                storage = value;
                this.NotifyPropertyChanged(propertyName);
            }

            return isDifferent;
>>>>>>> f146f91c5c7cfa85454dc6ae1a419f1180d64703
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
<<<<<<< HEAD
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
=======
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null) {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null) {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Notifies listeners that all property values have potentially changed.
        /// </summary>
        protected void NotifyPropertiesChanged() {
            // A null property name causes any registered listener to fire.
            this.NotifyPropertyChanged();
        }
>>>>>>> f146f91c5c7cfa85454dc6ae1a419f1180d64703
    }
}
