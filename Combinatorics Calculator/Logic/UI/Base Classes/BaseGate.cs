using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Controls;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Logic.UI.Base_Classes
{
    public abstract class BaseGate : IWireObserver, ICanvasElement
    {
        protected WireStatus wireStatus = WireStatus.GetInstance();

        private Canvas _pane;
        private System.Windows.Controls.Image _image;
        private Bitmap _bitmap;

        // Wires
        protected Dictionary<int, Wire> inputWires = new Dictionary<int, Wire>();

        protected Dictionary<int, Wire> outputWires = new Dictionary<int, Wire>();

        // Wire pixel offsets
        protected Dictionary<int, WireOffset> inputWireOffsets = new Dictionary<int, WireOffset>();

        protected Dictionary<int, WireOffset> outputWireOffsets = new Dictionary<int, WireOffset>();

        private ContextMenu _inputMenu;
        private ContextMenu _outputMenu;

        protected abstract void RegisterOffsets();

        public abstract void CalculateOutput();

        public abstract BaseGate GetNewControl();

        public BaseGate(Bitmap bitmap)
        {
            _bitmap = bitmap;
            CreateControl();
            RegisterOffsets();
        }

        public void CreateControl()
        {
            _pane = new Canvas();
            _pane.Width = 60;
            _pane.Height = 40;
            _pane.Background = System.Windows.Media.Brushes.Transparent;

            _image = new System.Windows.Controls.Image();
            _image.Source = UIIconConverter.BitmapToBitmapImage(_bitmap);
            _image.Stretch = System.Windows.Media.Stretch.Fill;
            _image.Width = 60;
            _image.Height = 40;

            _pane.Children.Add(_image);
        }

        public void OnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (wireStatus.GetSelected())
            {
                if (wireStatus.GetWire() == null)
                {
                    CreateOutputMenu();
                    _outputMenu.IsOpen = true;
                    _outputMenu.PlacementTarget = _pane;
                }
                else
                {
                    CreateInputMenu();
                    _inputMenu.IsOpen = true;
                    _inputMenu.PlacementTarget = _pane;
                }
            }
            e.Handled = true;
        }

        protected void CreateInputMenu()
        {
            _inputMenu = new ContextMenu();
            if (inputWireOffsets.Count > 0)
            {
                foreach (var item in inputWireOffsets.Keys)
                {
                    // TODO: Add check in here to see if key is used, if so, then dont add
                    if (!inputWires.ContainsKey(item))
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Header = "Input " + item;
                        menuItem.Click += (obj, e) =>
                        {
                            WireOffset wireOffset = inputWireOffsets[item];
                            RegisterInputWire(item, wireStatus.GetWire());
                            wireStatus.SetEnd(Canvas.GetLeft(_pane) + wireOffset.XOffset,
                                Canvas.GetTop(_pane) + wireOffset.YOffset, this);
                        };
                        _inputMenu.Items.Add(menuItem);
                    }
                }
            }
        }

        protected void CreateOutputMenu()
        {
            _outputMenu = new ContextMenu();
            if (outputWireOffsets.Count > 0)
            {
                foreach (var item in outputWireOffsets.Keys)
                {
                    if (!outputWires.ContainsKey(item))
                    {
                        // TODO: Add check in here to see if key is used, if so, then dont add
                        MenuItem menuItem = new MenuItem();
                        menuItem.Header = "Output " + item;
                        menuItem.Click += (obj, e) =>
                        {
                            WireOffset wireOffset = outputWireOffsets[item];
                            wireStatus.SetStart(Canvas.GetLeft(_pane) + wireOffset.XOffset,
                                Canvas.GetTop(_pane) + wireOffset.YOffset);
                            RegisterOutputWire(item, wireStatus.GetWire());
                        };
                        _outputMenu.Items.Add(menuItem);
                    }
                }
            }
        }

        protected void RegisterInputWire(int port, Wire wire)
        {
            inputWires.Add(port, wire);
        }

        protected void RegisterOutputWire(int port, Wire wire)
        {
            outputWires.Add(port, wire);
        }

        public UIElement GetControl()
        {
            return _pane;
        }

        public void WireStatusChanged(Wire wire, bool status)
        {
            CalculateOutput();
        }

        public void SetPlaced()
        {
            _pane.MouseDown += OnClick;
        }

        public ICanvasElement GetNew()
        {
            return GetNewControl();
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(SaveLoadTags.CANVAS_ELEMENT_NODE);
            writer.WriteElementString(SaveLoadTags.TYPE, GetType().Name);
            writer.WriteElementString(SaveLoadTags.TOP, Canvas.GetTop(GetControl()).ToString());
            writer.WriteElementString(SaveLoadTags.LEFT, Canvas.GetLeft(GetControl()).ToString());
          
            writer.WriteStartElement(SaveLoadTags.INPUT_WIRES_NODE);
            foreach (KeyValuePair<int, Wire> inputWirePair in inputWires)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.INPUT, inputWirePair.Key.ToString());
                writer.WriteElementString(SaveLoadTags.WIRE_ID, inputWirePair.Value.ID.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement(SaveLoadTags.OUTPUT_WIRES_NODE);
            foreach (KeyValuePair<int, Wire> outputWirePair in outputWires)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.OUTPUT, outputWirePair.Key.ToString());
                writer.WriteElementString(SaveLoadTags.WIRE_ID, outputWirePair.Value.ID.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            Canvas.SetTop(GetControl(), Convert.ToInt32(element.Element("Top").Value));
            Canvas.SetLeft(GetControl(), Convert.ToInt32(element.Element("Left").Value));
            this.inputWires = inputWires;
            this.outputWires = outputWires;
        }
    }
}