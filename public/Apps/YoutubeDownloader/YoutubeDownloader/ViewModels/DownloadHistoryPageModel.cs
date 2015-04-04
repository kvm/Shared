using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader.ViewModels
{
    public class DownloadHistoryPageModel : PageModel<PageModelSettings.None>
    {
        private VideoPivotItemViewModel videopivotItemViewModel;
        private AudioPivotItemViewModel audiopivotItemViewModel;

        public DownloadHistoryPageModel()
        {
            this.videopivotItemViewModel = new VideoPivotItemViewModel();
            this.audiopivotItemViewModel = new AudioPivotItemViewModel();
        }

        public VideoPivotItemViewModel VideoPivotViewModel
        {
            get { return this.videopivotItemViewModel; }
            set { this.SetProperty(ref this.videopivotItemViewModel, value); }
        }

        public AudioPivotItemViewModel AudioPivotViewModel
        {
            get { return this.audiopivotItemViewModel; }
            set { this.SetProperty(ref this.audiopivotItemViewModel, value); }
        }

        public override void Initialize(PageModelSettings.None settings)
        {
            this.VideoPivotViewModel.Initialize();
        }
    }
}
