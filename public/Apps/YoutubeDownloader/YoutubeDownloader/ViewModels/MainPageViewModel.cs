﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common;
using YoutubeExtractor;
using Windows.UI.Xaml.Controls;
using System.IO;

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

            //var videoList = GetRidOfDuplicateVideoFormats(DownloadUrlResolver.GetDownloadUrls(link).ToList());
            var videoList = DownloadUrlResolver.GetDownloadUrls(link).ToList();
            UpdateVideosWithSizeInfo(videoList);

            this.VideoInfos = new ObservableCollection<VideoInfo>(videoList);

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
                if (!resolutionToVideoDict.ContainsKey(video.Resolution))
                {
                    resolutionToVideoDict.Add(video.Resolution, video);
                }
                else
                {
                    if (resolutionToVideoDict[video.Resolution].CompareTo(video) != 1)
                    {
                        resolutionToVideoDict[video.Resolution] = video;
                    }
                }
            }

            // adding each format in the required order
            if (resolutionToVideoDict.ContainsKey(1080))
            {
                filteredVideos.Add(resolutionToVideoDict[1080]);
            }

            if (resolutionToVideoDict.ContainsKey(720))
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

        public void OnItemPicked(ItemsPickedEventArgs args)
        {
            if (args.AddedItems != null && args.AddedItems.Count != 0)
            {
                var video = args.AddedItems[0] as VideoInfo;

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
                videoDownloader.Execute();
            }
        }

        public void OnMusicDownloadTapped()
        {
            /*
            * We want the first extractable video with the highest audio quality.
            */
            VideoInfo video = videoInfos
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

        private void UpdateVideosWithSizeInfo(List<VideoInfo> videoInfos)
        {
            foreach (var videoInfo in videoInfos)
            {
                videoInfo.VideoSizeInMb = DownloadUrlResolver.GetVideoSizeInMb(videoInfo.DownloadUrl);
            }
        }

        public void CheckIfPageIsVideoPage(string pageUrl)
        {
            if (pageUrl.Contains("youtube.com/watch?v="))
            {
                this.IsVideoDownloadable = true;
            }
            else
            {
                this.IsVideoDownloadable = false;
            }
        }
    }
}
