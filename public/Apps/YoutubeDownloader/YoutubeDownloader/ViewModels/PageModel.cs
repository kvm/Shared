
namespace YoutubeDownloader.ViewModels
{
    using System.Collections.Generic;

    public abstract class PageModel<TSettings> : PageModelBase
        where TSettings : PageModelSettings, new()
    {
        protected PageModel()
        {
        }

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

        public abstract void Initialize(TSettings settings);
    }
}
