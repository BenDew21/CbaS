namespace CBaSCore.Logic.Business
{
    public interface IBusinessWireObserver
    {
        void WireStatusChanged(BusinessWire wire, bool status);
    }
}