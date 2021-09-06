using CBaSCore.Chip.Resources;
using CBaSCore.Project.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Input;

namespace CBaSCore.Project.UI.Nodes
{
    public class ModuleNode : BaseClassNode
    {
        private static readonly Bitmap Icon = Chip_Resources.Chip_Icon;

        public ModuleNode(StructureModel nodeDetails) : base(nodeDetails, Icon)
        {

        }

        protected override void DoubleClickEvent(object sender, MouseEventArgs e)
        {
             
        }
    }
}
