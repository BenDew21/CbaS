using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI.Toolbar_Buttons
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