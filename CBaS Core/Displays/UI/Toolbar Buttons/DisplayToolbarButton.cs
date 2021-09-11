using System.Windows.Input;
using CBaSCore.Displays.UI.Controls;
using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;

namespace CBaSCore.Displays.UI.Toolbar_Buttons
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