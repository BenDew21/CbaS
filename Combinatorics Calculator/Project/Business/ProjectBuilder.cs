using Combinatorics_Calculator.Project.Storage;
using Combinatorics_Calculator.Project.UI.Nodes;

namespace Combinatorics_Calculator.Project.Business
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
            }
            return null;
        }
    }
}