using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;

namespace Combinatorics_Calculator.Logic.UI.Controls
{
    class OutputControl : ICanvasElement, IWireObserver
    {
        private Grid _canvas;
        private Ellipse _ellipse;
        private Label _label;

        private Wire _inputWire;

        public OutputControl()
        {
            CreateControl();
        }

        public void CreateControl()
        {
            _canvas = new Grid();
            _canvas.Width = 20;
            _canvas.Height = 20;
            _canvas.Background = Brushes.Transparent;

            _ellipse = new Ellipse();
            _ellipse.Width = 20;
            _ellipse.Height = 20;
            _ellipse.Fill = Brushes.White;
            _ellipse.Stroke = Brushes.Black;

            _label = new Label();
            _label.Content = "0";

            _canvas.Children.Add(_ellipse);
            _canvas.Children.Add(_label);

            _label.HorizontalAlignment = HorizontalAlignment.Center;
            _label.Padding = new Thickness(0, 0, 0, 0.1);

            Canvas.SetZIndex(_canvas, 2);
        }

        public void SetInputWire(Wire wire) 
        {
            _inputWire = wire;
            WireStatusChanged(_inputWire, _inputWire.GetStatus());
        }

        public UIElement GetControl()
        {
            return _canvas;
        }

        public ICanvasElement GetNew()
        {
            return new OutputControl();
        }

        public void SetPlaced()
        {
            Canvas.SetZIndex(_canvas, 3);
            _canvas.MouseDown += Canvas_MouseDown;
        }

        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (WireStatus.GetInstance().GetSelected())
            {
                if (WireStatus.GetInstance().GetWire() != null)
                {
                    double x = Canvas.GetLeft(_canvas) + 10;
                    double y = Canvas.GetTop(_canvas) + 10;

                    SetInputWire(WireStatus.GetInstance().GetWire());
                    WireStatus.GetInstance().SetEnd(x, y, this);
                }
            }
        }

        public void WireStatusChanged(Wire wire, bool status)
        {
            _label.Content = status ? "1" : "0";
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(SaveLoadTags.CANVAS_ELEMENT_NODE);
            writer.WriteElementString(SaveLoadTags.TYPE, "OutputControl");
            writer.WriteElementString(SaveLoadTags.TOP, Canvas.GetTop(_canvas).ToString());
            writer.WriteElementString(SaveLoadTags.LEFT, Canvas.GetLeft(_canvas).ToString());
            writer.WriteStartElement(SaveLoadTags.INPUT_WIRES_NODE);
            writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
            if (_inputWire != null)
            {
                writer.WriteElementString(SaveLoadTags.INPUT, "1");
                writer.WriteElementString(SaveLoadTags.WIRE_ID, _inputWire.ID.ToString());
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            Canvas.SetTop(_canvas, Convert.ToInt32(element.Element("Top").Value));
            Canvas.SetLeft(_canvas, Convert.ToInt32(element.Element("Left").Value));

            if (inputWires.Count > 0)
            {
                _inputWire = inputWires[1];
            }
        }

        public int GetSnap()
        {
            return 10;
        }

        public int GetOffset()
        {
            return 0;
        }
    }
}
