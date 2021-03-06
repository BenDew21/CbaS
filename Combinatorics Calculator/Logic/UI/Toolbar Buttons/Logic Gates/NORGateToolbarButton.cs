using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls.Logic_Gates;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI.Toolbar_Buttons.Logic_Gates
{
    public class NORGateToolbarButton : BaseToolbarItem
    {
        public NORGateToolbarButton() : base(MenuName.Logic, Logic_Resources.NOR, "NOR Gate", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new NORGate());
        }
    }
}