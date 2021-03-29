using Combinatorics_Calculator.Project.Business;

namespace Combinatorics_Calculator.Project.Storage
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