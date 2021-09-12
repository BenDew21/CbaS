using System.Collections.Generic;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.Business
{
    public abstract class BaseBusinessGate : IBusinessWireObserver
    {
        // Wires
        protected Dictionary<int, BusinessWire> inputWires = new();
        protected Dictionary<int, BusinessWire> outputWires = new();
        
        public void AddInputWire(int inputIndex, BusinessWire wire)
        {
            inputWires.Add(inputIndex, wire);
        }
        
        public void AddOutputWire(int outputIndex, BusinessWire wire)
        {
            outputWires.Add(outputIndex, wire);
        }
        
        public void WireStatusChanged(BusinessWire wire, bool status)
        {
            CalculateOutput();
        }

        public abstract void CalculateOutput();
    }
}