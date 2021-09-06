using CBaSCore.Framework.Resources;
using CBaSCore.Project.Business;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CBaSCore.Framework.UI.Controls
{
    public class NewToolbarButton : BaseToolbarItem
    {
        private AddBusiness _business = new AddBusiness();

        public NewToolbarButton() : base(MenuName.File, Framework_Resources.new_toolbar_icon, "New", false)
        {

        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            _business.OpenDialog();
        }
    }
}
