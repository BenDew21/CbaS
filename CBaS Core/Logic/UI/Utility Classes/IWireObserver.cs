using CBaSCore.Logic.UI.Controls.Wiring;

namespace CBaSCore.Logic.UI.Utility_Classes
{
    public interface IWireObserver
    {
        void WireStatusChanged(Wire wire, bool status);
    }
}