using System.Windows.Controls;

namespace CBaSCore.Framework.UI.Controls
{
    public class CircuitViewTab : TabItem
    {
        public CircuitViewTab(int id)
        {
            ID = id;
            View = new CircuitView();
            Content = View;
        }

        public int ID { get; set; }

        public CircuitView View { get; set; }
    }
}