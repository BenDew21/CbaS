namespace CBaSCore.Logic.Business.Gate_Business
{
    public class BusinessOutputControl : IBusinessWireObserver
    {
        public bool Outputting { get; set; }

        private BusinessWire _inputWire;

        public void SetInputWire(BusinessWire inputWire)
        {
            _inputWire = inputWire;
            inputWire.RegisterWireObserver(this);
        }

        public void WireStatusChanged(BusinessWire wire, bool status)
        {
            Outputting = status;
        }
    }
}