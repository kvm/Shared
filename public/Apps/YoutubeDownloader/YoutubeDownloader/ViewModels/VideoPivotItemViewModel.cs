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
using YoutubeDownloader.Libraries.YoutubeExtractor.MediaLibrary;

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
                    int countVideosDownloaded = MediaLogger.m_videoTracks.Count;

                    for (int i = 0; i < countVideosDownloaded; i++)
                    {
                        var vm = new MediaItemViewModel();
                        MediaTrack track = MediaLogger.m_videoTracks[i];
                        vm.HeaderText = track.Title;
                        if (track.DownldStatus == DownloadStatus.Completed)
                            vm.SubHeaderText = ((double)track.Size / 1000000).ToString() + "Mb";
                        else if (track.DownldStatus == DownloadStatus.Canceled)
                            vm.SubHeaderText = "Canceled";
                        else if (track.DownldStatus == DownloadStatus.Downloading)
                            vm.SubHeaderText = "Downloading...";
                        songs.Add(vm);
                    }

                    this.groupedVideo = songs;
                    if (countVideosDownloaded == 0)
                        this.ShowNoVideoDownloadedText = Visibility.Visible;
                    else
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
