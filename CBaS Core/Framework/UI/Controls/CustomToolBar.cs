using CBaSCore.Framework.UI.Handlers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CBaSCore.Framework.UI.Controls
{
    public class CustomToolBar : StackPanel
    {
        private MenuBarHandler _handler;

        public CustomToolBar()
        {
            Orientation = Orientation.Horizontal;
            Background = new SolidColorBrush(Color.FromRgb(241, 241, 241));
            Margin = new Thickness(0, 0, 0, 10);
            Height = 60;
            _handler = MenuBarHandler.GetInstance();
            _handler.SetToolbar(this);
            _handler.RenderToolBar(0);
        }
    }
}