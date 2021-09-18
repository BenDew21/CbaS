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
    public class InputControl : ICanvasElement
    {
        private Ellipse _circle;

        private Wire _outputWire;

        private InputControlBusiness _inputControlBusiness = new();
        
        public InputControl()
        {
            _inputControlBusiness.Control = this;
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

        public void SetPlaced()
        {
            Panel.SetZIndex(_circle, 3);
            _circle.MouseDown += Control_MouseDown;
            _circle.MouseMove += Control_MouseMove;
            _circle.MouseUp += Control_MouseUp;
        }

        public void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DragHandler.GetInstance().MouseUp(this, e);
        }

        public void Control_MouseMove(object sender, MouseEventArgs e)
        {
            DragHandler.GetInstance().MouseMove(this, e);
        }

        public void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WireStatus.GetInstance().GetSelected())
            {
                if (WireStatus.GetInstance().GetWire() == null)
                {
                    var x = Canvas.GetLeft(_circle) + 5;
                    var y = Canvas.GetTop(_circle) + 5;

                    WireStatus.GetInstance().SetStart(x, y);
                    SetOutputWire(WireStatus.GetInstance().GetWire());
                }
            }
            else if (DragHandler.GetInstance().IsActive)
            {
                DragHandler.GetInstance().MouseDown(this, e);
            }
            else
            {
                _inputControlBusiness.ToggleOutputting();
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
            writer.WriteElementString(SaveLoadTags.ACTIVE, _inputControlBusiness.Outputting.ToString());
            writer.WriteStartElement(SaveLoadTags.OUTPUT_WIRES_NODE);

            if (_outputWire != null)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.OUTPUT, "1");
                writer.WriteElementString(SaveLoadTags.WIRE_ID, _outputWire.GetID().ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            Canvas.SetTop(_circle, Convert.ToInt32(element.Element(SaveLoadTags.TOP).Value));
            Canvas.SetLeft(_circle, Convert.ToInt32(element.Element(SaveLoadTags.LEFT).Value));

            if (outputWires.Count > 0) SetOutputWire(outputWires[1]);
            
            _inputControlBusiness.Outputting = Convert.ToBoolean(element.Element(SaveLoadTags.ACTIVE).Value);
        }

        public int GetSnap()
        {
            return 10;
        }

        public int GetOffset()
        {
            return 5;
        }

        public void UpdatePosition(double topX, double topY)
        {
            Canvas.SetLeft(GetControl(), topX);
            Canvas.SetTop(GetControl(), topY);

            _outputWire.SetStart(topX + 5, topY + 5);
        }

        public void SetOutputWire(Wire wire)
        {
            _outputWire = wire;
            _inputControlBusiness.SetOutputWire(wire.GetBusiness() as WireBusiness);
        }

        public void UpdateOutputting()
        {
            if (_inputControlBusiness.Outputting)
            {
                _circle.Fill = Brushes.Green;
                _circle.Stroke = Brushes.Green;
            }
            else
            {
                _circle.Fill = Brushes.Red;
                _circle.Stroke = Brushes.Red;
            }
        }
    }
}