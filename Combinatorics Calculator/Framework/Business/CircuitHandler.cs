using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Controls;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Project.Storage;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Framework.Business
{
    /// <summary>
    /// What needs to be saved:
    ///     Wires:
    ///         - Start pos
    ///         - End pos
    ///         - Linked wires
    ///         - Output gate/wire
    ///
    ///     ICanvasElements:
    ///         - Top position
    ///         - Left position
    ///         - Input wires (if applicable)
    ///         - Output wires (if applicable)
    ///         - State (if applicable)
    ///
    /// Saving logic:
    ///     - Save start, end x and y for wires
    ///     - Create linked wires list based on IDs
    ///     - Save top and left points for ICanvasElements
    ///     - Save input wires based on IDs
    ///     - Save output wires based on IDs
    ///     - Add output gates/wires to wires
    /// </summary>
    public class CircuitHandler
    {
        private static CircuitHandler _instance;

        private Dictionary<int, Wire> _wires = new Dictionary<int, Wire>();
        private Dictionary<int, ICanvasElement> _elements = new Dictionary<int, ICanvasElement>();

        private CircuitView _view;

        public int WireIterator = 1;
        public int ICanvasElementIterator = 1;

        private Dictionary<int, Circuit> _circuits = new Dictionary<int, Circuit>();

        public static CircuitHandler GetInstance()
        {
            if (_instance == null) _instance = new CircuitHandler();
            return _instance;
        }

        public void RegisterCircuitView(CircuitView view)
        {
            _view = view;
        }

        public void LoadCircuit(int nodeID, string path)
        {
            XElement document = XElement.Load(path);
            Circuit circuit = new Circuit(document);
            circuit.Path = path;
            _circuits.Add(nodeID, circuit);
        }

        public void OpenCircuit(int nodeID)
        {
            CircuitView view = new CircuitView();
            view.Draw(_circuits[nodeID]);
        }

        public Circuit GetCircuit(int id)
        {
            return _circuits[id];
        }

        //public void AddWire(Wire wire)
        //{
        //    _wires.Add(WireIterator, wire);
        //    WireIterator++;
        //}

        //public void AddICanvasElement(ICanvasElement element)
        //{
        //    _elements.Add(ICanvasElementIterator++, element);
        //    ICanvasElementIterator++;
        //}

        public void Load(string filePath)
        {
            XElement document = XElement.Load(filePath);
            _view.Draw(document);
        }

        public void SaveAll()
        {
            foreach (var circuit in _circuits)
            {
                circuit.Value.Save();
            }
        }

        public void Save(string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                // <Circuit>
                writer.WriteStartElement(SaveLoadTags.CIRCUIT_NODE);

                // <Wires>
                writer.WriteStartElement(SaveLoadTags.WIRES_NODE);

                foreach (KeyValuePair<int, Wire> wire in _wires)
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

                foreach (KeyValuePair<int, Wire> wire in _wires)
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

                foreach (KeyValuePair<int, ICanvasElement> element in _elements)
                {
                    element.Value.Save(writer);
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