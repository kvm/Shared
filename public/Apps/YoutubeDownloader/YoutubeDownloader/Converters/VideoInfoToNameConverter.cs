using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using YoutubeExtractor;

namespace YoutubeDownloader.Converters
{
    public class VideoInfoToNameConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var videoInfo = value as VideoInfo;

            string title = string.Concat(videoInfo.Title, ".", videoInfo.VideoExtension, ", ", videoInfo.VideoType.ToString());

            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
