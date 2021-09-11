using System.Windows;
using System.Windows.Input;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.UI.Utility_Classes;
using CBaSCore.Project.Storage;

namespace CBaSCore.Framework.UI.Controls
{
    /// <summary>
    ///     Interaction logic for ZoomTabContent.xaml
    /// </summary>
    public partial class ZoomTabContent : IdentifiableTabItem
    {
        private MouseHandlingMode _mouseMode;
        private Point _originalMouseDownPoint;

        public ZoomTabContent()
        {
            InitializeComponent();
            CircuitView = GetCircuitView();
            Closed += (sender, args) =>
            {
                CircuitView.UnregisterView();
                UnregisterControl();
                Content = null;
            };
            IsSelectedChanged += (sender, args) => { TabHandler.GetInstance().Tab_SelectionChanged(sender, args); };
        }

        public ZoomTabContent(Circuit circuit) : this()
        {
            CircuitView.ID = ID;
            CircuitView.Draw(circuit);
        }

        public void SetID(int id)
        {
            ID = id;
            CircuitView.ID = id;
        }

        //public void RegisterEvents()
        //{
        //}

        //public void UnregisterEvents()
        //{
        //}

        public CircuitView GetCircuitView()
        {
            return CircuitViewControl;
        }

        public void UnregisterControl()
        {
            CircuitViewControl.MouseWheel -= CircuitControl_MouseWheel;
            CircuitViewControl.MouseDown -= CircuitControl_MouseDown;
            CircuitViewControl.MouseUp -= CircuitControl_MouseUp;
            CircuitViewControl.MouseMove -= CircuitControl_MouseMove;
        }

        private void CircuitControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                var curContentMousePoint = e.GetPosition(CircuitView);
                ZoomIn(curContentMousePoint);
            }
            else if (e.Delta < 0)
            {
                var curContentMousePoint = e.GetPosition(CircuitView);
                ZoomOut(curContentMousePoint);
            }
        }

        /// <summary>
        ///     Zoom the viewport out, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomOut(Point contentZoomCenter)
        {
            zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale - 0.1, contentZoomCenter);
        }

        /// <summary>
        ///     Zoom the viewport in, centering on the specified point (in content coordinates).
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
                if (WireStatus.GetInstance().GetWire() != null)
                {
                    var position = e.GetPosition(CircuitView);
                    var snappedValues = Utilities.GetSnap(position.X, position.Y, 10);

                    WireStatus.GetInstance().SetEnd(snappedValues.Item1, snappedValues.Item2, null);
                }

            if (mouseButtonDown == MouseButton.Right)
            {
                _mouseMode = MouseHandlingMode.DragPanning;
                _originalMouseDownPoint = e.GetPosition(CircuitView);
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
                var wire = WireStatus.GetInstance().GetWire();
                if (wire != null)
                {
                    var position = e.GetPosition(CircuitView);
                    var snappedValues = Utilities.GetSnap(position.X, position.Y, 10);

                    wire.SetEnd(snappedValues.Item1, snappedValues.Item2);
                }
            }

            if (_mouseMode == MouseHandlingMode.DragPanning)
            {
                var mousePoint = e.GetPosition(CircuitView);
                var offset = mousePoint - _originalMouseDownPoint;

                zoomAndPanControl.ContentOffsetX -= offset.X;
                zoomAndPanControl.ContentOffsetY -= offset.Y;
            }
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            // TabHandler.GetInstance().RemoveTab(this);
        }
    }
}