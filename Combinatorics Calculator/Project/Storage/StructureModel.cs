using Combinatorics_Calculator.Project.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Combinatorics_Calculator.Project.Storage
{
    public class StructureModel
    {
        private int id;
        private int parentID;
        private string name;
        private ProjectItemEnum type;
        private string fileExtension;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
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

        public string FileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; }
        }
    }
}
