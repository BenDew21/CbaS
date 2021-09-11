using System;
using System.Collections.Generic;
using System.Windows.Controls;
using CBaSCore.Chip.Storage;
using CBaSCore.Chip.Utility_Classes;

namespace CBaSCore.Chip.UI.Controls
{
    public class MappingTypeComboBoxColumn : DataGridComboBoxColumn
    {
        public MappingTypeComboBoxColumn()
        {
            var names = new List<string>();
            foreach (var name in Enum.GetValues(typeof(MappingType))) names.Add(MappingTypeStringConverter.EnumToString((MappingType) name));

            ItemsSource = names;
        }
    }
}