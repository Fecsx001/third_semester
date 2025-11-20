using System;
using System.Globalization;
using System.Windows.Data;

namespace AsteroidGame.View
{
    public class HeightConverter : IValueConverter
    {
        public double Offset { get; set; } = 60.0; 

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int height)
            {
                return height + Offset;
            }
            if (value is double dHeight)
            {
                return dHeight + Offset;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}