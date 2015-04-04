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
        Dictionary<MediaTrack, MediaItemViewModel> m_mapMediaTrackToView;

        public VideoPivotItemViewModel()
        {
            this.showNoVideoDownloadedText = Visibility.Visible;
            this.groupedVideo = null;
            m_mapMediaTrackToView = new Dictionary<MediaTrack, MediaItemViewModel>();
        }

        public ObservableCollection<MediaItemViewModel> GroupedVideo
        {
            get
            {
                if (null == this.groupedVideo)
                {
                    var songs = new ObservableCollection<MediaItemViewModel>();
                    int countDownloadedVideos = MediaLogger.m_videoTracks.Count;

                    for (int i = 0; i < countDownloadedVideos; i++)
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
                        m_mapMediaTrackToView[track] = vm;
                    }

                    this.groupedVideo = songs;
                    if (countDownloadedVideos == 0)
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

        public void UpdateDownloadProgress(MediaTrack track, int progress) {
            if (m_mapMediaTrackToView != null)
            {
                MediaItemViewModel viewModel = m_mapMediaTrackToView[track];
                if (viewModel != null)
                {
                    if (track.DownldStatus == DownloadStatus.Completed)
                    {
                        viewModel.SubHeaderText = ((double)track.Size/1000000).ToString() + "Mb";
                    }
                    else if (track.DownldStatus == DownloadStatus.Downloading)
                    {
                        // use progress interger to update the progress
                    }
                }
            }
        }
    }
}
