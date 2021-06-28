using CBaS_Core.Framework.UI;
using CBaS_Core.Logic.Resources;
using CBaS_Core.Logic.UI.Controls;
using System.Windows.Input;

namespace CBaS_Core.Logic.UI.Toolbar_Buttons
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