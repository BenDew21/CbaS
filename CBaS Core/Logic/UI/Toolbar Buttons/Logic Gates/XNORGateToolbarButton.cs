using System.Windows.Input;
using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls.Logic_Gates;

namespace CBaSCore.Logic.UI.Toolbar_Buttons.Logic_Gates
{
    public class XNORGateToolbarButton : BaseToolbarItem
    {
        public XNORGateToolbarButton() : base(MenuName.Logic, Logic_Resources.XNOR, "XNOR Gate", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new XNORGate());
        }
    }
}