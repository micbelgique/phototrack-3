using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_APP
using global::Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using System.Globalization;
#endif

namespace Trasys.Dev.Tools.Converter
{
    /// <summary>
    /// Converts DateTime values to a string representation.
    /// With a # prefix before format string, the date will be always formatted with this string parameter(ex. #dd/MM/yyyy => 25/07/2013).
    /// Without a # prefix, the date will be "Yesterday", "Today", "Tomorrow", or the formatted date with this string parameter.
    /// </summary>
    public sealed class DateTimeConverter : IValueConverter
    {
        private const string _today = "today";
        private const string _tomorrow = "tomorrow";
        private const string _yesterday = "yesterday";

#if WINDOWS_APP

        /// <summary>
        /// Converts a given DateTime to a string representation.
        /// Today if the DateTime value is today.
        /// The formatted DateTime using the provided parameter.
        /// Null if the DateTime value is not valid.
        /// Using the # sign as first character of the parameter 
        /// string will force the converter to convert to the provided format
        /// , the today rule will not apply, usefull for diplaying only Time.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime? date = value as DateTime?;
            String format = parameter as String;
            if(format.Contains('_'))
            {
                format = format.Replace('_', ' ');
            }
            if (date != null && date.HasValue)
            {
                if (!String.IsNullOrEmpty(format))
                {
                    if (format.StartsWith("#"))
                    {
                        format = format.Substring(1);
                        return date.Value.ToString(format);
                    }
                    else if (date.Value.Date == DateTime.Today)
                    {
                        return _today;
                    }
                    else if (date.Value.Date == DateTime.Today.AddDays(+1))
                    {
                        return _tomorrow;
                    }
                    else if (date.Value.Date == DateTime.Today.AddDays(-1))
                    {
                        return _yesterday;
                    }
                    else
                    {
                        return date.Value.ToString(format);
                    }
                }
                else
                {
                    return date.Value.ToString();
                }
            }            

            return null;
        }

        /// <summary>
        /// Converts back the value to a DateTime object, if the binding is in the two way mode.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            String date = value as String;
            
            if (!String.IsNullOrEmpty(date))
            {
                if (String.Compare(date, _today, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return DateTime.Today;
                }
                else if (String.Compare(date, _yesterday, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return DateTime.Today.AddDays(-1);
                }
                else if (String.Compare(date, _tomorrow, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return DateTime.Today.AddDays(+1);
                }
                else
                {
                    DateTime newDate;
                    if (DateTime.TryParse(date, out newDate))
                    {
                        return newDate;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

#else
        /// <summary>
        /// Converts a given DateTime to a string representation.
        /// Today if the DateTime value is today.
        /// The formatted DateTime using the provided parameter.
        /// Null if the DateTime value is not valid.
        /// Using the # sign as first character of the parameter 
        /// string will force the converter to convert to the provided format
        /// , the today rule will not apply, usefull for diplaying only Time.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? date = value as DateTime?;
            String format = parameter as String;

            if (date != null && date.HasValue)
            {
                if (!String.IsNullOrEmpty(format))
                {
                    if (format.StartsWith("#"))
                    {
                        format = format.Substring(1);
                        return date.Value.ToString(format);
                    }
                    else if (date.Value.Date == DateTime.Today)
                    {
                        return _today;
                    }
                    else if (date.Value.Date == DateTime.Today.AddDays(+1))
                    {
                        return _tomorrow;
                    }
                    else if (date.Value.Date == DateTime.Today.AddDays(-1))
                    {
                        return _yesterday;
                    }
                    else
                    {
                        return date.Value.ToString(format);
                    }
                }
                else
                {
                    return date.Value.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Converts back the value to a DateTime object, if the binding is in the two way mode.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String date = value as String;

            if (!String.IsNullOrEmpty(date))
            {
                if (String.Compare(date, _today, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return DateTime.Today;
                }
                else if (String.Compare(date, _yesterday, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return DateTime.Today.AddDays(-1);
                }
                else if (String.Compare(date, _tomorrow, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return DateTime.Today.AddDays(+1);
                }
                else
                {
                    DateTime newDate;
                    if (DateTime.TryParse(date, out newDate))
                    {
                        return newDate;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }
#endif
    }
}
