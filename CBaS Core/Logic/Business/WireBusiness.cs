using System.Collections.Generic;
using CBaSCore.Logic.UI.Controls.Wiring;

namespace CBaSCore.Logic.Business
{
    public class WireBusiness : IWireBusinessObserver
    {
        private int _id;
        private bool _status = false;

        private Wire _parent;
        
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

        private IWireBusinessObserver _observer;

        private readonly List<WireBusiness> OutputWires = new();

        public void SetParent(Wire parent)
        {
            _parent = parent;
        }
        
        public void ToggleStatus(bool status)
        {
            Status = status;
            _observer?.WireStatusChanged(this, status);
            WireStatusChanged(this, status);
            _parent?.UpdateVisualStatus();
        }

        public void WireStatusChanged(WireBusiness wire, bool status)
        {
            foreach (var outputWire in OutputWires)
                if (outputWire != this)
                    outputWire.ToggleStatus(status);
        }

        public void AddOutputWire(WireBusiness wire)
        {
            if (wire.ID != ID) OutputWires.Add(wire);
        }
        
        public void RegisterWireObserver(IWireBusinessObserver observer)
        {
            _observer = observer;
        }
    }
}