
namespace YoutubeDownloader.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using YoutubeDownloader.Common;

    public abstract class PageModelSettings
    {
        protected PageModelSettings()
        {
        }

        public abstract void Load(IDictionary<string, string> savedValues);

        public abstract IEnumerable<KeyValuePair<string, string>> Save();

        #region Serialization helpers

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

        protected static KeyValuePair<string, string> SaveValue<T>(string settingName, T value)
        {
            string stringValue = (string)Convert.ChangeType(value, typeof(string), CultureInfo.InvariantCulture);
            KeyValuePair<string, string> result = new KeyValuePair<string, string>(settingName, stringValue);
            return result;
        }

        #endregion

        public sealed class None : PageModelSettings
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PageModelSettings.None"/> class.
            /// </summary>
            public None()
            {
            }

            public override void Load(IDictionary<string, string> savedValues)
            {
                // Nothing to load.
            }

            public override IEnumerable<KeyValuePair<string, string>> Save()
            {
                // Nothing to save.
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }
        }
    }
}
