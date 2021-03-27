using Combinatorics_Calculator.Project.Resources;
using Combinatorics_Calculator.Project.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Input;

namespace Combinatorics_Calculator.Project.UI.Nodes
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
