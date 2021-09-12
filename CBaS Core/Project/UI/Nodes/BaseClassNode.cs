using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Project.Business;
using CBaSCore.Project.Storage;
using Image = System.Windows.Controls.Image;

namespace CBaSCore.Project.UI.Nodes
{
    public abstract class BaseClassNode : TreeViewItem
    {
        private const int IconSize = 25;

        private readonly StackPanel _headerPanel = new();

        private readonly Image _imageControl = new();

        private readonly Label _nameLabel = new();
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

        private void Draw(Bitmap icon)
        {
            _iconImage = UiIconConverter.BitmapToBitmapImage(icon);

            _headerPanel.Orientation = Orientation.Horizontal;

            _imageControl.Width = IconSize;
            _imageControl.Height = IconSize;
            _imageControl.Source = _iconImage;

            _nameLabel.Content = NodeDetails.Name;

            _headerPanel.Children.Add(_imageControl);
            _headerPanel.Children.Add(_nameLabel);

            Header = _headerPanel;
        }

        protected void SetExpandIcon(Bitmap iconExpanded)
        {
            _iconImageExpanded = UiIconConverter.BitmapToBitmapImage(iconExpanded);

            Expanded += (sender, args) => { _imageControl.Source = _iconImageExpanded; };
            Collapsed += (sender, args) => { _imageControl.Source = _iconImage; };
        }

        public void GenerateRenameHeader()
        {
            var renameTextBox = new TextBox { Text = NodeDetails.Name };
            var panel = new StackPanel { Orientation = Orientation.Horizontal };
            var renameImage = new Image { Width = IconSize, Height = IconSize, Source = _imageControl.Source };
            
            panel.Children.Add(renameImage);
            panel.Children.Add(renameTextBox);

            Header = panel;
            renameTextBox.Focus();

            renameTextBox.KeyDown += (sender, args) =>
            {
                switch (args.Key)
                {
                    case Key.Enter:
                    {
                        var oldName = NodeDetails.Name;
                        NodeDetails.Name = renameTextBox.Text;

                        if (!oldName.Equals(NodeDetails.Name)) ProjectViewHandler.GetInstance().RenameItem(oldName, this);
                        ResetNodeHeader();
                        break;
                    }
                    case Key.Escape:
                        ResetNodeHeader();
                        break;
                }
            };
        }

        private void ResetNodeHeader()
        {
            _headerPanel.Children.Remove(_imageControl);
            _headerPanel.Children.Remove(_nameLabel);

            _imageControl.Width = IconSize;
            _imageControl.Height = IconSize;
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
            var renameItem = new MenuItem { Header = "Rename", CommandParameter = this };
            
            // TODO: Re-add Renaming Nodes
            // renameItem.Command = new RenameCommand();
            var contextMenu = new ContextMenu();
            contextMenu.Items.Add(renameItem);

            ContextMenu = contextMenu;
        }
    }
}