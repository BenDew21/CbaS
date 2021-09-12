using System.Drawing;
using System.Windows.Input;
using CBaSCore.Drawing.UI.Handlers;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Project.Storage;
using CBaSCore.Project.UI.Nodes;

namespace CBaSCore.Drawing.UI.Controls
{
    /// <summary>
    /// BaseClassNode for all Toolbox items
    /// </summary>
    public class ToolNode : BaseClassNode
    {
        /// <summary>
        /// Interface reference to the item this node represents
        /// </summary>
        private readonly IToolboxItem _item;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodeDetails">Node details to store for the node</param>
        /// <param name="iconName">The Bitmap for the icon</param>
        /// <param name="item">The item this node represents</param>
        public ToolNode(StructureModel nodeDetails, Bitmap iconName, IToolboxItem item) : base(nodeDetails, iconName)
        {
            _item = item;
        }

        /// <summary>
        /// When the node is selected
        /// </summary>
        public virtual void OnSelect()
        {
            ToolbarEventHandler.GetInstance().CanvasButtonPressed(_item.GetSelectedItem());
        }

        /// <summary>
        /// When the node is unselected 
        /// </summary>
        public virtual void OnUnselect()
        {
            ToolbarEventHandler.GetInstance().CanvasButtonPressed(null);
        }

        /// <summary>
        /// Double click event handler for node
        /// </summary>
        /// <param name="sender">Object which raised the event</param>
        /// <param name="e">Event args</param>
        protected override void DoubleClickEvent(object sender, MouseEventArgs e)
        {
            // Intentionally left blank
        }
    }
}