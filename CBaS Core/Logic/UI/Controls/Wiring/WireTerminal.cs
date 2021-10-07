using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CBaSCore.Logic.UI.Controls.Wiring
{
    public class WireTerminal : Shape
    {
        private readonly EllipseGeometry _ellipse;
        private readonly Wire _parentWire;

        public WireTerminal(Wire parentWire, Point centre)
        {
            _parentWire = parentWire;

            var p = new Point();
            p.X = 2.5;
            p.Y = 2.5;

            _ellipse = new EllipseGeometry(p, 3, 3);
            Stroke = Brushes.Black;
            Fill = Brushes.Black;
            MouseDown += WireTerminal_MouseDown;
            Canvas.SetZIndex(this, 100);
        }

        protected override Geometry DefiningGeometry => _ellipse;

        private void WireTerminal_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var centre = new Point(Canvas.GetLeft(this) + 2.5, Canvas.GetTop(this) + 2.5);
            _parentWire.Terminal_MouseDown(_parentWire, centre);

            e.Handled = true;
        }

        public void ToggleStatus(bool status)
        {
            if (status)
            {
                Stroke = Brushes.Green;
                Fill = Brushes.Green;
            }
            else
            {
                Stroke = Brushes.Black;
                Fill = Brushes.Black;
            }
        }
    }
}