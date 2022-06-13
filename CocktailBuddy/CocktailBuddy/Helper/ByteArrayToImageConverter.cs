using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace CocktailBuddy.Helper
{
    public class ByteArrayToImageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource image = ImageSource.FromStream(() => new MemoryStream((byte[])value));
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
