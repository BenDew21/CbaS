using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Logic.UI.Controls
{
    public class InputControl : ICanvasElement
    {
        private Ellipse _circle;

        private Wire _outputWire;

        private bool _outputting = false;

        public InputControl()
        {
            CreateControl();
        }

        public void CreateControl()
        {
            _circle = new Ellipse();
            _circle.Width = 10;
            _circle.Height = 10;
            _circle.Fill = Brushes.Red;
            _circle.Stroke = Brushes.Red;
            _circle.StrokeThickness = 0.5;
        }

        public void SetOutputWire(Wire wire)
        {
            _outputWire = wire;
            _outputWire.ToggleStatus(_outputting);
        }

        public void SetPlaced()
        {
            Canvas.SetZIndex(_circle, 3);
            _circle.MouseDown += Control_MouseDown;
        }

        private void Control_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (WireStatus.GetInstance().GetSelected())
            {
                if (WireStatus.GetInstance().GetWire() == null)
                {
                    double x = Canvas.GetLeft(_circle) + 5;
                    double y = Canvas.GetTop(_circle) + 5;

                    WireStatus.GetInstance().SetStart(x, y);
                    SetOutputWire(WireStatus.GetInstance().GetWire());
                }
            }
            else
            {
                _outputting = !_outputting;
                if (_outputting)
                {
                    _circle.Fill = Brushes.Green;
                    _circle.Stroke = Brushes.Green;
                }
                else
                {
                    _circle.Fill = Brushes.Red;
                    _circle.Stroke = Brushes.Red;
                }

                if (_outputWire != null)
                {
                    _outputWire.ToggleStatus(_outputting);
                }
            }

            e.Handled = true;
        }

        public UIElement GetControl()
        {
            return _circle;
        }

        public ICanvasElement GetNew()
        {
            return new InputControl();
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(SaveLoadTags.CANVAS_ELEMENT_NODE);
            writer.WriteElementString(SaveLoadTags.TYPE, "InputControl");
            writer.WriteElementString(SaveLoadTags.TOP, Canvas.GetTop(_circle).ToString());
            writer.WriteElementString(SaveLoadTags.LEFT, Canvas.GetLeft(_circle).ToString());
            writer.WriteStartElement(SaveLoadTags.OUTPUT_WIRES_NODE);
            writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
            if (_outputWire != null)
            {
                writer.WriteElementString(SaveLoadTags.OUTPUT, "1");
                writer.WriteElementString(SaveLoadTags.WIRE_ID, _outputWire.ID.ToString());
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            Canvas.SetTop(_circle, Convert.ToInt32(element.Element("Top").Value));
            Canvas.SetLeft(_circle, Convert.ToInt32(element.Element("Left").Value));
            
            if (outputWires.Count > 0)
            {
                _outputWire = outputWires[1];
            }
        }

        public int GetSnap()
        {
            return 10;
        }

        public int GetOffset()
        {
            return 5;
        }
    }
}