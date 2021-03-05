using Combinatorics_Calculator.Project.Resources;
using Combinatorics_Calculator.Project.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Combinatorics_Calculator.Project.UI.Nodes
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
