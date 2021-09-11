using System.Diagnostics;
using System.IO;
using CBaSCore.Project.Storage;
using CBaSCore.Project.UI;
using CBaSCore.Project.UI.Nodes;

namespace CBaSCore.Project.Business
{
    public class AddBusiness
    {
        private readonly AddStorage _storage = new();


        public void OpenDialog()
        {
            var selectedNode = ProjectViewHandler.GetInstance().GetSelectedNode();
            if (selectedNode != null)
                OpenNewItemWindow(selectedNode);
            else
                CreateNewProject();
        }

        public void CreateNewProject()
        {
            var window = new NewItemWindow();
            if (window.ShowDialog() == true)
            {
                var projectPath = window.Path;
                var filePath = projectPath + @"\" + window.ItemName + ".CBaSP";
                Directory.CreateDirectory(projectPath);
                _storage.CreateDatabase(filePath);
                var open = new OpenBusiness();
                open.OpenFile(filePath, window.ItemName);
            }
        }

        public void OpenNewItemWindow(BaseClassNode selectedNode)
        {
            var parentID = selectedNode.NodeDetails.ID;
            var path = GetRelativePathToNode(selectedNode);
            var fullPath = ProjectViewHandler.GetInstance().GetPathToNode(selectedNode.NodeDetails);

            var window = new NewItemWindow(parentID, path);
            if (window.ShowDialog() == true)
            {
                var model = new StructureModel
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