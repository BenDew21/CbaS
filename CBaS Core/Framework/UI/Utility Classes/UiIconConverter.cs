using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CBaSCore.Framework.UI.Utility_Classes
{
    /// <summary>
    /// UiIconConverter - Utility class for converting image types
    /// </summary>
    public static class UiIconConverter
    {
        /// <summary>
        /// Create a BitmapSource (extends ImageSource) from a Bitmap
        /// </summary>
        /// <param name="bitmap">The Bitmap to convert</param>
        /// <returns>The BitmapSource</returns>
        public static BitmapSource BitmapToBitmapImage(Bitmap bitmap)
        {
            var source = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return source;
        }

        /// <summary>
        /// Rotates a given image by a specified angle
        /// </summary>
        /// <param name="image">The BitmapSource to rotate</param>
        /// <param name="angle">The angle to rotate by clockwise in degrees</param>
        /// <returns>The rotated image</returns>
        public static BitmapSource RotateImage(BitmapSource image, double angle)
        {
            var transformBitmap = (TransformedBitmap)image;
            var rotateTransform = (RotateTransform)(transformBitmap.Transform);
            rotateTransform.Angle = angle;
            return image;
        }
    }
}