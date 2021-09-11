using System.Drawing;
using System.Windows.Input;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Project.Resources;
using CBaSCore.Project.Storage;

namespace CBaSCore.Project.UI.Nodes
{
    public class CircuitNode : BaseClassNode
    {
        private static readonly Bitmap Icon = Project_Resources.circuit_toolbar_icon;

        public CircuitNode(StructureModel nodeDetails) : base(nodeDetails, Icon)
        {
        }

        protected override void DoubleClickEvent(object sender, MouseEventArgs e)
        {
            TabHandler.GetInstance().AddTab(NodeDetails.ID, NodeDetails.Name);
        }
    }
}