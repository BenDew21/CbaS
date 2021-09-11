using System.Windows.Controls;
using CBaSCore.Framework.UI.Handlers;

namespace CBaSCore.Framework.UI.Controls
{
    public class CustomMenuBar : StackPanel
    {
        private readonly MenuBarHandler _handler;

        public CustomMenuBar()
        {
            Orientation = Orientation.Horizontal;
            _handler = MenuBarHandler.GetInstance();
            _handler.SetMenuBar(this);
            _handler.RenderMenuBar();
        }
    }
}