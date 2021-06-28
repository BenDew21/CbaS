using CBaS_Core.Project.Storage;
using CBaS_Core.Project.UI.Nodes;

namespace CBaS_Core.Project.Business
{
    public static class ProjectBuilder
    {
        public static BaseClassNode BuildNode(StructureModel model)
        {
            switch (model.Type)
            {
                case ProjectItemEnum.Circuit:
                    return new CircuitNode(model);

                case ProjectItemEnum.Folder:
                    return new FolderNode(model);

                case ProjectItemEnum.ProjectNode:
                    return new ProjectNode(model);

                case ProjectItemEnum.BinaryFile:
                    return new BinaryFileNode(model);
            }
            return null;
        }
    }
}