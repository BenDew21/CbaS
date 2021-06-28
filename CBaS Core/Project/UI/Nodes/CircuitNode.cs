using CBaS_Core.Framework.UI.Handlers;
using CBaS_Core.Project.Resources;
using CBaS_Core.Project.Storage;
using System.Drawing;
using System.Windows.Input;

namespace CBaS_Core.Project.UI.Nodes
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