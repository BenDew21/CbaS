using System.Windows.Input;
using CBaSCore.Framework.Resources;

namespace CBaSCore.Framework.UI.Controls
{
    public class SaveToolbarItem : BaseToolbarItem
    {
        public SaveToolbarItem() : base(MenuName.File, Framework_Resources.Save, "Save Circuit", false)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.Save();
        }
    }
}