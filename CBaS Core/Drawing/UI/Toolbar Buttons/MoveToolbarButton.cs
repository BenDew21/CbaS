using System.Windows.Input;
using CBaSCore.Drawing.Resources;
using CBaSCore.Framework.UI;

namespace CBaSCore.Drawing.UI.Toolbar_Buttons
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