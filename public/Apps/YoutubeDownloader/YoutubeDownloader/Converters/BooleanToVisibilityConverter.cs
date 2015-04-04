﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace YoutubeDownloader.Converters
{
    public class BooleanToVisibilityConverter: IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = (Visibility)value;

            if(value.Equals(Visibility.Collapsed))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
