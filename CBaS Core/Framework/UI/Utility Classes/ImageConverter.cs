using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CBaSCore.Framework.UI.Utility_Classes
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Bitmap)
            {
                var stream = new MemoryStream();
                ((Bitmap) value).Save(stream, ImageFormat.Png);

                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();

                return image;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}