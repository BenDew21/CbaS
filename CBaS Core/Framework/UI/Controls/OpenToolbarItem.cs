﻿using CBaSCore.Framework.Resources;
using System.Windows.Input;

namespace CBaSCore.Framework.UI.Controls
{
    public class OpenToolbarItem : BaseToolbarItem
    {
        public OpenToolbarItem() : base(MenuName.File, Framework_Resources.Open, "Open Circuit", false)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            // toolbarEventHandler.Load();
        }
    }
}