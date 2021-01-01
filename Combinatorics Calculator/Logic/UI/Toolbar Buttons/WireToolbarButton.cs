using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System.Diagnostics;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI.Toolbar_Buttons
{
    public class WireToolbarButton : BaseToolbarItem
    {
        public WireToolbarButton() : base(MenuName.Logic, Logic_Resources.Wire, "Wire", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            WireStatus.GetInstance().SetSelected(IsSelected);
        }
    }
}