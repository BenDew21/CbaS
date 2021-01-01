using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Combinatorics_Calculator.Logic.UI.Controls
{
    public class InputControl : ICanvasElement
    {
        private Ellipse _circle;

        private Wire _outputWire;

        private bool _outputting = false;

        public InputControl()
        {
            CreateControl();
        }

        public void CreateControl()
        {
            _circle = new Ellipse();
            _circle.Width = 10;
            _circle.Height = 10;
            _circle.Fill = Brushes.Red;
            _circle.Stroke = Brushes.Red;
            _circle.StrokeThickness = 0.5;
        }

        public void SetOutputWire(Wire wire)
        {
            _outputWire = wire;
            _outputWire.ToggleStatus(_outputting);
        }

        public void SetPlaced()
        {
            Canvas.SetZIndex(_circle, 3);
            _circle.MouseDown += Control_MouseDown;
        }

        private void Control_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (WireStatus.GetInstance().GetSelected())
            {
                if (WireStatus.GetInstance().GetWire() == null)
                {
                    double x = Canvas.GetLeft(_circle) + 5;
                    double y = Canvas.GetTop(_circle) + 5;

                    WireStatus.GetInstance().SetStart(x, y);
                    SetOutputWire(WireStatus.GetInstance().GetWire());
                }
            }
            else
            {
                _outputting = !_outputting;
                if (_outputting)
                {
                    _circle.Fill = Brushes.Green;
                    _circle.Stroke = Brushes.Green;
                }
                else
                {
                    _circle.Fill = Brushes.Red;
                    _circle.Stroke = Brushes.Red;
                }

                if (_outputWire != null)
                {
                    _outputWire.ToggleStatus(_outputting);
                }
            }

            e.Handled = true;
        }

        public UIElement GetControl()
        {
            return _circle;
        }

        public ICanvasElement GetNew()
        {
            return new InputControl();
        }
    }
}