using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI
{
    public class NOTGateToolbarButton : BaseToolbarItem
    {
        public NOTGateToolbarButton() : base(MenuName.Logic, Logic_Resources.NOT, "NOT Gate", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new NOTGate());
        }
    }
}