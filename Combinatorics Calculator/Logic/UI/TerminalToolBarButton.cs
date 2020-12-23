using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI
{
    public class TerminalToolBarButton : BaseToolbarItem
    {
        public TerminalToolBarButton() : base(MenuName.Logic, Logic_Resources.Terminal, "Input", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new InputControl());
        }
    }
}