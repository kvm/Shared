using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YoutubeExtractor;
using YoutubeDownloader.Libraries.YoutubeExtractor.MediaLibrary;

// The WebView Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace YoutubeDownloader {
    public sealed partial class MainPage : Page {
        // TODO: Replace with your URL here.
        private static readonly Uri HomeUri = new Uri("https://m.youtube.com", UriKind.Absolute);
        private string currentUri;
        public MainPage() {
            this.InitializeComponent();
            MediaLogger.OpenLogFileAndLoadTracks(true);
            MediaLogger.OpenLogFileAndLoadTracks(false);

            WebViewControl.NavigationCompleted += webView_NavigationCompleted;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Event to indicate webview has completed the navigation, either with success or failure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args) {
            if (args.IsSuccess) {
                currentUri = (args.Uri != null) ? args.Uri.ToString() : "<null>";
            }
            else {
                string url = "";
                try { 
                    url = args.Uri.ToString();
                }
                finally {
                    currentUri = url;
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            WebViewControl.Navigate(HomeUri);
            HardwareButtons.BackPressed += this.MainPage_BackPressed;
        }

        /// <summary>
        /// Invoked when this page is being navigated away.
        /// </summary>
        /// <param name="e">Event data that describes how this page is navigating.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            HardwareButtons.BackPressed -= this.MainPage_BackPressed;
        }

        /// <summary>
        /// Overrides the back button press to navigate in the WebView's back stack instead of the application's.
        /// </summary>
        private void MainPage_BackPressed(object sender, BackPressedEventArgs e) {
            if (WebViewControl.CanGoBack) {
                WebViewControl.GoBack();
                e.Handled = true;
            }
        }

        private void Browser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args) {
            if (!args.IsSuccess) {
                Debug.WriteLine("Navigation to this page failed, check your internet connection.");
            }
        }

        /// <summary>
        /// Navigates forward in the WebView's history.
        /// </summary>
        private void ForwardAppBarButton_Click(object sender, RoutedEventArgs e) {
            if (WebViewControl.CanGoForward) {
                WebViewControl.GoForward();
            }
        }

        /// <summary>
        /// Navigates to the initial home page.
        /// </summary>
        private void HomeAppBarButton_Click(object sender, RoutedEventArgs e) {
            WebViewControl.Navigate(HomeUri);
        }

        /// <summary>
        /// Navigates to the initial home page.
        /// </summary>
        private void DownloadVideoButton_Click(object sender, RoutedEventArgs e) {
            string link = currentUri;

            /*
             * Get the available video formats.
             * We'll work with them in the video and audio download examples.
             */
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
            /*
             * Select the first .mp4 video with 360p resolution
             */
            VideoInfo video = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption) {
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
}
