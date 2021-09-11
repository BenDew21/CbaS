using System.Windows.Input;
using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls.EEPROMs;

namespace CBaSCore.Logic.UI.Toolbar_Buttons.EEPROMs
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