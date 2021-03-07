using Combinatorics_Calculator.Displays.UI.Controls;
using Combinatorics_Calculator.Drawing.UI.Controls;
using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Controls;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Base_Classes;
using Combinatorics_Calculator.Logic.UI.Controls;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Project.Storage
{
    public class Circuit
    {
        public string Path { get; set; }
        public Dictionary<int, Wire> Wires { get; set; }
        public List<ICanvasElement> Elements { get; set; }

        private int _wireIterator;

        public Circuit()
        {
            Wires = new Dictionary<int, Wire>();
            Elements = new List<ICanvasElement>();

            _wireIterator = 0;
        }

        public Circuit(XElement document) : this()
        {
            Build(document);
        }

        public void Build(XElement document)
        {
            foreach (var value in from c in document.Descendants("Wire") select c)
            {
                int id = Convert.ToInt32(value.Element("ID").Value);

                Wire wire = new Wire();
                wire.ID = id;
                wire.SetStart(Convert.ToInt32(value.Element("X1").Value), Convert.ToInt32(value.Element("Y1").Value));
                wire.SetEnd(Convert.ToInt32(value.Element("X2").Value), Convert.ToInt32(value.Element("Y2").Value));

                Wires.Add(id, wire);
                _wireIterator = id;
            }

            foreach (var value in from c in document.Descendants("WireLinks").Descendants("WireLink") select c)
            {
                int parentID = Convert.ToInt32(value.Element("ParentID").Value);
                Wire parentWire = Wires[parentID];

                foreach (var link in from links in value.Descendants("Links").Descendants("Link") select links)
                {
                    int id = Convert.ToInt32(link.Value);
                    Wire childWire = Wires[id];
                    parentWire.AddOutputWire(childWire);
                }
            }

            CanvasElementFactory factory = new CanvasElementFactory();

            foreach (var value in from c in document.Descendants("CanvasElements").Descendants("CanvasElement") select c)
            {
                Dictionary<int, Wire> inputWires = new Dictionary<int, Wire>();
                Dictionary<int, Wire> outputWires = new Dictionary<int, Wire>();

                ICanvasElement element = factory.CreateFromName(value.Element("Type").Value);

                foreach (var inputWireDetail in from iw in value.Descendants("InputWires").Descendants("WireDetail") select iw)
                {
                    int input = Convert.ToInt32(inputWireDetail.Element("Input").Value);
                    Wire wire = Wires[Convert.ToInt32(inputWireDetail.Element("WireID").Value)];
                    wire.RegisterWireObserver((IWireObserver)element);
                    inputWires.Add(input, wire);
                }

                foreach (var outputWireDetail in from ow in value.Descendants("OutputWires").Descendants("WireDetail") select ow)
                {
                    int output = Convert.ToInt32(outputWireDetail.Element("Output").Value);
                    Wire wire = Wires[Convert.ToInt32(outputWireDetail.Element("WireID").Value)];
                    outputWires.Add(output, wire);
                }

                element.Load(value, inputWires, outputWires);
                AddElementToList(element);
            }
        }

        public void Draw(CircuitView view)
        {
            foreach (var wire in Wires.Values)
            {
                view.AddWire(wire);
            }

            foreach (var el in Elements)
            {
                view.AddControl(el);
            }
        }

        public void AddWire(Wire wire)
        {
            _wireIterator++;
            Wires.Add(_wireIterator, wire);
        }

        public void AddElementToList(ICanvasElement element)
        {
            Debug.WriteLine("Adding element to list");
            Elements.Add(element);            
        }

        public void Save()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(Path, settings))
            {
                // <Circuit>
                writer.WriteStartElement(SaveLoadTags.CIRCUIT_NODE);

                // <Wires>
                writer.WriteStartElement(SaveLoadTags.WIRES_NODE);

                foreach (KeyValuePair<int, Wire> wire in Wires)
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

                foreach (KeyValuePair<int, Wire> wire in Wires)
                {
                    if (wire.Value.OutputWires.Count > 0)
                    {
                        // <WireLink>
                        writer.WriteStartElement(SaveLoadTags.WIRE_LINK_NODE);
                        writer.WriteElementString(SaveLoadTags.PARENT_ID, wire.Key.ToString());

                        // <Links>
                        writer.WriteStartElement(SaveLoadTags.LINK_NODE);

                        foreach (var linkedWire in wire.Value.OutputWires)
                        {
                            writer.WriteElementString(SaveLoadTags.LINK, linkedWire.ID.ToString());
                        }

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

                foreach (var element in Elements)
                {
                    element.Save(writer);
                }

                // </CanvasElements>
                writer.WriteEndElement();

                // </Circuit>
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}