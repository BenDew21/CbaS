using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CBaSCore.Chip.Storage;
using CBaSCore.Chip.Utility_Classes;
using CBaSCore.Framework.UI.Controls;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Chip.UI.Controls
{
    /// <summary>
    /// Interaction logic for BuilderTab.xaml
    /// </summary>
    public partial class BuilderTab : IdentifiableTabItem
    {
        private MouseHandlingMode _mouseMode;
        private Point _originalMouseDownPoint;

        private List<PinMapping> _inputMappings;
        private List<PinMapping> _outputMappings;


        public BuilderTab()
        {
            InitializeComponent();

            List<string> names = new List<string>();
            foreach (var name in Enum.GetValues(typeof(MappingType)))
            {
                names.Add(MappingTypeStringConverter.EnumToString((MappingType) name));
            }

            MappingTypeColumn.ItemsSource = names;
            MappingDataGrid.Items.Add(new object());

            InputMappingsDataGrid.ItemsSource = _inputMappings;
            OutputMappingsDataGrid.ItemsSource = _outputMappings; 
        }

        public CircuitView GetCircuitView()
        {
            return CircuitViewControl;
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
            if (_mouseMode == MouseHandlingMode.DragPanning)
            {
                var mousePoint = e.GetPosition(CircuitViewControl);
                var offset = mousePoint - _originalMouseDownPoint;

                zoomAndPanControl.ContentOffsetX -= offset.X;
                zoomAndPanControl.ContentOffsetY -= offset.Y;
            }
        }

        private void button_close_Click(object sender, RoutedEventArgs e)
        {
            TabHandler.GetInstance().RemoveTab(this);
        }
    }
}
