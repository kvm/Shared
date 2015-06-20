using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        public void DownloadMp3()
        {
            /*
            * We want the first extractable video with the highest audio quality.
            */
            VideoInfo video = this.VideoInfos
                .Where(info => info.CanExtractAudio)
                .OrderByDescending(info => info.AudioBitrate)
                .First();

            /*
            * If the video has a decrypted signature, decipher it
            */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
            * Create the audio downloader.
            * The first argument is the video where the audio should be extracted from.
            * The second argument is the path to save the audio file.
            */
            var audioDownloader = new AudioDownloader(video, Path.Combine(video.Title + video.AudioExtension));

            // Register the progress events. We treat the download progress as 85% of the progress and the extraction progress only as 15% of the progress,
            // because the download will take much longer than the audio extraction.
            //audioDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage * 0.85);
            //audioDownloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);

            /*
             * Execute the audio downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            audioDownloader.Execute();
        }

        public void DownloadVideo()
        {
            VideoInfo video = this.SelectedVideoInfo;

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
             * Create the video downloader.
             * The first argument is the video to download.
             * The second argument is the path to save the video file.
             */
            var videoDownloader = new VideoDownloader(video, Path.Combine(video.Title + video.VideoExtension));

            // TODO: anichopr
            //// Register the ProgressChanged event and print the current progress
            //videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

            /*
             * Execute the video downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            videoDownloader.ExecuteAsync(false);
        }
    }
}
