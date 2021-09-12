using System.Collections.Generic;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.Business
{
    public class BusinessWire : IBusinessWireObserver
    {
        private int _id;
        private bool _status = false;
        
        public int ID
        {
            get => _id;
            set => _id = value;
        }

        public bool Status
        {
            get => _status;
            set => _status = value;
        }

        private IBusinessWireObserver _wireObserver;

        private List<BusinessWire> OutputWires = new();
        
        public void ToggleStatus(bool status)
        {
            Status = status;
            if (_wireObserver != null) _wireObserver.WireStatusChanged(this, status);
            WireStatusChanged(this, status);
        }

        public void WireStatusChanged(BusinessWire wire, bool status)
        {
            foreach (var outputWire in OutputWires)
                if (outputWire != this)
                    outputWire.ToggleStatus(status);
        }

        public void AddOutputWire(BusinessWire wire)
        {
            if (wire.ID != ID) OutputWires.Add(wire);
        }
        
        public void RegisterWireObserver(IBusinessWireObserver observer)
        {
            _wireObserver = observer;
        }
    }
}