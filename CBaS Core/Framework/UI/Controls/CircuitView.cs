using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using CBaSCore.Framework.Business;
using CBaSCore.Framework.Resources;
using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Project.Storage;

namespace CBaSCore.Framework.UI.Controls
{
    public class CircuitView : Canvas
    {
        private readonly Image _backgroundImage;
        private ICanvasElement _selectedGate;

        private Circuit circuit = new();

        public CircuitView()
        {
            _backgroundImage = new Image();
            _backgroundImage.Source = UiIconConverter.BitmapToBitmapImage(Framework_Resources.Background);
            _backgroundImage.Stretch = Stretch.Fill;
            Children.Add(_backgroundImage);

            ToolbarEventHandler.GetInstance().RegisterCircuitView(this);
            CircuitHandler.GetInstance().RegisterCircuitView(this);
            DragHandler.GetInstance().RegisterCircuitView(this);
        }

        public CircuitView(Circuit circuit)
        {
            _backgroundImage = new Image();
            _backgroundImage.Source = UiIconConverter.BitmapToBitmapImage(Framework_Resources.Background);
            _backgroundImage.Stretch = Stretch.Fill;
            Children.Add(_backgroundImage);

            ToolbarEventHandler.GetInstance().RegisterCircuitView(this);
            CircuitHandler.GetInstance().RegisterCircuitView(this);
            DragHandler.GetInstance().RegisterCircuitView(this);
        }

        public int ID { get; set; }

        public void Draw(XElement element)
        {
            circuit = new Circuit(element);
            circuit.Draw(this);
        }

        public void Draw(Circuit circuit)
        {
            this.circuit = circuit;
            circuit.Draw(this);
        }

        public Circuit GetCircuit()
        {
            return circuit;
        }

        public void RegisterControl(ICanvasElement gate)
        {
            _selectedGate = gate;
            MouseEnter += CircuitView_MouseEnter;
            MouseMove += CircuitView_MouseMove;
            MouseDown += CircuitView_MouseDown;
            MouseLeave += CircuitView_MouseLeave;
        }

        public void UnregisterControl()
        {
            _selectedGate = null;
            MouseEnter -= CircuitView_MouseEnter;
            MouseMove -= CircuitView_MouseMove;
            MouseDown -= CircuitView_MouseDown;
            MouseLeave -= CircuitView_MouseLeave;
        }

        public void UnregisterView()
        {
            MouseEnter -= CircuitView_MouseEnter;
            MouseMove -= CircuitView_MouseMove;
            MouseDown -= CircuitView_MouseDown;
            MouseLeave -= CircuitView_MouseLeave;

            Children.Clear();
        }

        private void CircuitView_MouseEnter(object sender, MouseEventArgs e)
        {
            Children.Add(_selectedGate.GetControl());

            SetZIndex(_selectedGate.GetControl(), 2);

            var position = e.GetPosition(this);
            UpdateGatePosition(position);
        }

        private void CircuitView_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            UpdateGatePosition(position);
        }

        private void CircuitView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _selectedGate.SetPlaced();
                AddNewControl(_selectedGate);

                _selectedGate = _selectedGate.GetNew();
                Children.Add(_selectedGate.GetControl());

                SetZIndex(_selectedGate.GetControl(), 2);
                var position = e.GetPosition(this);
                UpdateGatePosition(position);
            }
        }

        public void AddControl(ICanvasElement control)
        {
            if (!Children.Contains(control.GetControl())) Children.Add(control.GetControl());

            SetZIndex(control.GetControl(), 2);
            control.SetPlaced();
        }

        public void AddNewControl(ICanvasElement control)
        {
            AddControl(control);
            circuit.AddElementToList(control);
        }

        public void AddWire(Wire wire)
        {
            Children.Add(wire.GetControl());
            Children.Add(wire.GetStartEllipse());
            Children.Add(wire.GetEndEllipse());
            SetZIndex(wire.GetControl(), 1);
            SetZIndex(wire.GetStartEllipse(), 2);
            SetZIndex(wire.GetEndEllipse(), 2);
        }

        private void CircuitView_MouseLeave(object sender, MouseEventArgs e)
        {
            Children.Remove(_selectedGate.GetControl());
        }

        private void UpdateGatePosition(Point point)
        {
            var snappedValues = Utilities.GetSnap(point.X, point.Y, _selectedGate.GetSnap());
            SetLeft(_selectedGate.GetControl(), snappedValues.Item1 + _selectedGate.GetOffset());
            SetTop(_selectedGate.GetControl(), snappedValues.Item2 + _selectedGate.GetOffset());

            //if (_selectedGate is DiagramLabel)
            //{
            //    Canvas.SetLeft(_selectedGate.GetControl(), point.X);
            //    Canvas.SetTop(_selectedGate.GetControl(), point.Y);
            //}
            //else
            //{
            //    Tuple<double, double> snappedValues = Utilities.GetSnap(point.X, point.Y, 10);
            //    if (_selectedGate is InputControl)
            //    {
            //        Canvas.SetLeft(_selectedGate.GetControl(), snappedValues.Item1 + 5);
            //        Canvas.SetTop(_selectedGate.GetControl(), snappedValues.Item2 - 5);
            //    }
            //    else
            //    {
            //        Canvas.SetLeft(_selectedGate.GetControl(), snappedValues.Item1);
            //        Canvas.SetTop(_selectedGate.GetControl(), snappedValues.Item2);
            //    }
            //}
        }
    }
}