using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YoutubeDownloader.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace YoutubeDownloader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DownloadInfoPage : Page
    {
        private DownloadInfoPageViewModel _viewModel;

        public DownloadInfoPageViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }
        

        public DownloadInfoPage()
        {
            this.InitializeComponent();

            // create a new viewmodel
            ViewModel = new DownloadInfoPageViewModel();

            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string youtubeUrl = e.Parameter as string;

            //youtubeUrl = "https://www.youtube.com/watch?v=-ncIVUXZla8&list=TLRygxjwthh80";
            // now fetch all the data for this video
            this.ViewModel.FetchVideoFormatsForVideo(youtubeUrl);
        }
    }
}
