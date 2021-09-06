using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls.Logic_Gates;
using System.Windows.Input;

namespace CBaSCore.Logic.UI.Toolbar_Buttons.Logic_Gates
{
    public class ORGateToolbarButton : BaseToolbarItem
    {
        public ORGateToolbarButton() : base(MenuName.Logic, Logic_Resources.OR, "OR Gate", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new ORGate());
        }
    }
}