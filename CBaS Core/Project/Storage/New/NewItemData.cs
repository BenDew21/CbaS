using CBaSCore.Project.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CBaSCore.Project.Storage.New
{
    public class NewItemData
    {
        private Bitmap icon;
        private string name;
        private ProjectItemEnum type;
        private string description;
        private bool hasPath;
        private string fileExtension;

        public Bitmap Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ProjectItemEnum Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool HasPath
        {
            get { return hasPath; }
            set { hasPath = value; }
        }

        public string FileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; }
        }

        public NewItemData(Bitmap icon, string name, ProjectItemEnum type, string description, bool hasPath, string fileExtension)
        {
            Icon = icon;
            Name = name;
            Type = type;
            Description = description;
            HasPath = hasPath;
            FileExtension = fileExtension;
        } 
    }
}
