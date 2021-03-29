using Combinatorics_Calculator.Displays.UI.Controls;
using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Combinatorics_Calculator.Displays.UI.Toolbar_Buttons
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
