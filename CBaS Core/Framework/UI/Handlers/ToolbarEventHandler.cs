using CBaSCore.Framework.Business;
using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Framework.UI.Controls;

namespace CBaSCore.Framework.UI.Handlers
{
    public class ToolbarEventHandler
    {
        private static ToolbarEventHandler _instance;
        private CircuitView _circuitView;

        private readonly DragHandler _dragHandler = DragHandler.GetInstance();

        public static ToolbarEventHandler GetInstance()
        {
            if (_instance == null) _instance = new ToolbarEventHandler();
            return _instance;
        }

        public void RegisterCircuitView(CircuitView view)
        {
            if (_circuitView != null) _circuitView.UnregisterControl();

            _circuitView = view;
        }

        public void DragPressed(bool status)
        {
            _circuitView.UnregisterControl();
            _dragHandler.IsActive = status;
        }

        public void CanvasButtonPressed(bool status, ICanvasElement gate)
        {
            _circuitView.UnregisterControl();

            if (status) _circuitView.RegisterControl(gate);
        }

        public void Save()
        {
            // CircuitHandler.GetInstance().Save(@"C:\Programming\CBaS.cbasc");
            CircuitHandler.GetInstance().SaveAll();
        }

        public void Load(string path)
        {
        }
    }
}