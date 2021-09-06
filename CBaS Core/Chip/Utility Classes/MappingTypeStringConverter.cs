using CBaSCore.Chip.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBaSCore.Chip.Utility_Classes
{
    public class MappingTypeStringConverter
    {
        public static string EnumToString(MappingType mappingType)
        {
            string value = "";

            if (mappingType == MappingType.IO) value = "Input/Output";
            else value = mappingType.ToString();

            return value.Replace("_", " ");
        }
    }
}
