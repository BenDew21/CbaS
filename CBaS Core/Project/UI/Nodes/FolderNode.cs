using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using CBaSCore.Project.Resources;
using CBaSCore.Project.Storage;

namespace CBaSCore.Project.UI.Nodes
{
    public class FolderNode : BaseClassNode
    {
        private static readonly Bitmap OpenedIcon = Project_Resources.open_folder_icon;

        private static readonly Bitmap ClosedIcon = Project_Resources.closed_folder_icon;

        public FolderNode(StructureModel nodeDetails) : base(nodeDetails, ClosedIcon)
        {
            SetExpandIcon(OpenedIcon);
        }

        protected override void RightClickEvent(object sender, MouseButtonEventArgs e)
        {
            Focus();
            e.Handled = true;
        }

        protected override void DoubleClickEvent(object sender, MouseEventArgs e)
        {
        }

        protected override void CreateContextMenu()
        {
            base.CreateContextMenu();
            var contextMenu = ContextMenu;

            var addItem = new MenuItem();
            addItem.Header = "Add";

            // addItem.Command = new AddCommand();
            addItem.CommandParameter = this;

            contextMenu.Items.Add(addItem);
            ContextMenu = contextMenu;
        }
    }
}