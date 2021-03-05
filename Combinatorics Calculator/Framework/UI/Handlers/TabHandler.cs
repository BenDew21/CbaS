using Combinatorics_Calculator.Framework.UI.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Combinatorics_Calculator.Framework.UI.Handlers
{
    public class TabHandler
    {
        private static TabHandler _instance = null;

        private MainWindow _mainWindow;
        private Dictionary<int, ZoomTabContent> _tabs = new Dictionary<int, ZoomTabContent>();
        private Dictionary<int, double> _zoomLevels = new Dictionary<int, double>();

        public static TabHandler GetInstance()
        {
            if (_instance == null) _instance = new TabHandler();
            return _instance;
        }

        public void RegisterMainWindow(MainWindow window)
        {
            _mainWindow = window;
        }
    }
}
