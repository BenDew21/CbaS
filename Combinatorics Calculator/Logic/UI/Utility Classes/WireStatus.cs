using Combinatorics_Calculator.Framework.UI.Controls;
using Combinatorics_Calculator.Logic.UI.Controls;
using System.Windows.Controls;

namespace Combinatorics_Calculator.Logic.UI.Utility_Classes
{
    public class WireStatus
    {
        private CircuitView _canvas;

        private bool _isSelected = false;
        private Wire _wire;

        private static WireStatus _instance = null;

        public static WireStatus GetInstance()
        {
            if (_instance == null) _instance = new WireStatus();
            return _instance;
        }

        public void SetCircuitView(CircuitView canvas)
        {
            _canvas = canvas;
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
            if (wire != null)
            {
                _canvas.Children.Add(wire.GetControl());
            }
        }

        public void SetStart(double x, double y)
        {
            _wire = new Wire();
            _wire.SetStart(x, y);
            _canvas.Children.Add(_wire.GetControl());
            Canvas.SetZIndex(_wire.GetControl(), 1);
        }

        public void SetEnd(double x, double y, IWireObserver observer)
        {
            _wire.SetEnd(x, y, observer);
            _wire = null;
        }
    }
}