using System.Windows.Input;
using CBaSCore.Drawing.Resources;
using CBaSCore.Drawing.UI.Controls;
using CBaSCore.Framework.UI;

namespace CBaSCore.Drawing.UI.Toolbar_Buttons
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