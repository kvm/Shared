using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common;
using YoutubeExtractor;

namespace YoutubeDownloader.ViewModels
{
    public class MainPageViewModel : PageModel<PageModelSettings.None>
    {

        private ObservableCollection<VideoInfo> videoInfos;

        public ObservableCollection<VideoInfo> VideoInfos
        {
            get { return videoInfos; }
            set { this.SetProperty(ref videoInfos, value); }
        }

        private bool isVideoDownloadable;

        public bool IsVideoDownloadable
        {
            get { return isVideoDownloadable; }
            set { this.SetProperty(ref isVideoDownloadable, value); }
        }

        /// <summary>
        /// Fetch video format for the download url
        /// </summary>
        /// <param name="link"></param>
        public void FetchVideoFormatsForVideo(string link)
        {
            if (!CheckIfUrlIsValid(link))
            {
                return;
            }

            this.VideoInfos = new ObservableCollection<VideoInfo>(GetRidOfDuplicateVideoFormats(DownloadUrlResolver.GetDownloadUrls(link).ToList()));

            if (this.VideoInfos.Count > 0)
            {
                IsVideoDownloadable = true;
            }
        }

        /// <summary>
        /// CHeck if url is of a video or some other page
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool CheckIfUrlIsValid(string url)
        {
            Uri uri = new Uri(url);

            if (uri.AbsolutePath.Equals("/watch") && uri.Query.Contains("v="))
            {
                return true;
            }

            return false;
        }

        public override void Initialize(PageModelSettings.None settings)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// This function picks one video per resolution i.e one per 1080p, 720p etc.
        /// </summary>
        /// <param name="currentVideos"></param>
        /// <returns></returns>
        private List<VideoInfo> GetRidOfDuplicateVideoFormats(List<VideoInfo> currentVideos)
        {
            var filteredVideos = new List<VideoInfo>();

            var resolutionToVideoDict = new Dictionary<int, VideoInfo>();
            foreach (var video in currentVideos)
            {
                if(!resolutionToVideoDict.ContainsKey(video.Resolution))
                {
                    resolutionToVideoDict.Add(video.Resolution, video);
                }
                else
                {
                    if(resolutionToVideoDict[video.Resolution].CompareTo(video) != 1)
                    {
                        resolutionToVideoDict[video.Resolution] = video;
                    }
                }
            }

            // adding each format in the required order
            if(resolutionToVideoDict.ContainsKey(1080))
            {
                filteredVideos.Add(resolutionToVideoDict[1080]);
            }

            if(resolutionToVideoDict.ContainsKey(720))
            {
                filteredVideos.Add(resolutionToVideoDict[720]);
            }

            if (resolutionToVideoDict.ContainsKey(360))
            {
                filteredVideos.Add(resolutionToVideoDict[360]);
            }

            if (resolutionToVideoDict.ContainsKey(240))
            {
                filteredVideos.Add(resolutionToVideoDict[240]);
            }

            if (resolutionToVideoDict.ContainsKey(144))
            {
                filteredVideos.Add(resolutionToVideoDict[144]);
            }

            return filteredVideos;
        }
    }
}
