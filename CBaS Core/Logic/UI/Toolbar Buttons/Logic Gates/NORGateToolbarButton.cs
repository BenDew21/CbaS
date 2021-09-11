using System.Windows.Input;
using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls.Logic_Gates;

namespace CBaSCore.Logic.UI.Toolbar_Buttons.Logic_Gates
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