﻿using System.Windows.Input;
using CBaSCore.Framework.UI;
using CBaSCore.Project.Business;
using CBaSCore.Project.Resources;
using Microsoft.Win32;

namespace CBaSCore.Project.UI
{
    public class OpenProjectToolbarButton : BaseToolbarItem
    {
        private readonly OpenBusiness business = new();

        public OpenProjectToolbarButton() : base(MenuName.File, Project_Resources.open_toolbar_icon, "Open Project", false)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) business.OpenFile(openFileDialog.FileName, openFileDialog.SafeFileName);
        }
    }
}