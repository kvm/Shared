using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
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

        private ViewModels.MainPageViewModel viewModel;

        public ViewModels.MainPageViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new ViewModels.MainPageViewModel();
                }
                return viewModel;
            }

            set { viewModel = value; }
        }
        

        public MainPage() {
            this.InitializeComponent();
            MediaLogger.OpenLogFileAndLoadTracks(true);
            MediaLogger.OpenLogFileAndLoadTracks(false);

            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.DataContext = this.ViewModel;
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

        private async void WebViewControl_FrameContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            currentUri = await sender.InvokeScriptAsync("eval", new String[] { "document.location.href;" });
            this.viewModel.FetchVideoFormatsForVideo(currentUri);
        }

        private void DownloadHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DownloadHistoryPage));
        }

        private void DownloadVideoButton_Click(object sender, RoutedEventArgs e)
        {
            // todo: download video and store it in library
        }
    }
}
