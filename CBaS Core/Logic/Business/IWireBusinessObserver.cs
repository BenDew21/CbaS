namespace CBaSCore.Logic.Business
{
    public interface IWireBusinessObserver
    {
        void WireStatusChanged(WireBusiness wire, bool status);
    }
}