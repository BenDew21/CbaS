using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CBaSCore.GitIntegration.Business;
using CBaSCore.GitIntegration.UI.Nodes;

namespace CBaSCore.GitIntegration.UI
{
    /// <summary>
    /// GitDockContent - Controller for Git integration control
    /// </summary>
    public partial class GitDockContent : UserControl
    {
        /// <summary>
        /// Node storing default changelist
        /// </summary>
        private BaseGitNode _defaultChangelistNode;
        
        /// <summary>
        /// Node storing unversioned files
        /// </summary>
        private BaseGitNode _unversionedFilesNode;
        
        /// <summary>
        /// Constructor - set up the control
        /// </summary>
        public GitDockContent()
        {
            InitializeComponent();
            GitHandler.GetInstance().SetControl(this);
            
            GenerateNodes();
        }

        /// <summary>
        /// Generate the constant nodes in the control
        /// </summary>
        private void GenerateNodes()
        {
            var root = new BaseGitNode("Changes", true, SimpleGitState.NoChange);
            _defaultChangelistNode = new BaseGitNode("Default Changelist", true, SimpleGitState.NoChange);
            _unversionedFilesNode = new BaseGitNode("Unversioned Files", true, SimpleGitState.NoChange);
            root.Items.Add(_defaultChangelistNode);
            root.Items.Add(_unversionedFilesNode);
            TreeViewChanges.Items.Add(root);
        }
        
        /// <summary>
        /// Update the change nodes from a provided list of default changes and unversioned files
        /// </summary>
        /// <param name="defaultChangeList">The new default changelist</param>
        /// <param name="unversionedFiles">The new unversioned files</param>
        public void UpdateChanges(List<BaseGitNode> defaultChangeList, List<BaseGitNode> unversionedFiles)
        {
            _defaultChangelistNode.Items.Clear();
            foreach (var item in defaultChangeList)
            {
                _defaultChangelistNode.Items.Add(item);
            }
            
            _unversionedFilesNode.Items.Clear();
            foreach (var item in unversionedFiles)
            {
                _unversionedFilesNode.Items.Add(item);
            }
            
            // Clear the commit message
            TextBoxCommitMessage.Text = "";
        }

        /// <summary>
        /// Commit the changes
        /// </summary>
        /// <param name="shouldPush">Whether the changes should be pushed</param>
        private void CommitChanges(bool shouldPush)
        {
            var changedNames = new List<string>();
            foreach (var node in _defaultChangelistNode.Items)
            {
                if (node is BaseGitNode gitNode && gitNode.Checked != false)
                {
                    changedNames.Add(gitNode.NodeName);
                }
            }
            
            foreach (var node in _unversionedFilesNode.Items)
            {
                if (node is BaseGitNode gitNode && gitNode.Checked != false)
                {
                    changedNames.Add(gitNode.NodeName);
                }
            }
            
            // Make sure the commit message is clean from whitespace
            var cleanText = TextBoxCommitMessage.Text.TrimEnd();
            GitHandler.GetInstance().CommitChanges(changedNames, cleanText, shouldPush);
        }

        /// <summary>
        /// When the Commit button is pressed
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Clicked event args</param>
        private void CommitButton_OnClick(object sender, RoutedEventArgs e)
        {
            CommitChanges(false);
        }
        
        /// <summary>
        /// When the Commit & Push button is pressed
        /// </summary>
        /// <param name="sender">The calling object</param>
        /// <param name="e">Clicked event args</param>
        private void CommitAndPushButton_OnClick(object sender, RoutedEventArgs e)
        {
            CommitChanges(true);
        }
    }
}