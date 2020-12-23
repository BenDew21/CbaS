﻿using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System.Diagnostics;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI
{
    public class WireToolbarButton : BaseToolbarItem
    {
        public WireToolbarButton() : base(MenuName.Logic, Logic_Resources.Wire, "Wire", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            Debug.WriteLine("Wire button is {0}", IsSelected);
            WireStatus.GetInstance().SetSelected(IsSelected);
        }
    }
}