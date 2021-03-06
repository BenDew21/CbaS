﻿using Combinatorics_Calculator.Framework.Business;
using Combinatorics_Calculator.Project.Storage;
using Combinatorics_Calculator.Project.UI.Nodes;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Combinatorics_Calculator.Project.Business
{
    public class ProjectViewHandler
    {
        private static ProjectViewHandler _instance;

        private readonly Dictionary<int, BaseClassNode> treeList = new Dictionary<int, BaseClassNode>();

        private string _projectDirectory;

        private string _projectName;

        private TreeView _treeView;

        // private readonly List<ITreeViewObserver> observers = new List<ITreeViewObserver>();

        private ProjectNode parentNode;

        public static ProjectViewHandler GetInstance()
        {
            if (_instance == null) _instance = new ProjectViewHandler();
            return _instance;
        }

        private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // foreach (var obs in observers) obs.TreeViewItemSelectionChanged((BaseClassNode)e.NewValue);
        }

        public void SetTreeView(TreeView treeView)
        {
            _treeView = treeView;
            _treeView.SelectedItemChanged += TreeViewOnSelectedItemChanged;
        }

        public void SetProjectDirectory(string directory)
        {
            _projectDirectory = directory;
        }

        public string GetProjectDirectory()
        {
            return _projectDirectory;
        }

        public void SetProjectName(string name)
        {
            _projectName = name;
        }

        //public void RegisterObserver(ITreeViewObserver observer)
        //{
        //    observers.Add(observer);
        //}

        public void CreateNode(StructureModel model)
        {
            BaseClassNode node = ProjectBuilder.BuildNode(model);

            if (model.ParentID == 0)
            {
                parentNode.Items.Add(node);
            }
            else
            {
                treeList[model.ParentID].Items.Add(node);
            }

            treeList.Add(node.NodeDetails.ID, node);
        }

        private BaseClassNode GetFolderNode(BaseClassNode node, int id)
        {
            if (node.NodeDetails.ID == id)
            {
                return node;
            }
            foreach (var item in node.Items)
            {
                var castedItem = item as BaseClassNode;
                BaseClassNode returnValue = GetFolderNode(castedItem, id);
                if (returnValue != null)
                {
                    return returnValue;
                }
            }
            return null;
        }

        /// <summary>
        /// Generate the project view in the application
        /// </summary>
        /// <param name="model">The model imported from the database</param>
        public void GenerateView(List<StructureModel> model)
        {
            // Create the parent node first
            parentNode = new ProjectNode(new StructureModel
            {
                ID = 0,
                ParentID = 0,
                Name = _projectName,
                FileExtension = "",
                Type = ProjectItemEnum.ProjectNode
            });

            // Generate the folders -  this is done first to make sure folders appear at the top of each directory
            foreach (var item in model.OrderBy(t => t.ParentID).Where(t => t.Type.ToString().Equals("Folder")))
            {
                // Build the node
                var node = ProjectBuilder.BuildNode(item);
                // If the parent ID is the root node (the project node) then add it here
                if (item.ParentID == 0)
                {
                    parentNode.Items.Add(node);
                }
                else
                {
                    // The parent is a folder, so get the folder and add it
                    var baseNode = treeList[item.ParentID];
                    baseNode.Items.Add(node);
                }
                // Add the item to the tree list
                treeList.Add(item.ID, node);
            }

            // Generate the items
            foreach (var item in model.Where(s => !s.Type.ToString().Equals("Folder")))
            {
                BaseClassNode node = ProjectBuilder.BuildNode(item);
                if (item.ParentID == 0)
                {
                    parentNode.Items.Add(node);
                }
                else
                {
                    var baseNode = treeList[item.ParentID];
                    baseNode.Items.Add(node);
                }
                treeList.Add(item.ID, node);

                if (node is CircuitNode)
                {
                    string path = GetPathToNode(item) + @"\" + item.Name + item.FileExtension;
                    CircuitHandler.GetInstance().LoadCircuit(item.ID, path);
                }
            }

            // Refresh the items in the view
            _treeView.Items.Clear();
            _treeView.Items.Add(parentNode);
        }

        public StructureModel GetNodeDetails(int id)
        {
            return treeList[id].NodeDetails;
        }

        public void RenameItem(string oldName, BaseClassNode node)
        {
            bool shouldUpdate = true;

            var path = GetPathToNode(node.NodeDetails);

            try
            {
                // If the file extension is empty, rename a directory
                if (string.IsNullOrEmpty(node.NodeDetails.FileExtension))
                {
                    Directory.Move(path + @"\" + oldName, path + @"\" + node.NodeDetails.Name);
                }
                else
                {
                    var pathToOld = path + @"\" + oldName + node.NodeDetails.FileExtension;
                    var pathToNew = path + @"\" + node.NodeDetails.Name + node.NodeDetails.FileExtension;

                    Directory.Move(pathToOld, pathToNew);
                }
            }
            catch (IOException)
            {
                shouldUpdate = false;
            }

            if (shouldUpdate)
            {
                using (var connection = DatabaseHandler.GetInstance().GetConnection())
                {
                    connection.Open();
                    var sql = "UPDATE PROJECTSTRUCTURE SET NAME = @name WHERE ID = @id";

                    using (var command = new SqliteCommand(sql, connection))
                    {
                        var nameParameter = new SqliteParameter("@name", node.NodeDetails.Name);
                        var idParameter = new SqliteParameter("@id", node.NodeDetails.ID);

                        command.Parameters.Add(nameParameter);
                        command.Parameters.Add(idParameter);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public string GetPathToNode(StructureModel node)
        {
            IList<string> pathItems = new List<string>();

            if (node.ParentID == 0) return _projectDirectory;

            var parentID = node.ParentID;
            while (parentID != 0)
            {
                var parentNode = treeList[parentID];
                parentID = parentNode.NodeDetails.ParentID;
                pathItems.Add(parentNode.NodeDetails.Name);
            }

            var fullPath = _projectDirectory;
            foreach (var item in pathItems.Reverse()) fullPath += @"\" + item;

            return fullPath;
        }

        /// <summary>
        /// Get the path to a node, not including the project name
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The path as a string</returns>
        public string GetRelativePathToNode(BaseClassNode node)
        {
            IList<string> pathItems = new List<string>();

            var parent = VisualTreeHelper.GetParent(node);

            if (node.NodeDetails.ParentID == 0) return "";

            var parentID = node.NodeDetails.ParentID;
            while (parentID != 0)
            {
                var parentNode = treeList[parentID];
                parentID = parentNode.NodeDetails.ParentID;
                pathItems.Add(parentNode.NodeDetails.Name);
            }

            var path = "";

            foreach (var item in pathItems.Reverse()) path += @"\" + item;

            return path;
        }
    }
}