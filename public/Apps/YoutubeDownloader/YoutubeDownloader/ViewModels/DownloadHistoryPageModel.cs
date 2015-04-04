using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using YoutubeDownloader.Libraries.YoutubeExtractor.MediaLibrary;

namespace YoutubeDownloader.ViewModels
{
    public class DownloadHistoryPageModel : PageModel<PageModelSettings.None>
    {
        private static VideoPivotItemViewModel videopivotItemViewModel;
        private static AudioPivotItemViewModel audiopivotItemViewModel;

        public DownloadHistoryPageModel()
        {
            videopivotItemViewModel = new VideoPivotItemViewModel();
            audiopivotItemViewModel = new AudioPivotItemViewModel();
        }

        public VideoPivotItemViewModel VideoPivotViewModel
        {
            get { return videopivotItemViewModel; }
            set { this.SetProperty(ref videopivotItemViewModel, value); }
        }

        public AudioPivotItemViewModel AudioPivotViewModel
        {
            get { return audiopivotItemViewModel; }
            set { this.SetProperty(ref audiopivotItemViewModel, value); }
        }

        public override void Initialize(PageModelSettings.None settings)
        {
            this.VideoPivotViewModel.Initialize();
        }

        public static void UpdateMediaDownloadProgress(MediaTrack track, int progress)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (track.MediaType == MediaItemType.Video && videopivotItemViewModel != null)
                {
                    videopivotItemViewModel.UpdateDownloadProgress(track, progress);
                }
                else if (track.MediaType == MediaItemType.Audio && audiopivotItemViewModel != null)
                {
                    //
                }
            });
        }
    }
}
