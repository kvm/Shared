using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeDownloader.Common;

namespace YoutubeDownloader.ViewModels
{
    public class MediaItemViewModel : BindableBase
    {
        private string headerText;
        private string subheaderText;

        public string HeaderText
        {
            get { return headerText; }
            set { this.SetProperty(ref this.headerText, value); }
        }

        public string SubHeaderText
        {
            get { return subheaderText; }
            set { this.SetProperty(ref this.subheaderText, value); }
        }
    }
}
