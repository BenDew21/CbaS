using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Utility_Classes;
using System.Windows.Input;

namespace CBaSCore.Logic.UI.Toolbar_Buttons
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