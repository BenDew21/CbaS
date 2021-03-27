﻿using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls.EEPROMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI.Toolbar_Buttons.EEPROMs
{
    public class EEPROM28C16ToolbarButton : BaseToolbarItem
    {
        public EEPROM28C16ToolbarButton() : base(MenuName.Logic, Logic_Resources._28C16, "28C16", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new EEPROM28C16());
        }
    }
}