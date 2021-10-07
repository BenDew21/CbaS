using System.Collections.Generic;

namespace CBaSCore.Logic.Business
{
    public abstract class BaseGateWireBusiness : IWireBusinessObserver
    {
        // Wires
        protected readonly Dictionary<int, WireBusiness> inputWires = new();
        protected readonly Dictionary<int, WireBusiness> outputWires = new();
        
        public void AddInputWire(int inputIndex, WireBusiness wire)
        {
            inputWires.Add(inputIndex, wire);
        }
        
        public void AddOutputWire(int outputIndex, WireBusiness wire)
        {
            outputWires.Add(outputIndex, wire);
        }
        
        public void WireStatusChanged(WireBusiness wire, bool status)
        {
            CalculateOutput();
        }

        public abstract void CalculateOutput();
    }
}