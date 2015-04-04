using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common;
using Windows.UI.Xaml;

namespace YoutubeDownloader.ViewModels
{
    public class VideoPivotItemViewModel : ViewModel
    {
        private ObservableCollection<MediaItemViewModel> groupedVideo;
        private Visibility showNoVideoDownloadedText;

        public VideoPivotItemViewModel()
        {
            this.showNoVideoDownloadedText = Visibility.Visible;
            this.groupedVideo = null;
        }

        public ObservableCollection<MediaItemViewModel> GroupedVideo
        {
            get
            {
                if (null == this.groupedVideo)
                {
                    var songs = new ObservableCollection<MediaItemViewModel>();

                    for (uint i = 0; i < 10; i++)
                    {
                        var vm = new MediaItemViewModel();
                        vm.HeaderText = "Tu hi mera mera";
                        vm.SubHeaderText = "24.16Mb";
                        songs.Add(vm);
                    }

                    this.groupedVideo = songs;
                    this.ShowNoVideoDownloadedText = Visibility.Collapsed;
                }

                return this.groupedVideo;
            }
        }

        public void Initialize()
        {
        }

        public Visibility ShowNoVideoDownloadedText
        {
            get { return this.showNoVideoDownloadedText; }
            private set { this.SetProperty(ref this.showNoVideoDownloadedText, value); }
        }

    }
}
