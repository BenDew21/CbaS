using System.Drawing;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace CBaSCore.Framework.UI.Utility_Classes
{
    /// <summary>
    /// LayoutAnchorableFactory - Factory for creating LayoutAnchorable controls
    /// </summary>
    public static class LayoutAnchorableFactory
    {
        /// <summary>
        /// Create a new layout panel from the provided details
        /// </summary>
        /// <param name="details">The details to use</param>
        /// <returns>The created layout panel</returns>
        public static LayoutAnchorable CreateLayout(LayoutAnchorableDetails details)
        {
            var layout = new LayoutAnchorable();
            layout.Title = details.Title;
            layout.IconSource = UiIconConverter.BitmapToBitmapImage(details.Icon);
            layout.CanHide = details.CanHide;
            layout.CanClose = details.CanClose;
            layout.Content = details.Content;

            return layout;
        }
    }

    /// <summary>
    /// LayoutAnchorableDetails - DTO for LayoutAnchorables
    /// </summary>
    public class LayoutAnchorableDetails
    {
        public string Title;
        public Bitmap Icon;
        public bool CanHide;
        public bool CanClose;
        public Control Content;
    }
}