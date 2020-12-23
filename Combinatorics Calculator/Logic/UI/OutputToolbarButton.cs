using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI
{
    public class OutputToolbarButton : BaseToolbarItem
    {
        public OutputToolbarButton() : base(MenuName.Logic, Logic_Resources.Output, "Output", true) { }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new OutputControl());
        }
    }
}
