using CBaS_Core.Framework.UI;
using CBaS_Core.Project.Business;
using CBaS_Core.Project.Resources;
using Microsoft.Win32;
using System.Windows.Input;

namespace CBaS_Core.Project.UI
{
    public class OpenProjectToolbarButton : BaseToolbarItem
    {
        private readonly OpenBusiness business = new OpenBusiness();

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