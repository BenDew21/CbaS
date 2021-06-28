﻿using CBaS_Core.Framework.Resources;
using System.Windows.Input;

namespace CBaS_Core.Framework.UI.Controls
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