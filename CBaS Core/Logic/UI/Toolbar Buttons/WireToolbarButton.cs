using CBaS_Core.Framework.UI;
using CBaS_Core.Logic.Resources;
using CBaS_Core.Logic.UI.Utility_Classes;
using System.Windows.Input;

namespace CBaS_Core.Logic.UI.Toolbar_Buttons
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