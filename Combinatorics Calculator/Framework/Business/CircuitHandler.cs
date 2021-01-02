﻿using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Controls;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        public static CircuitHandler GetInstance()
        {
            if (_instance == null) _instance = new CircuitHandler();
            return _instance;
        }

        public void RegisterCircuitView(CircuitView view)
        {
            _view = view;
        }

        public void AddWire(Wire wire)
        {
            _wires.Add(WireIterator, wire);
            WireIterator++;
        }

        public void AddICanvasElement(ICanvasElement element)
        {
            _elements.Add(ICanvasElementIterator++, element);
            ICanvasElementIterator++;
        }

        public void Load(string filePath)
        {
            XElement document = XElement.Load(filePath);
            foreach (var value in from c in document.Descendants("Wire") select c)
            {
                WireStatus.GetInstance().SetStart(Convert.ToInt32(value.Element("X1").Value), 
                    Convert.ToInt32(value.Element("Y1").Value));

                int id = Convert.ToInt32(value.Element("ID").Value);
                WireStatus.GetInstance().GetWire().ID = id;

                WireStatus.GetInstance().SetEnd(Convert.ToInt32(value.Element("X2").Value),
                    Convert.ToInt32(value.Element("Y2").Value), null);
            }

            foreach (var value in from c in document.Descendants("WireLinks").Descendants("WireLink") select c)
            {
                int parentID = Convert.ToInt32(value.Element("ParentID").Value);
                Wire parentWire = _wires[parentID];

                foreach (var link in from links in value.Descendants("Links").Descendants("Link") select links)
                {
                    int id = Convert.ToInt32(link.Value);
                    Wire childWire = _wires[id];
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
                    Wire wire = _wires[Convert.ToInt32(inputWireDetail.Element("WireID").Value)];
                    wire.RegisterWireObserver((IWireObserver) element);
                    inputWires.Add(input, wire);
                }

                foreach (var outputWireDetail in from ow in value.Descendants("OutputWires").Descendants("WireDetail") select ow)
                {
                    int output = Convert.ToInt32(outputWireDetail.Element("Output").Value);
                    Wire wire = _wires[Convert.ToInt32(outputWireDetail.Element("WireID").Value)];
                    outputWires.Add(output, wire);
                }

                element.Load(value, inputWires, outputWires);
                _view.AddControl(element);
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
                writer.WriteStartElement("Circuit");

                // <Wires>
                writer.WriteStartElement("Wires");
                
                foreach (KeyValuePair<int, Wire> wire in _wires)
                {
                    // <Wire>
                    writer.WriteStartElement("Wire");
                    writer.WriteElementString("ID", wire.Key.ToString());
                    writer.WriteElementString("X1", wire.Value.X1.ToString());
                    writer.WriteElementString("Y1", wire.Value.Y1.ToString());
                    writer.WriteElementString("X2", wire.Value.X2.ToString());
                    writer.WriteElementString("Y2", wire.Value.Y2.ToString());
                    // </Wire>
                    writer.WriteEndElement();
                }

                // </Wires>
                writer.WriteEndElement();

                // <WireLinks>
                writer.WriteStartElement("WireLinks");

                foreach (KeyValuePair<int, Wire> wire in _wires)
                {
                    if (wire.Value.OutputWires.Count > 0)
                    {
                        // <WireLink>
                        writer.WriteStartElement("WireLink");
                        writer.WriteElementString("ParentID", wire.Key.ToString());

                        // <Links>
                        writer.WriteStartElement("Links");

                        foreach (var linkedWire in wire.Value.OutputWires)
                        {
                            writer.WriteElementString("Link", linkedWire.ID.ToString());
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
                writer.WriteStartElement("CanvasElements");

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
