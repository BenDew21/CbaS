using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Controls;

namespace Combinatorics_Calculator.Framework.UI.Handlers
{
    public class ToolbarEventHandler
    {
        private static ToolbarEventHandler _instance;
        private CircuitView _circuitView;

        public static ToolbarEventHandler GetInstance()
        {
            if (_instance == null) _instance = new ToolbarEventHandler();
            return _instance;
        }

        public void RegisterCircuitView(CircuitView view)
        {
            _circuitView = view;
        }

        public void CanvasButtonPressed(bool status, ICanvasElement gate)
        {
            _circuitView.UnregisterControl();

            if (status)
            {
                _circuitView.RegisterControl(gate);
            }
        }
    }
}