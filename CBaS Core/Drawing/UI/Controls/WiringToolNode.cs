using System.Drawing;
using CBaSCore.Logic.UI.Utility_Classes;
using CBaSCore.Project.Storage;

namespace CBaSCore.Drawing.UI.Controls
{
    /// <summary>
    /// ToolNode specific to wiring
    /// </summary>
    public class WiringToolNode : ToolNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodeDetails">Node details to store for the node</param>
        /// <param name="iconName">The Bitmap for the icon</param>
        public WiringToolNode(StructureModel nodeDetails, Bitmap iconName) : base(nodeDetails, iconName, null)
        {
            // Do nothing
        }

        /// <summary>
        /// When the node is selected
        /// </summary>
        public override void OnSelect()
        {
            WireStatus.GetInstance().SetSelected(true);
        }

        /// <summary>
        /// When the node is unselected 
        /// </summary>
        public override void OnUnselect()
        {
            WireStatus.GetInstance().SetSelected(false);
        }
    }
}