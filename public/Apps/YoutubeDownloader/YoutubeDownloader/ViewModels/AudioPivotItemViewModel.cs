using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common;
using Windows.UI.Xaml;
using YoutubeDownloader.Libraries.YoutubeExtractor.MediaLibrary;

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
                    int countAudiosDownloaded = MediaLogger.m_audioTracks.Count;

                    for (int i = 0; i < countAudiosDownloaded; i++)
                    {
                        MediaItemViewModel mediaItemViewModel = new MediaItemViewModel();
                        MediaTrack track = MediaLogger.m_audioTracks[i];
                        mediaItemViewModel.HeaderText = track.Title;
                        if (track.DownldStatus == DownloadStatus.Completed)
                            mediaItemViewModel.SubHeaderText = ((double)track.Size / 1000000).ToString() + "Mb";
                        else if (track.DownldStatus == DownloadStatus.Canceled)
                            mediaItemViewModel.SubHeaderText = "Canceled";
                        else if (track.DownldStatus == DownloadStatus.Downloading)
                            mediaItemViewModel.SubHeaderText = "Downloading...";

                        songs.Add(mediaItemViewModel);
                    }

                    this.groupedAudio = songs;
                    if (countAudiosDownloaded == 0)
                        this.ShowNoAudioDownloadedText = Visibility.Visible;
                    else
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
