using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly List<IWireBusinessObserver> _observers = new();

        public void SetParent(Wire parent)
        {
            _parent = parent;
        }
        
        public void ToggleStatus(bool status)
        {
            Status = status;

            foreach (var output in _observers)
            {
                output.WireStatusChanged(null, status);
            }
            
            _parent?.UpdateVisualStatus();
        }

        public void AddOutputWire(WireBusiness wire)
        {
            if (wire.ID != ID)
            {
                _observers.Add(wire);
            }
        }
        
        public void RegisterWireObserver(IWireBusinessObserver observer)
        {
            _observers.Add(observer);
        }

        public void WireStatusChanged(WireBusiness wire, bool status)
        {
            ToggleStatus(status);
        }
    }
}