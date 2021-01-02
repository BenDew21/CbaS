using Combinatorics_Calculator.Framework.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Combinatorics_Calculator.Framework.UI.Controls
{
    public class SaveToolbarItem : BaseToolbarItem
    {
        public SaveToolbarItem() : base(MenuName.File, Framework_Resources.Save, "Save Circuit", false) { }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.Save();
        }
    }
}
