using CBaSCore.Chip.Resources;
using CBaSCore.Framework.Resources;
using CBaSCore.Logic.Resources;
using CBaSCore.Project.Business;
using CBaSCore.Project.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBaSCore.Project.Storage.New
{
    public class NewItemModel
    {
        public IEnumerable<NewItemData> Items
        {
            get
            {
                yield return new NewItemData(Framework_Resources.new_toolbar_icon, "Circuit",
                    ProjectItemEnum.Circuit, "New Circuit", false, ".ccc");
                yield return new NewItemData(Project_Resources.binary_file_icon, "Binary File",
                    ProjectItemEnum.BinaryFile, "New Binary File", false, ".CBaSBin");
                yield return new NewItemData(Chip_Resources.Chip_Icon, "Module", 
                    ProjectItemEnum.Module, "New Module", false, ".CBaSM");
            }
        }
    }
}
