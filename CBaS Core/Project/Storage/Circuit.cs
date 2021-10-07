using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Framework.UI.Controls;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.Business;
using CBaSCore.Logic.UI.Controls;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Project.Storage
{
    public class Circuit
    {
        public Circuit()
        {
            Wires = new Dictionary<int, Wire>();
            Elements = new List<ICanvasElement>();

            WireIterator = 0;
        }

        public Circuit(XElement document) : this()
        {
            Build(document);
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Dictionary<int, Wire> Wires { get; set; }
        public List<ICanvasElement> Elements { get; set; }
        public int WireIterator { get; set; }

        public void Build(XElement document)
        {
            foreach (var value in from c in document.Descendants("Wire") select c)
            {
                var id = Convert.ToInt32(value.Element("ID").Value);

                var wire = new Wire();
                (wire.GetBusiness() as WireBusiness).ID = id;
                wire.SetStart(Convert.ToInt32(value.Element("X1").Value), Convert.ToInt32(value.Element("Y1").Value));
                wire.SetEnd(Convert.ToInt32(value.Element("X2").Value), Convert.ToInt32(value.Element("Y2").Value));

                Wires.Add(id, wire);
                WireIterator = id;
            }

            foreach (var value in from c in document.Descendants("WireLinks").Descendants("WireLink") select c)
            {
                var parentID = Convert.ToInt32(value.Element("ParentID").Value);
                var parentWire = Wires[parentID];

                foreach (var link in from links in value.Descendants("Links").Descendants("Link") select links)
                {
                    var id = Convert.ToInt32(link.Value);
                    var childWire = Wires[id];
                    parentWire.AddOutputWire(childWire);
                }
            }

            var factory = new CanvasElementFactory();

            foreach (var value in from c in document.Descendants("CanvasElements").Descendants("CanvasElement") select c)
            {
                var inputWires = new Dictionary<int, Wire>();
                var outputWires = new Dictionary<int, Wire>();

                var element = factory.CreateFromName(value.Element("Type").Value);

                foreach (var inputWireDetail in from iw in value.Descendants("InputWires").Descendants("WireDetail") select iw)
                {
                    var input = Convert.ToInt32(inputWireDetail.Element("Input").Value);
                    var wire = Wires[Convert.ToInt32(inputWireDetail.Element("WireID").Value)];
                    wire.RegisterWireObserver((IWireObserver) element);
                    inputWires.Add(input, wire);
                }

                foreach (var outputWireDetail in from ow in value.Descendants("OutputWires").Descendants("WireDetail") select ow)
                {
                    var output = Convert.ToInt32(outputWireDetail.Element("Output").Value);
                    var wire = Wires[Convert.ToInt32(outputWireDetail.Element("WireID").Value)];
                    outputWires.Add(output, wire);
                }

                element.Load(value, inputWires, outputWires);
                AddElementToList(element);
            }

            foreach (var control in Elements)
                if (control is IActivatableControl)
                {
                    var cont = control as IActivatableControl;
                    cont.Activate();
                }
        }

        public void Draw(CircuitView view)
        {
            foreach (var wire in Wires.Values)
            {
                view.AddWire(wire);
            }

            foreach (var el in Elements) view.AddControl(el);
        }

        public List<ICanvasElement> GetInputs()
        {
            return Elements.Where(x => x is InputControl).ToList();
        }

        public List<ICanvasElement> GetOutputs()
        {
            return Elements.Where(x => x is OutputControl).ToList();
        }

        public int GetNextWireIterator()
        {
            WireIterator++;
            return WireIterator;
        }

        public void AddWire(Wire wire)
        {
            Wires.Add((wire.GetBusiness() as WireBusiness).ID, wire);
        }

        public void AddElementToList(ICanvasElement element)
        {
            Elements.Add(element);
        }

        public void Save()
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            if (string.IsNullOrEmpty(Path)) Path = @"C:\Programming\Test.CBaS";

            using (var writer = XmlWriter.Create(Path, settings))
            {
                // <Circuit>
                writer.WriteStartElement(SaveLoadTags.CIRCUIT_NODE);

                // <Wires>
                writer.WriteStartElement(SaveLoadTags.WIRES_NODE);

                foreach (var wire in Wires)
                {
                    // <Wire>
                    writer.WriteStartElement(SaveLoadTags.WIRE_NODE);
                    writer.WriteElementString(SaveLoadTags.ID, wire.Key.ToString());
                    writer.WriteElementString(SaveLoadTags.X1, wire.Value.X1.ToString());
                    writer.WriteElementString(SaveLoadTags.Y1, wire.Value.Y1.ToString());
                    writer.WriteElementString(SaveLoadTags.X2, wire.Value.X2.ToString());
                    writer.WriteElementString(SaveLoadTags.Y2, wire.Value.Y2.ToString());
                    // </Wire>
                    writer.WriteEndElement();
                }

                // </Wires>
                writer.WriteEndElement();

                // <WireLinks>
                writer.WriteStartElement(SaveLoadTags.WIRE_LINKS_NODE);

                foreach (var wire in Wires)
                {
                    if (wire.Value.OutputWires.Count > 0)
                    {
                        // <WireLink>
                        writer.WriteStartElement(SaveLoadTags.WIRE_LINK_NODE);
                        writer.WriteElementString(SaveLoadTags.PARENT_ID, wire.Key.ToString());

                        // <Links>
                        writer.WriteStartElement(SaveLoadTags.LINK_NODE);

                        foreach (var linkedWire in wire.Value.OutputWires) writer.WriteElementString(SaveLoadTags.LINK, 
                            (linkedWire.GetBusiness() as WireBusiness).ID.ToString());

                        // </Links>
                        writer.WriteEndElement();

                        // </WireLink>
                        writer.WriteEndElement();
                    }
                }

                // </WireLinks>
                writer.WriteEndElement();

                // <CanvasElements>
                writer.WriteStartElement(SaveLoadTags.CANVAS_ELEMENTS_NODE);

                foreach (var element in Elements) element.Save(writer);

                // </CanvasElements>
                writer.WriteEndElement();

                // </Circuit>
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}