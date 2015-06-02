using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace YoutubeDownloader.Converters
{
    public class TextTrimmerConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value == null)
            {
                return string.Empty;
            }

            string inputText = (string)value;

            int trimLength = System.Convert.ToInt32(parameter);

            if (inputText.Length < trimLength)
            {
                return inputText;
            }
            else
            {
                return inputText.Substring(0, trimLength) + "...";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
