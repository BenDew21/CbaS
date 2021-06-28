using System.Windows.Controls;

namespace CBaS_Core.Framework.UI.Controls
{
    public class CircuitViewTab : TabItem
    {
        public int ID { get; set; }

        public CircuitView View { get; set; }

        public CircuitViewTab(int id)
        {
            ID = id;
            View = new CircuitView();
            Content = View;
        }
    }
}