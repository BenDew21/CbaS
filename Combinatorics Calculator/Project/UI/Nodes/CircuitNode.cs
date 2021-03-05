using Combinatorics_Calculator.Project.Resources;
using Combinatorics_Calculator.Project.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Input;

namespace Combinatorics_Calculator.Project.UI.Nodes
{
    public class CircuitNode : BaseClassNode
    {
        private static readonly Bitmap Icon = Project_Resources.circuit_toolbar_icon;

        public CircuitNode(StructureModel nodeDetails) : base(nodeDetails, Icon) {}

        protected override void DoubleClickEvent(object sender, MouseEventArgs e)
        {
            // throw new NotImplementedException();
        }
    }
}
