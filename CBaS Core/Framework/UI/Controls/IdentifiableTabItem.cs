using System.Windows.Controls;

namespace CBaSCore.Framework.UI.Controls
{
    public class IdentifiableTabItem : TabItem
    {
        public int ID { get; set; }
        
        public CircuitView CircuitView { get; set; }
    }
}