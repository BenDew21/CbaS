using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CBaSCore.GitIntegration.Business;
using CBaSCore.GitIntegration.UI.Nodes;
using CBaSCore.Project.Resources;

namespace CBaSCore.GitIntegration.UI
{
    public partial class GitDockContent : UserControl
    {
        private BaseGitNode _defaultChangelistNode;
        private BaseGitNode _unversionedFilesNode;
        
        public GitDockContent()
        {
            InitializeComponent();
            GitHandler.GetInstance().SetControl(this);
            
            GenerateNodes();
        }

        private void GenerateNodes()
        {
            var root = new BaseGitNode("Changes", true, SimpleGitState.NoChange);
            _defaultChangelistNode = new BaseGitNode("Default Changelist", true, SimpleGitState.NoChange);
            _unversionedFilesNode = new BaseGitNode("Unversioned Files", true, SimpleGitState.NoChange);
            root.Items.Add(_defaultChangelistNode);
            root.Items.Add(_unversionedFilesNode);
            TreeViewChanges.Items.Add(root);
        }
        
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
        }

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
            
            GitHandler.GetInstance().CommitChanges(changedNames, TextBoxCommitMessage.Text, shouldPush);
        }

        private void CommitButton_OnClick(object sender, RoutedEventArgs e)
        {
            CommitChanges(false);
        }
        
        private void CommitAndPushButton_OnClick(object sender, RoutedEventArgs e)
        {
            CommitChanges(true);
        }
    }
}