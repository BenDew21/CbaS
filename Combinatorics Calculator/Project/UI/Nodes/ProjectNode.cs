using Combinatorics_Calculator.Project.Resources;
using Combinatorics_Calculator.Project.Storage;
using System.Windows.Controls;
using System.Windows.Input;

namespace Combinatorics_Calculator.Project.UI.Nodes
{
    public class ProjectNode : BaseClassNode
    {
        public ProjectNode(StructureModel nodeDetails) : base(nodeDetails, Project_Resources.project_icon)
        {
        }

        protected override void RightClickEvent(object sender, MouseButtonEventArgs e)
        {
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