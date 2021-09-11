using System.Drawing;
using CBaSCore.Project.Business;

namespace CBaSCore.Project.Storage.New
{
    public class NewItemData
    {
        public NewItemData(Bitmap icon, string name, ProjectItemEnum type, string description, bool hasPath, string fileExtension)
        {
            Icon = icon;
            Name = name;
            Type = type;
            Description = description;
            HasPath = hasPath;
            FileExtension = fileExtension;
        }

        public Bitmap Icon { get; set; }

        public string Name { get; set; }

        public ProjectItemEnum Type { get; set; }

        public string Description { get; set; }

        public bool HasPath { get; set; }

        public string FileExtension { get; set; }
    }
}