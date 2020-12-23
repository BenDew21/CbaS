using Combinatorics_Calculator.Framework.UI.Handlers;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Controls;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Combinatorics_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MouseHandlingMode _mouseMode;
        private Point _originalMouseDownPoint;

        public MainWindow()
        {
            InitializeComponent();
            WireStatus.GetInstance().SetCircuitView(CircuitViewControl);
            ToolbarEventHandler.GetInstance().RegisterCircuitView(CircuitViewControl);

            ANDGate gate = new ANDGate();
            CircuitViewControl.Children.Add(gate.GetControl());
            gate.SetPlaced();
            Canvas.SetLeft(gate.GetControl(), 100);
            Canvas.SetTop(gate.GetControl(), 100);

            NOTGate not = new NOTGate();
            CircuitViewControl.Children.Add(not.GetControl());
            not.SetPlaced();
            Canvas.SetLeft(not.GetControl(), 300);
            Canvas.SetTop(not.GetControl(), 100);

            OutputControl control = new OutputControl();
            CircuitViewControl.Children.Add(control.GetControl());
            control.SetPlaced();
            Canvas.SetLeft(control.GetControl(), 300);
            Canvas.SetTop(control.GetControl(), 800);
        }

        /// <summary>
        /// Event raised by rotating the mouse wheel.
        /// </summary>
        private void CircuitControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                Point curContentMousePoint = e.GetPosition(CircuitViewControl);
                ZoomIn(curContentMousePoint);
            }
            else if (e.Delta < 0)
            {
                Point curContentMousePoint = e.GetPosition(CircuitViewControl);
                ZoomOut(curContentMousePoint);
            }
        }

        /// <summary>
        /// Zoom the viewport out, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomOut(Point contentZoomCenter)
        {
            zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale - 0.1, contentZoomCenter);
        }

        /// <summary>
        /// Zoom the viewport in, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomIn(Point contentZoomCenter)
        {
            zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale + 0.1, contentZoomCenter);
        }

        private void CircuitControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CircuitViewControl.Focus();
            var mouseButtonDown = e.ChangedButton;

            if (mouseButtonDown == MouseButton.Right)
            {
                _mouseMode = MouseHandlingMode.DragPanning;
                _originalMouseDownPoint = e.GetPosition(CircuitViewControl);
            }
        }

        private void CircuitControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mouseMode = MouseHandlingMode.None;
        }

        private void CircuitControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (WireStatus.GetInstance().GetSelected())
            {
                Wire wire = WireStatus.GetInstance().GetWire();
                if (wire != null)
                {
                    Point position = e.GetPosition(CircuitViewControl);
                    Tuple<double, double> snappedValues = Utilities.GetSnap(position.X, position.Y, 10);

                    wire.SetEnd(snappedValues.Item1, snappedValues.Item2, null);
                }
            }

            if (_mouseMode == MouseHandlingMode.DragPanning)
            {
                var mousePoint = e.GetPosition(CircuitViewControl);
                var offset = mousePoint - _originalMouseDownPoint;

                zoomAndPanControl.ContentOffsetX -= offset.X;
                zoomAndPanControl.ContentOffsetY -= offset.Y;
            }
        }

        private void ZoomOneHundredPercent_Click(object sender, RoutedEventArgs e)
        {
            zoomAndPanControl.AnimatedZoomTo(1.0);
        }

        private void ZoomIn_Button_Click(object sender, RoutedEventArgs e)
        {
            ZoomIn(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));
        }

        private void ZoomOut_Button_Click(object sender, RoutedEventArgs e)
        {
            ZoomOut(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));
        }
    }
}