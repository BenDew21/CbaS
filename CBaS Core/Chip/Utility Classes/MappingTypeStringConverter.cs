using CBaSCore.Chip.Storage;

namespace CBaSCore.Chip.Utility_Classes
{
    public class MappingTypeStringConverter
    {
        public static string EnumToString(MappingType mappingType)
        {
            var value = "";

            if (mappingType == MappingType.IO) value = "Input/Output";
            else value = mappingType.ToString();

            return value.Replace("_", " ");
        }
    }
}