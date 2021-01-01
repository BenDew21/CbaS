using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Combinatorics_Calculator.Logic.UI.Controls.Wiring
{
    public class WireTerminal : Shape
    {
        private Wire _parentWire;

        private EllipseGeometry _ellipse;

        protected override Geometry DefiningGeometry
        {
            get
            {
                return _ellipse;
            }
        }

        public WireTerminal(Wire parentWire, Point centre)
        {
            _parentWire = parentWire;

            Point p = new Point();
            p.X = 2.5;
            p.Y = 2.5;

            _ellipse = new EllipseGeometry(p, 3, 3);
            Stroke = Brushes.Black;
            Fill = Brushes.Black;
            MouseDown += WireTerminal_MouseDown;
        }

        private void WireTerminal_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point centre = new Point(Canvas.GetLeft(this) + 2.5, Canvas.GetTop(this) + 2.5);
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
