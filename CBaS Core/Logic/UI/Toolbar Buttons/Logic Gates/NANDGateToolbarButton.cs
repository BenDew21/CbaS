﻿using CBaS_Core.Framework.UI;
using CBaS_Core.Logic.Resources;
using CBaS_Core.Logic.UI.Controls.Logic_Gates;
using System.Windows.Input;

namespace CBaS_Core.Logic.UI.Toolbar_Buttons.Logic_Gates
{
    public class NANDGateToolbarButton : BaseToolbarItem
    {
        public NANDGateToolbarButton() : base(MenuName.Logic, Logic_Resources.NAND, "NAND Gate", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new NANDGate());
        }
    }
}