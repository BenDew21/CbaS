using Combinatorics_Calculator.Drawing.Resources;
using Combinatorics_Calculator.Framework.UI;
using System.Windows.Input;

namespace Combinatorics_Calculator.Drawing.UI.Toolbar_Buttons
{
    public class MoveToolbarButton : BaseToolbarItem
    {
        public MoveToolbarButton() : base(MenuName.Drawing, Drawing_Resources.Move, "Move Control", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.DragPressed(IsSelected);
        }
    }
}