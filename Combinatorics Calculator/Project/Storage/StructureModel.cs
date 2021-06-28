using CBaS_Core.Project.Business;

namespace CBaS_Core.Project.Storage
{
    public class StructureModel
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string Name { get; set; }

        public ProjectItemEnum Type { get; set; }

        public string FileExtension { get; set; }

        public string FullPath { get; set; }
    }
}