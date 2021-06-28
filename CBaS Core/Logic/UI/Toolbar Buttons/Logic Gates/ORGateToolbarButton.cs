using CBaS_Core.Framework.UI;
using CBaS_Core.Logic.Resources;
using CBaS_Core.Logic.UI.Controls.Logic_Gates;
using System.Windows.Input;

namespace CBaS_Core.Logic.UI.Toolbar_Buttons.Logic_Gates
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