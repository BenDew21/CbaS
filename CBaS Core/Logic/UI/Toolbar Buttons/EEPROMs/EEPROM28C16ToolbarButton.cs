using CBaS_Core.Framework.UI;
using CBaS_Core.Logic.Resources;
using CBaS_Core.Logic.UI.Controls.EEPROMs;
using System.Windows.Input;

namespace CBaS_Core.Logic.UI.Toolbar_Buttons.EEPROMs
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