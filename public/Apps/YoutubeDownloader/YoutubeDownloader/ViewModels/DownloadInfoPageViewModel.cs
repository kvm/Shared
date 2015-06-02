using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common;
using YoutubeExtractor;

namespace YoutubeDownloader.ViewModels
{
    public class DownloadInfoPageViewModel : INotifyPropertyChanged
    {
        private string _videoId;

        public string VideoId
        {
            get { return _videoId; }
            set { _videoId = value; }
        }
        

        private string _backgroundImageUri;

        /// <summary>
        /// Sets the background image of the page
        /// </summary>
        public string BackgroundImageUri
        {
            get
            {
                return _backgroundImageUri;
            }

            set
            {
                _backgroundImageUri = value;
                RaisePropertyChanged("BackgroundImageUri");
            }
        }


        private string _videoTitle;

        public string VideoTitle
        {
            get { return _videoTitle; }
            set
            {
                _videoTitle = value;
                RaisePropertyChanged("VideoTitle");
            }
        }

        private ObservableCollection<VideoInfo> _videoInfos;

        public ObservableCollection<VideoInfo> VideoInfos
        {
            get { return _videoInfos; }
            set
            {
                _videoInfos = value;
                RaisePropertyChanged("VideoInfos");
            }
        }

        private VideoInfo _SelectedVideoInfo;

        public VideoInfo SelectedVideoInfo
        {
            get { return _SelectedVideoInfo; }
            set
            {
                _SelectedVideoInfo = value;
                RaisePropertyChanged("SelectedVideoInfo");
            }
        }

        private int _TitleLength;

        public int TitleLength
        {
            get { return _TitleLength; }
            set
            {
                _TitleLength = value;
                RaisePropertyChanged("TitleLength");
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async void FetchVideoFormatsForVideo(string link)
        {
            var videoList = DownloadUrlResolver.GetDownloadUrls(link).ToList();
            //UpdateVideosWithSizeInfo(videoList);

            this.VideoInfos = new ObservableCollection<VideoInfo>(videoList);

            // update the image and title fields
            this.VideoId = GetVideoIdFromYoutubeUrl(link);

            this.BackgroundImageUri = String.Format(Constants.ImageUriFormat, this.VideoId);

            this.VideoTitle = this.VideoInfos[0].Title;

            this.SelectedVideoInfo = this.VideoInfos[0];

            this.TitleLength = 70;
        }

        /// <summary>
        /// Fetches id from youtube video url 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetVideoIdFromYoutubeUrl(string url)
        {
            Uri uri = new Uri(url);
            var query = uri.Query;

            if(!string.IsNullOrEmpty(query))
            {
                query = query.Replace("?", "");

                var parameters = query.Split(new char[] { '&' });
                foreach (var parameter in parameters)
                {
                    var idValuePair = parameter.Split(new char[] { '=' });
                    if(idValuePair[0] == "v")
                    {
                        return idValuePair[1];
                    }
                }
            }

            return null;
        }
    }
}
