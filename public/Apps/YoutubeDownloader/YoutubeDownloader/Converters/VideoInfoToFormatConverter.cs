using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using YoutubeExtractor;

namespace YoutubeDownloader.Converters
{
    public class VideoInfoToFormatConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var videoInfo = value as VideoInfo;

            string title = string.Concat(videoInfo.Resolution, "p ", videoInfo.VideoType.ToString());

            // adding file size in mb
            if(videoInfo.VideoSizeInMb > 0)
            {
                title += string.Concat(",  ", videoInfo.VideoSizeInMb.ToString("N3"), "Mb");
            }

            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
