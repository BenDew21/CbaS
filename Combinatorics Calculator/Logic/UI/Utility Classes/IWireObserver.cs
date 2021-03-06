using Combinatorics_Calculator.Logic.UI.Controls.Wiring;

namespace Combinatorics_Calculator.Logic.UI.Utility_Classes
{
    public interface IWireObserver
    {
        void WireStatusChanged(Wire wire, bool status);
    }
}