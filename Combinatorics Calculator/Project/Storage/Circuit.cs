using Combinatorics_Calculator.Drawing.UI.Controls;
using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Controls;
using Combinatorics_Calculator.Logic.UI.Base_Classes;
using Combinatorics_Calculator.Logic.UI.Controls;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Project.Storage
{
    public class Circuit
    {
        public Dictionary<int, Wire> Wires { get; set; }
        public List<InputControl> Inputs { get; set; }
        public List<OutputControl> Outputs { get; set; }
        public List<BaseGate> Gates { get; set; }
        public List<DiagramLabel> Labels { get; set; }
        public List<SquareWaveGenerator> Generators { get; set; }

        public Dictionary<int, ICanvasElement> Elements { get; set; }

        private int _wireIterator;

        public Circuit()
        {
            Wires = new Dictionary<int, Wire>();
            Inputs = new List<InputControl>();
            Outputs = new List<OutputControl>();
            Gates = new List<BaseGate>();
            Labels = new List<DiagramLabel>();
            Generators = new List<SquareWaveGenerator>();
            Elements = new Dictionary<int, ICanvasElement>();

            _wireIterator = 0;
        }

        public Circuit(XElement document) :this()
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
            }

            foreach (var value in from c in document.Descendants("WireLinks").Descendants("WireLink") select c)
            {
                int parentID = Convert.ToInt32(value.Element("ParentID").Value);
                Wire parentWire = Wires[parentID];

                foreach (var link in from links in value.Descendants("Links").Descendants("Link") select links)
                {
                    int id = Convert.ToInt32(link.Value);
                    _wireIterator = id;
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

            foreach (var el in Outputs)
            {
                view.AddControl(el);
            }

            foreach (var el in Inputs)
            {
                view.AddControl(el);
            }

            foreach (var el in Gates)
            {
                view.AddControl(el);
            }

            foreach (var el in Labels)
            {
                view.AddControl(el);
            }

            foreach (var el in Generators)
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
            switch (element)
            {
                case OutputControl oc:
                    Outputs.Add(oc);
                    break;
                case InputControl ic:
                    Inputs.Add(ic);
                    break;
                case BaseGate bg:
                    Gates.Add(bg);
                    break;
                case DiagramLabel dl:
                    Labels.Add(dl);
                    break;
                case SquareWaveGenerator swg:
                    Generators.Add(swg);
                    break;
                default:
                    throw new ArgumentException("Element type not recognised", paramName: nameof(element));
            }
        }
    }
}
