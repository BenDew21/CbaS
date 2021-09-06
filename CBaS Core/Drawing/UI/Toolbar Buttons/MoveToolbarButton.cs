using CBaSCore.Drawing.Resources;
using CBaSCore.Framework.UI;
using System.Windows.Input;

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