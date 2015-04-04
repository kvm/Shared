using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common;
using Windows.UI.Xaml;

namespace YoutubeDownloader.ViewModels
{
    public class AudioPivotItemViewModel : ViewModel
    {
        private ObservableCollection<MediaItemViewModel> groupedAudio;
        private Visibility showNoAudioDownloadedText;

        public AudioPivotItemViewModel()
        {
            this.showNoAudioDownloadedText = Visibility.Visible;
            this.groupedAudio = null;
        }

        public ObservableCollection<MediaItemViewModel> GroupedAudio
        {
            get
            {
                if (null == this.groupedAudio)
                {
                    var songs = new ObservableCollection<MediaItemViewModel>();

                    for (uint i = 0; i < 10; i++)
                    {
                        var vm = new MediaItemViewModel();
                        vm.HeaderText = "Tu hi mera mera";
                        vm.SubHeaderText = "24.16Mb";
                        songs.Add(vm);
                    }

                    this.groupedAudio = songs;
                    this.ShowNoAudioDownloadedText = Visibility.Collapsed;
                }

                return this.groupedAudio;
            }
        }

        public void Initialize()
        {
        }

        public Visibility ShowNoAudioDownloadedText
        {
            get { return this.showNoAudioDownloadedText; }
            private set { this.SetProperty(ref this.showNoAudioDownloadedText, value); }
        }

    }
}
