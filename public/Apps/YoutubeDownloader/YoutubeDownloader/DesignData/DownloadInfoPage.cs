using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common;
using YoutubeExtractor;

namespace YoutubeDownloader.DesignData
{
    public class DownloadInfoPage
    {
        public string VideoId { get; set; }

        public string BackgroundImageUri { get; set; }

        public string VideoTitle { get; set; }

        public List<VideoInfo> VideoInfos { get; set; }

        public VideoInfo SelectedVideoInfo { get; set; }

        public DownloadInfoPage()
        {
            BackgroundImageUri = string.Format(Constants.ImageUriFormat, "-ncIVUXZla8");
            VideoTitle = "Avicii - Waiting For Love (Lyric Video)";
            VideoInfos = new List<VideoInfo>
            {
                new VideoInfo(133)
                {
                    DownloadUrl = "360p"
                },
                new VideoInfo(133)
                {
                    DownloadUrl = "360p"
                },
                new VideoInfo(133)
                {
                    DownloadUrl = "360p"
                }
            };

            SelectedVideoInfo = new VideoInfo(133)
            {
                DownloadUrl = "360p"
            };
        }
    }
}
