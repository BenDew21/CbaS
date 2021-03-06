using Combinatorics_Calculator.Drawing.Resources;
using Combinatorics_Calculator.Drawing.UI.Controls;
using Combinatorics_Calculator.Framework.UI;
using System.Windows.Input;

namespace Combinatorics_Calculator.Drawing.UI.Toolbar_Buttons
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