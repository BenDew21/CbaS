using CBaS_Core.Logic.Resources;
using CBaS_Core.Displays.UI.Controls;
using CBaS_Core.Framework.UI;
using System.Windows.Input;

namespace CBaS_Core.Displays.UI.Toolbar_Buttons
{
    public class DisplayToolbarButton : BaseToolbarItem
    {
        public DisplayToolbarButton() : base(MenuName.Logic, Logic_Resources.Output, "7 Segment Display", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new SegmentedDisplay());
        }
    }
}