// <copyright file="PageModel.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace YoutubeDownloader.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Base type for top-level view model ('page model') classes that use a specific page model settings type to encapsulate saved state.
    /// </summary>
    /// <seealso cref="PageModelSettings"/>
    /// <typeparam name="TSettings">The specific page model settings type used by a concrete subtype.</typeparam>
    public abstract class PageModel<TSettings> : PageModelBase
        where TSettings : PageModelSettings, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModel&lt;TSettings&gt;" /> class.
        /// </summary>
        protected PageModel()
        {
        }

        /// <summary>
        /// Initializes this <see cref="PageModelBase"/> from the previously serialized string-encoded <see cref="PageModelSettings"/> values in <paramref name="savedValues"/>.
        /// </summary>
        /// <param name="savedValues">A dictionary of string-encoded <see cref="PageModelSettings"/> values previously serialized by the correct settings type for this page model.</param>
        public override void Initialize(IDictionary<string, string> savedValues)
        {
            TSettings pageModelSettings = new TSettings();
            if (savedValues != null &&
                savedValues.Count > 0)
            {
                pageModelSettings.Load(savedValues);
            }

            this.Initialize(pageModelSettings);
        }

        /// <summary>
        /// Perform appropriate initialization for your page model based on its settings.
        /// </summary>
        /// <param name="settings">The settings instance that should be used to initialize the page model.</param>
        public abstract void Initialize(TSettings settings);
    }
}
