using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace YoutubeExtractor {
    /// <summary>
    /// Provides a method to download a video from YouTube.
    /// </summary>
    public class VideoDownloader : Downloader {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoDownloader"/> class.
        /// </summary>
        /// <param name="video">The video to download.</param>
        /// <param name="savePath">The path to save the video.</param>
        /// <param name="bytesToDownload">An optional value to limit the number of bytes to download.</param>
        /// <exception cref="ArgumentNullException"><paramref name="video"/> or <paramref name="savePath"/> is <c>null</c>.</exception>
        public VideoDownloader(VideoInfo video, string savePath, int? bytesToDownload = null)
            : base(video, savePath, bytesToDownload)
        { }

        /// <summary>
        /// Occurs when the downlaod progress of the video file has changed.
        /// </summary>
        public event EventHandler<ProgressEventArgs> DownloadProgressChanged;

        public override void Execute()
        {

        }

        /// <summary>
        /// Starts the video download.
        /// </summary>
        /// <exception cref="IOException">The video file could not be saved.</exception>
        /// <exception cref="WebException">An error occured while downloading the video.</exception>
        public async Task ExecuteAsync(bool isForAudio) {
            this.OnDownloadStarted(EventArgs.Empty);
            var request = (HttpWebRequest)WebRequest.Create(this.Video.DownloadUrl);

            // TODO anichopr
            //if (this.BytesToDownload.HasValue) {
            //    request.AddRange(0, this.BytesToDownload.Value - 1);
            //}
            //request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);

            var response = await request.GetResponseAsync();

            Stream source = response.GetResponseStream();
            Stream target = await CreateFile(this.SavePath, isForAudio ? ApplicationData.Current.LocalFolder : KnownFolders.VideosLibrary);

            var buffer = new byte[10059776];
            bool cancel = false;
            int bytes;
            int copiedBytes = 0;

            while (!cancel && (bytes = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                target.Write(buffer, 0, bytes);
                copiedBytes += bytes;
                var eventArgs = new ProgressEventArgs((copiedBytes * 1.0 / response.ContentLength) * 100);
                if (this.DownloadProgressChanged != null)
                {
                    this.DownloadProgressChanged(this, eventArgs);
                    if (eventArgs.Cancel)
                        cancel = true;
                }
            }

            target.Dispose();
            this.OnDownloadFinished(EventArgs.Empty, cancel, copiedBytes);
        }

        private async void ReadWebRequestCallback(IAsyncResult callbackResult) {
            WebRequest request = (WebRequest)callbackResult.AsyncState;
            WebResponse response = request.EndGetResponse(callbackResult);
            Stream source = response.GetResponseStream();
            Stream target = await CreateFile(this.SavePath, KnownFolders.VideosLibrary);

            var buffer = new byte[10059776];
            bool cancel = false;
            int bytes;
            int copiedBytes = 0;

            while (!cancel && (bytes = source.Read(buffer, 0, buffer.Length)) > 0) {
                target.Write(buffer, 0, bytes);
                copiedBytes += bytes;
                var eventArgs = new ProgressEventArgs((copiedBytes * 1.0 / response.ContentLength) * 100);
                if (this.DownloadProgressChanged != null) {
                    this.DownloadProgressChanged(this, eventArgs);
                    if (eventArgs.Cancel)
                        cancel = true;
                }
            }

            this.OnDownloadFinished(EventArgs.Empty, cancel, copiedBytes);
        }

        async Task<Stream> CreateFile(string filename, StorageFolder folder) {
            using (ManualResetEvent completedEvent = new ManualResetEvent(false)) {
                Stream fileStream = null;
                //IAsyncOperation<StorageFile> asyncOp = ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);
                //asyncOp.Completed = new AsyncOperationCompletedHandler<StorageFile>(async (IAsyncOperation<StorageFile> operation, AsyncStatus status) => {
                //    if (asyncOp.Status == AsyncStatus.Completed) {
                //        StorageFile storageFile = asyncOp.GetResults();
                //        fileStream = await storageFile.OpenStreamForWriteAsync();
                //        completedEvent.Set();
                //    }
                //});
                StorageFile storageFile = await folder.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);
                fileStream = await storageFile.OpenStreamForWriteAsync();
                completedEvent.Set();
                completedEvent.WaitOne();
                return fileStream;
            }
        }

    } // class

} // namespace