using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Project.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CBaSCore.Chip.Storage
{
    public class Module
    {
        public string Path { get; set; }

        public Circuit LinkedCircuit { get; set; }

        public List<PinMapping> OutputMappings { get; set; }

        public List<PinMapping> InputMappings { get; set; }

        public Dictionary<int, MappingType> PinMappings { get; set; }

        public void Save()
        {
            LinkedCircuit.Save();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            if (String.IsNullOrEmpty(Path)) Path = @"C:\Programming\Test.CBaS";

            using (XmlWriter writer = XmlWriter.Create(Path, settings))
            {
                // <Module>
                writer.WriteStartElement(SaveLoadTags.MODULE_NODE);

                // <LinkedCircuit>ID</LinkedCircuit>
                writer.WriteElementString(SaveLoadTags.LINKED_CIRCUIT, LinkedCircuit.ID.ToString());

                // </Module>
                writer.WriteEndElement();
            }
        }
    }
}
