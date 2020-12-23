using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Combinatorics_Calculator.Logic.UI.Controls
{
    public class Wire
    {
        private Line _line;
        private IWireObserver _wireObserver;

        private double _x1, _x2, _y1, _y2;
        private bool _status;

        public Wire()
        {
            _line = new Line();
            _line.Stroke = Brushes.Black;
            _line.StrokeThickness = 1.3;
        }

        public Line GetControl()
        {
            return _line;
        }

        public void SetStart(double x, double y)
        {
            _x1 = x;
            _y1 = y;
            _line.X1 = x;
            _line.Y1 = y;
        }

        public void SetEnd(double x, double y, IWireObserver observer)
        {
            _x2 = x;
            _y2 = y;
            _line.X2 = x;
            _line.Y2 = y;

            _wireObserver = observer;
        }

        public Line GetLine()
        {
            return _line;
        }

        public void ToggleStatus(bool status)
        {
            _status = status;
            if (status)
            {
                _line.Stroke = Brushes.Green;
            }
            else
            {
                _line.Stroke = Brushes.Black;
            }
            if (_wireObserver != null) _wireObserver.WireStatusChanged(this, status);
        }

        public bool GetStatus()
        {
            return _status;
        }
    }
}