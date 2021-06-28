using CBaS_Core.Logic.UI.Controls.Wiring;

namespace CBaS_Core.Logic.UI.Utility_Classes
{
    public interface IWireObserver
    {
        void WireStatusChanged(Wire wire, bool status);
    }
}