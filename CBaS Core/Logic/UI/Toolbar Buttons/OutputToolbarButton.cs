using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls;
using System.Windows.Input;

namespace CBaSCore.Logic.UI.Toolbar_Buttons
{
    public class OutputToolbarButton : BaseToolbarItem
    {
        public OutputToolbarButton() : base(MenuName.Logic, Logic_Resources.Output, "Output", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new OutputControl());
        }
    }
}