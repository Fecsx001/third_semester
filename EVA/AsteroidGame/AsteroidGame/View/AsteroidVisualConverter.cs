using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace AsteroidGame.View
{
    public class AsteroidFillConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var widthProperty = value?.GetType().GetProperty("Width");
            var heightProperty = value?.GetType().GetProperty("Height");
            
            if (widthProperty != null && heightProperty != null)
            {
                int width = (int)(widthProperty.GetValue(value) ?? 0);
                int height = (int)(heightProperty.GetValue(value) ?? 0);
                int size = (width + height) / 2;
                
                if (size >= 60) return new SolidColorBrush(Color.FromRgb(80, 60, 40)); // Giant
                if (size >= 40) return new SolidColorBrush(Color.FromRgb(100, 80, 60)); // Large
                if (size >= 25) return Brushes.Gray; // Medium
                return Brushes.LightGray; // Small
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AsteroidStrokeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var widthProperty = value?.GetType().GetProperty("Width");
            var heightProperty = value?.GetType().GetProperty("Height");
            
            if (widthProperty != null && heightProperty != null)
            {
                int width = (int)(widthProperty.GetValue(value) ?? 0);
                int height = (int)(heightProperty.GetValue(value) ?? 0);
                int size = (width + height) / 2;
                
                if (size >= 60) return Brushes.DarkRed; // Giant
                if (size >= 40) return Brushes.DarkOrange; // Large
                if (size >= 25) return Brushes.White; // Medium
                return Brushes.Yellow; // Small
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CraterSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double size && parameter is string paramString && double.TryParse(paramString, NumberStyles.Any, CultureInfo.InvariantCulture, out double factor))
            {
                return size * factor;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}