
namespace YoutubeDownloader.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using YoutubeDownloader.Common;

    public abstract class PageModelBase : BindableBase
    {
        protected PageModelBase()
        {
        }

        public abstract void Initialize(IDictionary<string, string> savedValues);

        public virtual void OnBackKeyPress(CancelEventArgs e)
        {
        }
    }
}
