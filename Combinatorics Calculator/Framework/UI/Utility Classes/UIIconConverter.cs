using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Combinatorics_Calculator.Framework.UI.Utility_Classes
{
    public static class UIIconConverter
    {
        public static BitmapSource BitmapToBitmapImage(Bitmap bitmap)
        {
            var source = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return source;
        }

        public static BitmapSource RotateImage(BitmapSource image, double angle)
        {
            var transformBitmap = (TransformedBitmap)image;
            RotateTransform rotateTransform = (RotateTransform)(transformBitmap.Transform);
            rotateTransform.Angle = angle;
            return image;
        }
    }
}