using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;

namespace Trasys.PhotoTrack.Mobile.Dev.Apps.Tools.Converter
{
    public class CoordonateFormatedToGeoLocalizationConverter : IValueConverter
    {
#if WINDOWS_APP

        /// <summary>
        /// Convert the specified value to a Visibility value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is string)
            {
                string[] splitted = ((string)value).Split(new string[]{" - "}, StringSplitOptions.RemoveEmptyEntries);
                
                BasicGeoposition myLocation = new BasicGeoposition
                {
                    Longitude = System.Convert.ToDouble(splitted[0]),
                    Latitude = System.Convert.ToDouble(splitted[1])
                };
                Geopoint point = new Geopoint(myLocation);
                return point;
            }
            return null;
        }

        /// <summary>
        /// Convert the Visibility value to an original inverse value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if(value is Geopoint)
            {
                Geopoint point = value as Geopoint;
                string.Format("{0} - {1}", point.Position.Longitude, point.Position.Latitude);
            }
            return string.Empty;
        }

#else

        /// <summary>
        /// Convert the specified value to a Visibility value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Convert the Visibility value to an original inverse value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }

#endif
    }
}
