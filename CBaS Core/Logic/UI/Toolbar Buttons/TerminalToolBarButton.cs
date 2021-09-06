using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls;
using System.Windows.Input;

namespace CBaSCore.Logic.UI.Toolbar_Buttons
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