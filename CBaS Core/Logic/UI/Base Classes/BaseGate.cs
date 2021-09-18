using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.Business;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Logic.UI.Utility_Classes;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;

namespace CBaSCore.Logic.UI.Base_Classes
{
    public abstract class BaseGate<T> : IWireObserver, ICanvasElement where T : BaseGateWireBusiness, new()
    {
        private readonly Bitmap _bitmap;
        private Image _image;

        private ContextMenu _inputMenu;
        private ContextMenu _outputMenu;

        private Canvas _pane;

        // Wire pixel offsets
        protected Dictionary<int, WireOffset> inputWireOffsets = new();

        // Wires
        protected Dictionary<int, Wire> inputWires = new();

        protected Dictionary<int, WireOffset> outputWireOffsets = new();

        protected Dictionary<int, Wire> outputWires = new();
        protected WireStatus wireStatus = WireStatus.GetInstance();

        private T _business = new();
        
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
            _pane.Background = Brushes.Transparent;

            _image = new Image();
            _image.Source = UiIconConverter.BitmapToBitmapImage(_bitmap);
            _image.Stretch = Stretch.Fill;
            _image.Width = 60;
            _image.Height = 40;

            _pane.Children.Add(_image);
        }

        public void Control_MouseDown(object sender, MouseButtonEventArgs e)
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
            else if (DragHandler.GetInstance().IsActive)
            {
                DragHandler.GetInstance().MouseDown(this, e);
            }

            e.Handled = true;
        }

        public void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DragHandler.GetInstance().MouseUp(this, e);
        }

        public void Control_MouseMove(object sender, MouseEventArgs e)
        {
            DragHandler.GetInstance().MouseMove(this, e);
        }

        public UIElement GetControl()
        {
            return _pane;
        }

        public void SetPlaced()
        {
            _pane.MouseDown += Control_MouseDown;
            _pane.MouseMove += Control_MouseMove;
            _pane.MouseUp += Control_MouseUp;
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
            foreach (var inputWirePair in inputWires)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.INPUT, inputWirePair.Key.ToString());
                writer.WriteElementString(SaveLoadTags.WIRE_ID, inputWirePair.Value.GetID().ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteStartElement(SaveLoadTags.OUTPUT_WIRES_NODE);
            foreach (var outputWirePair in outputWires)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.OUTPUT, outputWirePair.Key.ToString());
                writer.WriteElementString(SaveLoadTags.WIRE_ID, outputWirePair.Value.GetID().ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            Canvas.SetTop(GetControl(), Convert.ToInt32(element.Element("Top").Value));
            Canvas.SetLeft(GetControl(), Convert.ToInt32(element.Element("Left").Value));

            foreach (var (key, value) in inputWires)
            {
                RegisterInputWire(key, value);
            }
            
            foreach (var (key, value) in outputWires)
            {
                RegisterOutputWire(key, value);
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

        public void UpdatePosition(double topX, double topY)
        {
            Canvas.SetLeft(GetControl(), topX);
            Canvas.SetTop(GetControl(), topY);

            foreach (var offsetPair in inputWireOffsets) inputWires[offsetPair.Key].SetEnd(topX + offsetPair.Value.XOffset, topY + offsetPair.Value.YOffset, this);

            foreach (var offsetPair in outputWireOffsets) outputWires[offsetPair.Key].SetStart(topX + offsetPair.Value.XOffset, topY + offsetPair.Value.YOffset);
        }

        public void WireStatusChanged(Wire wire, bool status)
        {
            CalculateOutput();
        }

        public IWireBusinessObserver GetBusiness()
        {
            return _business;
        }

        private void CalculateOutput()
        {
            _business.CalculateOutput();
        }
        
        protected abstract void RegisterOffsets();

        protected abstract BaseGate<T> GetNewControl();

        private void CreateInputMenu()
        {
            _inputMenu = new ContextMenu();
            if (inputWireOffsets.Count > 0)
                foreach (var item in inputWireOffsets.Keys)
                    if (!inputWires.ContainsKey(item))
                    {
                        var menuItem = new MenuItem();
                        menuItem.Header = "Input " + item;
                        menuItem.Click += (obj, e) =>
                        {
                            var wireOffset = inputWireOffsets[item];
                            RegisterInputWire(item, wireStatus.GetWire());
                            wireStatus.SetEnd(Canvas.GetLeft(_pane) + wireOffset.XOffset,
                                Canvas.GetTop(_pane) + wireOffset.YOffset, this);
                        };
                        _inputMenu.Items.Add(menuItem);
                    }
        }

        private void CreateOutputMenu()
        {
            _outputMenu = new ContextMenu();
            if (outputWireOffsets.Count > 0)
                foreach (var item in outputWireOffsets.Keys)
                    if (!outputWires.ContainsKey(item))
                    {
                        var menuItem = new MenuItem();
                        menuItem.Header = "Output " + item;
                        menuItem.Click += (obj, e) =>
                        {
                            var wireOffset = outputWireOffsets[item];
                            wireStatus.SetStart(Canvas.GetLeft(_pane) + wireOffset.XOffset,
                                Canvas.GetTop(_pane) + wireOffset.YOffset);
                            RegisterOutputWire(item, wireStatus.GetWire());
                        };
                        _outputMenu.Items.Add(menuItem);
                    }
        }

        private void RegisterInputWire(int port, Wire wire)
        {
            inputWires.Add(port, wire);
            _business.AddInputWire(port, wire.GetBusiness() as WireBusiness);
        }

        private void RegisterOutputWire(int port, Wire wire)
        {
            outputWires.Add(port, wire);
            _business.AddOutputWire(port, wire.GetBusiness() as WireBusiness);
        }
    }
}