using CBaSCore.GitIntegration.Business;
using CBaSCore.Project.Storage;

namespace CBaSCore.GitIntegration.Storage
{
    public class GitNodeDetails : StructureModel
    {
        public SimpleGitState State { get; set; }

        public string NewFullPath { get; set; }
    }
}