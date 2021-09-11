using Xceed.Wpf.AvalonDock.Layout;

namespace CBaSCore.Framework.UI.Controls
{
    public class IdentifiableTabItem : LayoutDocument
    {
        public int ID { get; set; }
        
        public CircuitView CircuitView { get; set; }
    }
}