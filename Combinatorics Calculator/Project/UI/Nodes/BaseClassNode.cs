using CBaS_Core.Framework.UI.Utility_Classes;
using CBaS_Core.Project.Business;
using CBaS_Core.Project.Storage;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CBaS_Core.Project.UI.Nodes
{
    public abstract class BaseClassNode : TreeViewItem
    {
        private readonly StackPanel _headerPanel = new StackPanel();

        private readonly System.Windows.Controls.Image _imageControl
            = new System.Windows.Controls.Image();

        private readonly Label _nameLabel = new Label();
        private BitmapSource _iconImage;

        private BitmapSource _iconImageExpanded;

        protected BaseClassNode(StructureModel nodeDetails, Bitmap iconName)
        {
            NodeDetails = nodeDetails;
            Draw(iconName);
            MouseRightButtonDown += RightClickEvent;
            MouseDoubleClick += DoubleClickEvent;
            CreateContextMenu();
        }

        public StructureModel NodeDetails { get; }

        protected void Draw(Bitmap icon)
        {
            _iconImage = UIIconConverter.BitmapToBitmapImage(icon);

            _headerPanel.Orientation = Orientation.Horizontal;

            _imageControl.Width = 15;
            _imageControl.Height = 15;
            _imageControl.Source = _iconImage;

            _nameLabel.Content = NodeDetails.Name;

            _headerPanel.Children.Add(_imageControl);
            _headerPanel.Children.Add(_nameLabel);

            Header = _headerPanel;
        }

        protected void SetExpandIcon(Bitmap iconExpanded)
        {
            _iconImageExpanded = UIIconConverter.BitmapToBitmapImage(iconExpanded);

            Expanded += (sender, args) => { _imageControl.Source = _iconImageExpanded; };
            Collapsed += (sender, args) => { _imageControl.Source = _iconImage; };
        }

        public void GenerateRenameHeader()
        {
            var renameTextBox = new TextBox();

            renameTextBox.Text = NodeDetails.Name;

            var panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            var renameImage = new System.Windows.Controls.Image();

            renameImage.Width = 15;
            renameImage.Height = 15;
            renameImage.Source = _imageControl.Source;

            panel.Children.Add(renameImage);
            panel.Children.Add(renameTextBox);

            Header = panel;
            renameTextBox.Focus();

            renameTextBox.KeyDown += (sender, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    var oldName = NodeDetails.Name;
                    NodeDetails.Name = renameTextBox.Text;

                    if (!oldName.Equals(NodeDetails.Name)) ProjectViewHandler.GetInstance().RenameItem(oldName, this);
                    ResetNodeHeader();
                }

                if (args.Key == Key.Escape) ResetNodeHeader();
            };
        }

        public void ResetNodeHeader()
        {
            _headerPanel.Children.Remove(_imageControl);
            _headerPanel.Children.Remove(_nameLabel);

            _imageControl.Width = 15;
            _imageControl.Height = 15;
            _imageControl.Source = _iconImage;

            _nameLabel.Content = NodeDetails.Name;

            _headerPanel.Children.Add(_imageControl);
            _headerPanel.Children.Add(_nameLabel);

            Header = _headerPanel;
        }

        protected virtual void RightClickEvent(object sender, MouseButtonEventArgs e)
        {
            Focus();
            e.Handled = true;
        }

        protected abstract void DoubleClickEvent(object sender, MouseEventArgs e);

        protected virtual void CreateContextMenu()
        {
            var renameItem = new MenuItem();
            renameItem.Header = "Rename";

            // renameItem.Command = new RenameCommand();
            renameItem.CommandParameter = this;

            var contextMenu = new ContextMenu();
            contextMenu.Items.Add(renameItem);

            ContextMenu = contextMenu;
        }
    }
}