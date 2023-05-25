using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ManagerReport
{
    [ValueConversion(typeof(String), typeof(ImageSource))]
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string valueString))
            {
                return null;
            }
            try
            {
                ImageSource image = BitmapFrame.Create(new Uri(valueString), BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
                return image;
            }
            catch { return null; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
