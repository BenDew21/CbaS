using Combinatorics_Calculator.Framework.UI.Handlers;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using Combinatorics_Calculator.Project.Business;
using System;
using System.Windows;
using System.Windows.Input;

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
            ProjectViewHandler.GetInstance().SetTreeView(Explorer);

            TabHandler.GetInstance().RegisterTabControl(CircuitsTabControl);
        }

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

            if (mouseButtonDown == MouseButton.Left && WireStatus.GetInstance().GetSelected())
            {
                if (WireStatus.GetInstance().GetWire() != null)
                {
                    Point position = e.GetPosition(CircuitViewControl);
                    Tuple<double, double> snappedValues = Utilities.GetSnap(position.X, position.Y, 10);

                    WireStatus.GetInstance().SetEnd(snappedValues.Item1, snappedValues.Item2, null);
                }
            }

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
    }
}