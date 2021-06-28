using CBaS_Core.Framework.UI;
using CBaS_Core.Logic.Resources;
using CBaS_Core.Logic.UI.Controls;
using System.Windows.Input;

namespace CBaS_Core.Logic.UI.Toolbar_Buttons
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