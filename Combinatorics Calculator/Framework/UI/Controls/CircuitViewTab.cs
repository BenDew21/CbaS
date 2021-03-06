using System.Windows.Controls;

namespace Combinatorics_Calculator.Framework.UI.Controls
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