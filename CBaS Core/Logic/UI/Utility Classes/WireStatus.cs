using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CBaSCore.Framework.UI.Controls;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.UI.Controls.Wiring;

namespace CBaSCore.Logic.UI.Utility_Classes
{
    public class WireStatus
    {
        private static WireStatus _instance;
        private CircuitView _canvas;

        private bool _isSelected;
        private Wire _wire;

        public static WireStatus GetInstance()
        {
            if (_instance == null) _instance = new WireStatus();
            return _instance;
        }

        public void SetCircuitView(CircuitView canvas)
        {
            _canvas = canvas;
        }

        public Tuple<double, double> GetPointRelativeToCanvas(MouseButtonEventArgs e)
        {
            var basePoint = e.GetPosition(_canvas);
            return Utilities.GetSnap(basePoint.X, basePoint.Y, 10);
        }

        public Point GetPointCanvas(MouseButtonEventArgs e)
        {
            return e.GetPosition(_canvas);
        }

        public bool GetSelected()
        {
            return _isSelected;
        }

        public void SetSelected(bool selected)
        {
            _isSelected = selected;
        }

        public Wire GetWire()
        {
            return _wire;
        }

        public void SetWire(Wire wire)
        {
            _wire = wire;
            if (wire != null) _canvas.Children.Add(wire.GetControl());
        }

        public void SetStart(double x, double y)
        {
            _wire = new Wire();
            _wire.SetStart(x, y);
            _canvas.Children.Add(_wire.GetControl());
            _canvas.Children.Add(_wire.GetStartEllipse());
            Panel.SetZIndex(_wire.GetControl(), 1);
            Panel.SetZIndex(_wire.GetStartEllipse(), 2);

            _wire.SetID(_canvas.GetCircuit().GetNextWireIterator());
        }

        public void SetEnd(double x, double y, IWireObserver observer)
        {
            _wire.SetEnd(x, y, observer);
            _canvas.Children.Add(_wire.GetEndEllipse());
            Panel.SetZIndex(_wire.GetEndEllipse(), 2);
            _canvas.GetCircuit().AddWire(_wire);
            _wire = null;
        }
    }
}