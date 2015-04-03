// <copyright file="PageModelSettings.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace YoutubeDownloader.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using YoutubeDownloader.Common;

    /// <summary>
    /// Base type for serializable settings classes that encapsulate all of the saved state required
    /// by a top-level view model (PageModel) to reconstruct itself and any contained view models.
    /// </summary>
    public abstract class PageModelSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelSettings"/> class.
        /// </summary>
        protected PageModelSettings()
        {
        }

        /// <summary>
        /// Overridden in derived types to initialize the instance of <see cref="PageModelSettings"/> based on the serialized property values in <paramref name="savedValues"/>.
        /// </summary>
        /// <param name="savedValues">A dictionary that maps property names to string-encoded property values that should be used to initialize this <see cref="PageModelSettings"/> instance.</param>
        public abstract void Load(IDictionary<string, string> savedValues);

        /// <summary>
        /// Overridden in derived types to serialized the property values of this <see cref="PageModelSettings"/> instance as a collection of property-name/string-encoded property-value pairs.
        /// </summary>
        /// <returns>The property name/string-encoded property value pairs for the serialized property values of this <see cref="PageModelSettings"/> instance.</returns>
        public abstract IEnumerable<KeyValuePair<string, string>> Save();

        #region Serialization helpers

        /// <summary>
        /// Attempts to retrieve a setting value of type T with the specified name from the given serialized setting values, returning a default value if the setting is not present.
        /// </summary>
        /// <typeparam name="T">type the setting is expected to be</typeparam>
        /// <param name="fromValues">The dictionary of property name/string-encoded property value pairs from which to load the value.</param>
        /// <param name="settingName">The name with which to look up the value in the dictionary.</param>
        /// <param name="defaultValue">The default value to return if no property value with the specified name is found in the dictionary.</param>
        /// <returns>The Boolean setting value found in the dictionary, or <paramref name="defaultValue"/> if no setting with the specified name was found.</returns>
        protected static T LoadValue<T>(IDictionary<string, string> fromValues, string settingName, T defaultValue)
        {
            T result = defaultValue;
            string value;

            if (fromValues.TryGetValue(settingName, out value))
            {
                result = (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }

            return result;
        }

        /// <summary>
        /// Generates a key-value pair for the specified setting of type T.
        /// </summary>
        /// <typeparam name="T">type the setting is expected to be</typeparam>
        /// <param name="settingName">The setting name the specifies the key.</param>
        /// <param name="value">The String setting value.</param>
        /// <returns>A new <see cref="KeyValuePair&lt;string, string&gt;"/> containing the specified setting name and value.</returns>
        protected static KeyValuePair<string, string> SaveValue<T>(string settingName, T value)
        {
            string stringValue = (string)Convert.ChangeType(value, typeof(string), CultureInfo.InvariantCulture);
            KeyValuePair<string, string> result = new KeyValuePair<string, string>(settingName, stringValue);
            return result;
        }

        #endregion

        /// <summary>
        /// This class is a convenience for page models that do not require settings, and can therefore be declared
        /// as deriving from <see cref="PageModel&lt;PageModelSettings.None&gt;"/>.
        /// </summary>
        public sealed class None : PageModelSettings
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PageModelSettings.None"/> class.
            /// </summary>
            public None()
            {
            }

            /// <summary>
            /// <see cref="PageModelSettings.None"/> does not support <see cref="Load"/> or <see cref="Save"/> operations.
            /// Calling this method on an instance of <see cref="PageModelSettings.None"/> will have no effect.
            /// </summary>
            /// <param name="savedValues">This parameter is ignored.</param>
            public override void Load(IDictionary<string, string> savedValues)
            {
                // Nothing to load.
            }

            /// <summary>
            /// <see cref="PageModelSettings.None"/> does not support <see cref="Load"/> or <see cref="Save"/> operations.
            /// Calling this method on an instance of <see cref="PageModelSettings.None"/> will return an empty collection.
            /// </summary>
            /// <returns>Always returns an empty <see cref="IEnumerable&lt;KeyValuePair&lt;string, string&gt;&gt;"/>.</returns>
            public override IEnumerable<KeyValuePair<string, string>> Save()
            {
                // Nothing to save.
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }
        }
    }
}
