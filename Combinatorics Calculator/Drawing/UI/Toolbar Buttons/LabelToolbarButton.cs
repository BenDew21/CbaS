using CBaS_Core.Drawing.Resources;
using CBaS_Core.Drawing.UI.Controls;
using CBaS_Core.Framework.UI;
using System.Windows.Input;

namespace CBaS_Core.Drawing.UI.Toolbar_Buttons
{
    public class LabelToolbarButton : BaseToolbarItem
    {
        public LabelToolbarButton() : base(MenuName.Drawing, Drawing_Resources.Output, "Label", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new DiagramLabel());
        }
    }
}