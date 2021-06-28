using CBaS_Core.Project.Resources;
using CBaS_Core.Project.Storage;
using System.Drawing;
using System.Windows.Input;

namespace CBaS_Core.Project.UI.Nodes
{
    public class BinaryFileNode : BaseClassNode
    {
        private static readonly Bitmap Icon = Project_Resources.binary_file_icon;

        public BinaryFileNode(StructureModel nodeDetails) : base(nodeDetails, Icon)
        {
        }

        protected override void DoubleClickEvent(object sender, MouseEventArgs e)
        {
            // TabHandler.GetInstance().AddTab(NodeDetails.ID, NodeDetails.Name);
        }
    }
}