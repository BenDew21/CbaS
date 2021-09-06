using CBaSCore.Chip.Storage;
using CBaSCore.Chip.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CBaSCore.Chip.UI.Controls
{
    public class MappingTypeComboBoxColumn : DataGridComboBoxColumn
    {
        public MappingTypeComboBoxColumn()
        {
            List<string> names = new List<string>();
            foreach (var name in Enum.GetValues(typeof(MappingType)))
            {
                names.Add(MappingTypeStringConverter.EnumToString((MappingType)name));
            }

            ItemsSource = names;
        }
    }
}
