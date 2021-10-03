using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.GitIntegration.Business;
using CBaSCore.Project.Resources;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;

namespace CBaSCore.GitIntegration.UI.Nodes
{
    public class BaseGitNode : TreeViewItem
    {
        public bool? Checked { get; set; }
        
        public string NodeName { get; set; }
        
        public bool IsCheckBoxThreeState { get; set; }
        
        public SimpleGitState Change { get; set; }

        private const int IconSize = 15;

        private readonly StackPanel _headerPanel = new();

        private readonly CheckBox _checkBox = new();
        
        private readonly Image _imageControl = new();

        private readonly Label _nameLabel = new();
        private BitmapSource _iconImage;

        private BitmapSource _iconImageExpanded;

        private Dictionary<SimpleGitState, SolidColorBrush> _textColourMap = new();

        public BaseGitNode()
        {
            _textColourMap.Add(SimpleGitState.NoChange, Brushes.Black);
            _textColourMap.Add(SimpleGitState.Created, Brushes.DarkGreen);
            _textColourMap.Add(SimpleGitState.Modified, Brushes.DarkBlue);
            _textColourMap.Add(SimpleGitState.Removed, Brushes.Red);
            
            Draw(NodeName, IsCheckBoxThreeState, Change);   
        }
        
        public BaseGitNode(string name, bool isCheckboxThreeState, SimpleGitState change)
        {
            _textColourMap.Add(SimpleGitState.NoChange, Brushes.Black);
            _textColourMap.Add(SimpleGitState.Created, Brushes.DarkGreen);
            _textColourMap.Add(SimpleGitState.Modified, Brushes.DarkBlue);
            _textColourMap.Add(SimpleGitState.Removed, Brushes.Red);
            
            NodeName = name;
            Change = change;
            IsCheckBoxThreeState = isCheckboxThreeState;
            
            Draw(name, isCheckboxThreeState, change);
        }

        private void Draw(string name, bool isCheckboxThreeState, SimpleGitState change)
        {
            _headerPanel.Orientation = Orientation.Horizontal;

            _checkBox.IsThreeState = isCheckboxThreeState;
            _checkBox.Width = IconSize;
            _checkBox.Height = IconSize;

            _checkBox.Checked += (sender, args) =>
            {
                Checked = _checkBox.IsChecked;
                if (Checked != true) return;
                if (!isCheckboxThreeState) return;
                foreach (var item in Items)
                {
                    ((BaseGitNode) item).Checked = Checked;
                }
            };

            _checkBox.Unchecked += (sender, args) =>
            {
                Checked = _checkBox.IsChecked;
                if (Checked != true) return;
                if (!isCheckboxThreeState) return;
                foreach (var item in Items)
                {
                    ((BaseGitNode) item).Checked = Checked;
                }
            };
            
            _headerPanel.Children.Add(_checkBox);

            if (change != SimpleGitState.NoChange)
            {
                var splitPath = name.Split(".");
                var extension = splitPath[^1];

                switch (extension)
                {
                    case ProjectFileExtensions.CircuitExtension:
                        _iconImage = UiIconConverter.BitmapToBitmapImage(Project_Resources.circuit_toolbar_icon);
                        break;
                    case ProjectFileExtensions.BinaryFileExtension:
                        _iconImage = UiIconConverter.BitmapToBitmapImage(Project_Resources.binary_file_icon);
                        break;
                    case ProjectFileExtensions.ProjectFileExtension:
                        _iconImage = UiIconConverter.BitmapToBitmapImage(Project_Resources.project_icon);
                        break;
                    default:
                        _iconImage = UiIconConverter.BitmapToBitmapImage(Project_Resources.new_toolbar_icon);
                        break;
                }
                
                _imageControl.Width = IconSize;
                _imageControl.Height = IconSize;
                _imageControl.Source = _iconImage;
                _imageControl.Margin = new Thickness(5, 0, 0, 0);
                _headerPanel.Children.Add(_imageControl);
            }

            _nameLabel.Content = name;
            _nameLabel.Foreground = _textColourMap[change];
            
            _headerPanel.Children.Add(_nameLabel);
            
            Header = _headerPanel;

            var toolTip = new ToolTip {Content = change.ToString()};
            ToolTip = change == SimpleGitState.NoChange ? null : toolTip;
        }
    }
}