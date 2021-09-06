using CBaSCore.Framework.UI.Controls;

namespace CBaSCore.Framework.UI.Handlers
{
    public class DrawingHandler
    {
        private static DrawingHandler _instance = null;
        private CircuitView _circuitView;

        public static DrawingHandler GetInstance()
        {
            if (_instance == null) _instance = new DrawingHandler();
            return _instance;
        }

        public void RegisterCircuitView(CircuitView view)
        {
            _circuitView = view;
        }
    }
}