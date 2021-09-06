using CBaSCore.Project.Business;
using CBaSCore.Project.Storage.New;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CBaSCore.Project.UI
{
    /// <summary>
    /// Interaction logic for NewItemWindow.xaml
    /// </summary>
    public partial class NewItemWindow : Window
    {
        private readonly int _parentID;
        private readonly bool _isNewProject = false;

        private string fileExtension;
        private NewItemData selectedItem;
        private string path;
        private string selectedItemPath;
        private string itemName;
        private string folderPath;


        public string FileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; }
        }

        public NewItemData SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string SelectedItemPath
        {
            get { return selectedItemPath; }
            set { selectedItemPath = value; }
        }

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        public NewItemWindow()
        {
            InitializeComponent();
            DataContext = new NewItemModel();
            _isNewProject = true;

            TextBoxName.IsEnabled = false;
            TextBoxName.Background = null;
            TextBoxProjectPath.IsReadOnly = false;
            TextBoxProjectPath.Background = null;

            ListBoxItem.SelectionChanged += (sender, args) =>
            {
                SelectedItem = ListBoxItem.SelectedItem as NewItemData;
                TextBlockDescription.Text = SelectedItem.Description;
            };

            ListBoxItem.SelectedItem = 0;
        }

        public NewItemWindow(int parentID, string path)
        {
            InitializeComponent();
            DataContext = new NewItemModel();

            ListBoxItem.SelectionChanged += (sender, args) =>
            {
                SelectedItem = ListBoxItem.SelectedItem as NewItemData;
                FileExtension = SelectedItem.FileExtension;
                TextBlockDescription.Text = SelectedItem.Description;
                if (SelectedItem.HasPath)
                {
                    TextBoxFilePath.IsReadOnly = false;
                    TextBoxFilePath.Background = null;
                    ButtonBrowse.IsEnabled = true;
                }
                else if (SelectedItem.Type == ProjectItemEnum.ProjectNode)
                {
                    TextBoxFilePath.IsReadOnly = false;
                    TextBoxFilePath.Background = null;
                    ButtonBrowse.IsEnabled = false;
                }
                else
                {
                    TextBoxFilePath.IsReadOnly = true;
                    TextBoxFilePath.Background = Brushes.DarkGray;
                    ButtonBrowse.IsEnabled = false;
                    TextBoxFilePath.Text = "";
                }
            };

            TextBoxProjectPath.Text = path;
            _parentID = parentID;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!_isNewProject)
            {
                if (_parentID == 0)
                {
                    Path = ProjectViewHandler.GetInstance().GetProjectDirectory() + @"\" + TextBoxName.Text + FileExtension;
                    FolderPath = ProjectViewHandler.GetInstance().GetProjectDirectory() + @"\" + TextBoxName.Text;
                }
                else
                {
                    Path = ProjectViewHandler.GetInstance().GetProjectDirectory() + @"\" + TextBoxProjectPath.Text
                    + @"\" + TextBoxName.Text + FileExtension;

                    FolderPath = ProjectViewHandler.GetInstance().GetProjectDirectory() + @"\" + TextBoxProjectPath.Text
                                + @"\" + TextBoxName.Text;
                }
            }

            SelectedItemPath = TextBoxFilePath.Text;
            ItemName = TextBoxName.Text;
            DialogResult = true;
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (_isNewProject)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "ASIS Project File (*.APF)|*.APF";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string path = saveFileDialog.FileName;
                    string actualPath = path.Split('.')[0];
                    Path = actualPath;
                    TextBoxFilePath.Text = actualPath;
                    TextBoxName.Text = saveFileDialog.SafeFileName.Split('.')[0];
                }
            }
            else
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg|Bitmap Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png|All Files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    TextBoxFilePath.Text = openFileDialog.FileName;

                    string[] splitFilePath = openFileDialog.FileName.Split('/');
                    string nameWithFilePath = splitFilePath[splitFilePath.Length - 1];
                    FileExtension = "." + nameWithFilePath.Split('.')[1];
                    // If the name text box is empty, then use the original file name
                    if (TextBoxName.Text.Length == 0)
                    {
                        TextBoxName.Text = openFileDialog.SafeFileName;
                    }
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
