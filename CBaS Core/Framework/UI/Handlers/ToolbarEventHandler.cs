﻿using CBaS_Core.Framework.Business;
using CBaS_Core.Framework.UI.Base_Classes;
using CBaS_Core.Framework.UI.Controls;

namespace CBaS_Core.Framework.UI.Handlers
{
    public class ToolbarEventHandler
    {
        private static ToolbarEventHandler _instance;
        private CircuitView _circuitView;

        private DragHandler _dragHandler = DragHandler.GetInstance();

        public static ToolbarEventHandler GetInstance()
        {
            if (_instance == null) _instance = new ToolbarEventHandler();
            return _instance;
        }

        public void RegisterCircuitView(CircuitView view)
        {
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

            if (status)
            {
                _circuitView.RegisterControl(gate);
            }
        }

        public void Save()
        {
            CircuitHandler.GetInstance().SaveAll();
        }

        public void Load()
        {
            CircuitHandler.GetInstance().Load("Circuit_New.ccc");
        }

        public void Load(string path)
        {
        }
    }
}