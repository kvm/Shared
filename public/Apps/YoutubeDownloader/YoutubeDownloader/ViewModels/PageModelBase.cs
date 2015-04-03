// <copyright file="PageModelBase.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace YoutubeDownloader.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using YoutubeDownloader.Common;

    /// <summary>
    /// Base type for top-level view model ('page model') classes that use page model settings to encapsulate saved state.
    /// </summary>
    /// <seealso cref="PageModelSettings"/>
    public abstract class PageModelBase : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelBase" /> class.
        /// </summary>
        protected PageModelBase()
        {
        }

        /// <summary>
        /// Overridden in derived types to initialize this <see cref="PageModelBase"/> from the previously serialized string-encoded <see cref="PageModelSettings"/> values in <paramref name="savedValues"/>.
        /// </summary>
        /// <param name="savedValues">A dictionary of string-encoded <see cref="PageModelSettings"/> values previously serialized by the correct settings type for this page model.</param>
        public abstract void Initialize(IDictionary<string, string> savedValues);

        /// <summary>
        /// Decide what to do if the back key is pressed. Allows page to override it.
        /// </summary>
        /// <param name="e">The cancel event args</param>
        public virtual void OnBackKeyPress(CancelEventArgs e)
        {
        }
    }
}
