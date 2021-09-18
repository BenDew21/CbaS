using CBaSCore.Logic.UI.Controls;

namespace CBaSCore.Logic.Business.Gate_Business
{
    public class OutputControlWireBusiness : IWireBusinessObserver
    {
        public bool Outputting { get; set; }

        private WireBusiness _input;

        private OutputControl _parent;

        public void SetParent(OutputControl control)
        {
            _parent = control;
        }
        
        public void SetInputWire(WireBusiness input)
        {
            _input = input;
            input.RegisterWireObserver(this);
        }

        public void WireStatusChanged(WireBusiness wire, bool status)
        {
            Outputting = status;
            _parent?.UpdateDisplay();
        }
    }
}