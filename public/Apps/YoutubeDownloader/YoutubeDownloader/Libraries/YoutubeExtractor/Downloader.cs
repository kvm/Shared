using System;
using YoutubeDownloader.Libraries.YoutubeExtractor.MediaLibrary;

namespace YoutubeExtractor
{
    /// <summary>
    /// Provides the base class for the <see cref="AudioDownloader"/> and <see cref="VideoDownloader"/> class.
    /// </summary>
    public abstract class Downloader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Downloader"/> class.
        /// </summary>
        /// <param name="video">The video to download/convert.</param>
        /// <param name="filename">The path to save the video/audio.</param>
        /// /// <param name="bytesToDownload">An optional value to limit the number of bytes to download.</param>
        /// <exception cref="ArgumentNullException"><paramref name="video"/> or <paramref name="filename"/> is <c>null</c>.</exception>
        protected Downloader(VideoInfo video, string filename, int? bytesToDownload = null)
        {
            if (video == null)
                throw new ArgumentNullException("video");

            if (filename == null)
                throw new ArgumentNullException("savePath");

            this.Video = video;
            this.SavePath = filename;
            this.BytesToDownload = bytesToDownload;
            this.Track = new MediaTrack(video.DownloadUrl, filename, MediaItemType.Video);
        }

        public event EventHandler DownloadFinished;
        public event EventHandler DownloadStarted;

        public int? BytesToDownload { get; private set; }
        public string SavePath { get; private set; }

        /// <summary>
        /// Gets the video to download/convert.
        /// </summary>
        public VideoInfo Video { get; private set; }

        public MediaTrack Track { get; private set; }

        /// <summary>
        /// Starts the work of the <see cref="Downloader"/>.
        /// </summary>
        public abstract void Execute();

        protected void OnDownloadFinished(EventArgs e, bool fcancel, int bytesDownloaded)
        {
            if (this.DownloadFinished != null)
            {
                this.DownloadFinished(this, e);
            }
            MediaLogger.OnMediaDownloadCompleted(this.Track, fcancel, bytesDownloaded);
        }

        protected void OnDownloadStarted(EventArgs e)
        {
            if (this.DownloadStarted != null)
            {
                this.DownloadStarted(this, e);
            }
            MediaLogger.OnMediaDownloadStarted(this.Track);
        }
    }
}