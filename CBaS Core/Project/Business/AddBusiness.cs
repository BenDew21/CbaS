using CBaSCore.Project.Storage;
using CBaSCore.Project.UI;
using CBaSCore.Project.UI.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CBaSCore.Project.Business
{
    public class AddBusiness
    {
        private AddStorage _storage = new AddStorage();


        public void OpenDialog()
        {
            BaseClassNode selectedNode = ProjectViewHandler.GetInstance().GetSelectedNode();
            if (selectedNode != null)
            {
                OpenNewItemWindow(selectedNode);
            }
            else
            {
                CreateNewProject();
            }
        }

        public void CreateNewProject()
        {
            NewItemWindow window = new NewItemWindow();
            if (window.ShowDialog() == true)
            {
                string projectPath = window.Path;
                string filePath = projectPath + @"\" + window.ItemName + ".CBaSP";
                Directory.CreateDirectory(projectPath);
                _storage.CreateDatabase(filePath);
                OpenBusiness open = new OpenBusiness();
                open.OpenFile(filePath, window.ItemName);
            }
        }

        public void OpenNewItemWindow(BaseClassNode selectedNode)
        {
            var parentID = selectedNode.NodeDetails.ID;
            var path = GetRelativePathToNode(selectedNode);
            var fullPath = ProjectViewHandler.GetInstance().GetPathToNode(selectedNode.NodeDetails);

            NewItemWindow window = new NewItemWindow(parentID, path);
            if (window.ShowDialog() == true)
            {
                StructureModel model = new StructureModel
                {
                    ParentID = parentID,
                    Name = window.ItemName,
                    Type = window.SelectedItem.Type,
                    FileExtension = window.FileExtension
                };

                if (window.SelectedItem.Type == ProjectItemEnum.Folder)
                {
                    Directory.CreateDirectory(window.FolderPath);
                }
                else if (window.SelectedItem.Type == ProjectItemEnum.Circuit || 
                         window.SelectedItem.Type == ProjectItemEnum.Module)
                {
                    var x = fullPath + path + @"\" + model.Name + model.FileExtension;
                    Debug.WriteLine(x);
                    File.Create(x);
                }

                model.ID = _storage.AddItem(model);
                ProjectViewHandler.GetInstance().CreateNode(model);
            }
        }

        public string GetRelativePathToNode(BaseClassNode node)
        {
            return ProjectViewHandler.GetInstance().GetRelativePathToNode(node) + @"\" + node.NodeDetails.Name;
        }
    }
}
