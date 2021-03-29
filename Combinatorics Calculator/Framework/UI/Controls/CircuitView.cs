using Combinatorics_Calculator.Framework.Business;
using Combinatorics_Calculator.Framework.Resources;
using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Handlers;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Project.Storage;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Framework.UI.Controls
{
    public class CircuitView : Canvas
    {
        private ICanvasElement _selectedGate;
        private Image _backgroundImage;

        private Circuit circuit = new Circuit();

        public CircuitView()
        {
            _backgroundImage = new Image();
            _backgroundImage.Source = UIIconConverter.BitmapToBitmapImage(Framework_Resources.Background);
            _backgroundImage.Stretch = System.Windows.Media.Stretch.Fill;
            Children.Add(_backgroundImage);

            ToolbarEventHandler.GetInstance().RegisterCircuitView(this);
            CircuitHandler.GetInstance().RegisterCircuitView(this);
            DragHandler.GetInstance().RegisterCircuitView(this);
        }

        public CircuitView(Circuit circuit)
        {
            _backgroundImage = new Image();
            _backgroundImage.Source = UIIconConverter.BitmapToBitmapImage(Framework_Resources.Background);
            _backgroundImage.Stretch = System.Windows.Media.Stretch.Fill;
            Children.Add(_backgroundImage);

            ToolbarEventHandler.GetInstance().RegisterCircuitView(this);
            CircuitHandler.GetInstance().RegisterCircuitView(this);
            DragHandler.GetInstance().RegisterCircuitView(this);
        }

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
            Debug.WriteLine("Control registered " + gate.GetType().ToString());
            _selectedGate = gate;
            MouseEnter += CircuitView_MouseEnter;
            MouseMove += CircuitView_MouseMove;
            MouseDown += CircuitView_MouseDown;
            MouseLeave += CircuitView_MouseLeave;
        }

        public void UnregisterControl()
        {
            Debug.WriteLine("Control unregistered");
            _selectedGate = null;
            MouseEnter -= CircuitView_MouseEnter;
            MouseMove -= CircuitView_MouseMove;
            MouseDown -= CircuitView_MouseDown;
            MouseLeave -= CircuitView_MouseLeave;
        }

        private void CircuitView_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Children.Add(_selectedGate.GetControl());

            Canvas.SetZIndex(_selectedGate.GetControl(), 2);

            System.Windows.Point position = e.GetPosition(this);
            UpdateGatePosition(position);
        }

        private void CircuitView_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Point position = e.GetPosition(this);
            UpdateGatePosition(position);
        }

        private void CircuitView_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                _selectedGate.SetPlaced();
                AddNewControl(_selectedGate);

                _selectedGate = _selectedGate.GetNew();
                Children.Add(_selectedGate.GetControl());

                Canvas.SetZIndex(_selectedGate.GetControl(), 2);
                System.Windows.Point position = e.GetPosition(this);
                UpdateGatePosition(position);
            }
        }

        public void AddControl(ICanvasElement control)
        {
            if (!Children.Contains(control.GetControl()))
            {
                Children.Add(control.GetControl());
            }

            Canvas.SetZIndex(control.GetControl(), 2);
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
            Canvas.SetZIndex(wire.GetControl(), 1);
            Canvas.SetZIndex(wire.GetStartEllipse(), 2);
            Canvas.SetZIndex(wire.GetEndEllipse(), 2);
        }

        private void CircuitView_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Children.Remove(_selectedGate.GetControl());
        }

        private void UpdateGatePosition(System.Windows.Point point)
        {
            Tuple<double, double> snappedValues = Utilities.GetSnap(point.X, point.Y, _selectedGate.GetSnap());
            Canvas.SetLeft(_selectedGate.GetControl(), snappedValues.Item1 + _selectedGate.GetOffset());
            Canvas.SetTop(_selectedGate.GetControl(), snappedValues.Item2 + _selectedGate.GetOffset());

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