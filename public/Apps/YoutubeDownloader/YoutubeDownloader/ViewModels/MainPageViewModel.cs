using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common;
using YoutubeExtractor;

namespace YoutubeDownloader.ViewModels {
    public class MainPageViewModel: BindableBase {

        private ObservableCollection<VideoInfo> videoInfos;

        public ObservableCollection<VideoInfo> VideoInfos
        {
            get { return videoInfos; }
            set { videoInfos = value; }
        }

        private bool isVideoDownloadable;

        public bool IsVideoDownloadable
        {
            get { return isVideoDownloadable; }
            set { isVideoDownloadable = value; }
        }
        

        public void FetchVideoFormatsForVideo(string link)
        {
            this.VideoInfos = new ObservableCollection<VideoInfo>(DownloadUrlResolver.GetDownloadUrls(link).ToList());

            if(this.VideoInfos.Count > 0)
            {
                IsVideoDownloadable = true;
            }
        }
        

        public bool CheckIfUrlIsValid(string url)
        {
            Uri uri = new Uri(url);
            string leftPart, rightPart;
            string[] tokens = url.Split(new char[] { '?' });

            if(tokens.Count == 2)
            {
                leftPart = tokens[0];
                rightPart = tokens[1];

                if(leftPart.EndsWith("watch"))
                {
                    //uri.AbsolutePath
                }
            }

            return false;
        }
    }
}
