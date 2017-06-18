using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumbrTumblr.HelpersAndExtensionMethods
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is decimal)
                return value.ToString();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal dec;
            if (decimal.TryParse(value as string, out dec))
                return dec;
            return value;
        }
    }

    public class IntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
                return value.ToString();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int dec;
            if (int.TryParse(value as string, out dec))
                return dec;
            return value;
        }
    }

    public class NullableIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = null;
            if (value is int?)
            {
                if (((int?) value).HasValue)
                {
                    result = ((int?) value).Value.ToString();
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int? result = null;

            if (value != null && value.ToString() != string.Empty)
            {
                int number = default(int);
                if (int.TryParse(value as string, out number))
                {
                    result = number;
                }
            }

            return result;
        }
    }

    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
                return value.ToString();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool dec;
            if (bool.TryParse(value as string, out dec))
                return dec;
            return value;
        }
    }

    public class DatetimeToStringConverter : IValueConverter
{
    #region IValueConverter implementation

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null)
            return string.Empty;

        var datetime = (DateTime)value;
        //put your custom formatting here
        return datetime.ToLocalTime().ToString("g");
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException(); 
    }

    #endregion
}





}
