using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.Business;
using CBaSCore.Logic.Business.Gate_Business;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.UI.Controls
{
    public class OutputControl : ICanvasElement, IWireObserver
    {
        private Grid _canvas;
        private Ellipse _ellipse;

        private Wire _inputWire;
        private Label _label;

        private OutputControlWireBusiness _outputControlWireBusiness = new();
        
        public OutputControl()
        {
            _outputControlWireBusiness.SetParent(this);
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

            Panel.SetZIndex(_canvas, 2);
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
            Panel.SetZIndex(_canvas, 3);
            _canvas.MouseDown += Control_MouseDown;
            _canvas.MouseMove += Control_MouseMove;
            _canvas.MouseUp += Control_MouseUp;
        }

        public void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WireStatus.GetInstance().GetSelected())
            {
                if (WireStatus.GetInstance().GetWire() != null)
                {
                    var x = Canvas.GetLeft(_canvas) + 10;
                    var y = Canvas.GetTop(_canvas) + 10;
                    
                    RegisterInputWire(WireStatus.GetInstance().GetWire());
                    WireStatus.GetInstance().SetEnd(x, y, this);
                }
            }
            else
            {
                DragHandler.GetInstance().MouseDown(this, e);
            }
        }

        public void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DragHandler.GetInstance().MouseUp(this, e);
        }

        public void Control_MouseMove(object sender, MouseEventArgs e)
        {
            DragHandler.GetInstance().MouseMove(this, e);
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(SaveLoadTags.CANVAS_ELEMENT_NODE);
            writer.WriteElementString(SaveLoadTags.TYPE, "OutputControl");
            writer.WriteElementString(SaveLoadTags.TOP, Canvas.GetTop(_canvas).ToString());
            writer.WriteElementString(SaveLoadTags.LEFT, Canvas.GetLeft(_canvas).ToString());
            writer.WriteStartElement(SaveLoadTags.INPUT_WIRES_NODE);

            if (_inputWire != null)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.INPUT, "1");
                writer.WriteElementString(SaveLoadTags.WIRE_ID, _inputWire.GetID().ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            Canvas.SetTop(_canvas, Convert.ToInt32(element.Element("Top").Value));
            Canvas.SetLeft(_canvas, Convert.ToInt32(element.Element("Left").Value));

            if (inputWires.Count > 0) RegisterInputWire(inputWires[1]);
        }

        public int GetSnap()
        {
            return 10;
        }

        public int GetOffset()
        {
            return 0;
        }

        public void UpdatePosition(double topX, double topY)
        {
            Canvas.SetLeft(GetControl(), topX);
            Canvas.SetTop(GetControl(), topY);

            _inputWire.SetEnd(topX + 10, topY + 10, this);
        }

        private void RegisterInputWire(Wire wire)
        {
            _inputWire = wire;
            _outputControlWireBusiness.SetInputWire(wire.GetBusiness() as WireBusiness);
        }
        
        public void UpdateDisplay()
        {
            _label.Content = _outputControlWireBusiness.Outputting ? "1" : "0";
        }

        public void WireStatusChanged(Wire wire, bool status)
        {
            
        }

        public IWireBusinessObserver GetBusiness()
        {
            return _outputControlWireBusiness;
        }
    }
}