using System.Windows.Input;
using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls.Logic_Gates;

namespace CBaSCore.Logic.UI.Toolbar_Buttons.Logic_Gates
{
    public class XORGateToolbarButton : BaseToolbarItem
    {
        public XORGateToolbarButton() : base(MenuName.Logic, Logic_Resources.XOR, "XOR Gate", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new XORGate());
        }
    }
}