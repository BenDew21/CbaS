using System.Drawing;
using System.Windows.Input;
using CBaSCore.Chip.Resources;
using CBaSCore.Project.Storage;

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