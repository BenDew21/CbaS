using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls.Logic_Gates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI.Toolbar_Buttons.Logic_Gates
{
    public class XORGateToolbarButton : BaseToolbarItem
    {
        public XORGateToolbarButton() : base(MenuName.Logic, Logic_Resources.XOR, "XOR Gate", true) { }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new XORGate());
        }
    }
}