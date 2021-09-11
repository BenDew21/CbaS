using System.Windows.Input;
using CBaSCore.Framework.Resources;
using CBaSCore.Project.Business;

namespace CBaSCore.Framework.UI.Controls
{
    public class NewToolbarButton : BaseToolbarItem
    {
        private readonly AddBusiness _business = new();

        public NewToolbarButton() : base(MenuName.File, Framework_Resources.new_toolbar_icon, "New", false)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            _business.OpenDialog();
        }
    }
}