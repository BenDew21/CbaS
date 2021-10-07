using System;
using System.Collections.Generic;
using System.IO;
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
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Logic.UI.Utility_Classes;
using SQLitePCL;

namespace CBaSCore.Logic.UI.Controls.EEPROMs
{
    public class EEPROM28C16 : ICanvasElement, IWireObserver
    {
        private Image _image;

        private ContextMenu _inputMenu;

        // Wire pixel offsets
        private readonly Dictionary<int, WireOffset> _inputWireOffsets = new();

        // Wires
        private readonly Dictionary<int, Wire> _inputWires = new();
        private readonly Dictionary<int, Wire> _outputWires = new();
        
        private ContextMenu _outputMenu;

        private readonly Dictionary<int, WireOffset> _outputWireOffsets = new();
        
        private readonly Dictionary<int, string> _pinDescriptions = new();
        
        private readonly WireStatus _wireStatus = WireStatus.GetInstance();

        private readonly EEPROM28C16Business _business = new();
        
        public EEPROM28C16()
        {
            CreateControl();
            RegisterWireOffsets();
            RegisterDescriptions();
            CreateInputMenu();
            CreateOutputMenu();
        }

        #region Registering wires (UI Level)

        private void RegisterInputWire(int port, Wire wire)
        {
            _inputWires.Add(port, wire);
            _business.RegisterInputWire(port, wire.GetBusiness() as WireBusiness);
        }

        private void RegisterOutputWire(int port, Wire wire)
        {
            _outputWires.Add(port, wire);
            _business.RegisterOutputWire(port, wire.GetBusiness() as WireBusiness);
        }

        #endregion Registering wires (UI Level)

        #region Register pins

        /// <summary>
        ///     Pin 1 - A7                      Pin 24 - Vcc (NC for this)
        ///     Pin 2 - A6                      Pin 23 - A8
        ///     Pin 3 - A5                      Pin 22 - A9
        ///     Pin 4 - A4                      Pin 21 - Write Enable (Active Low)
        ///     Pin 5 - A3                      Pin 20 - Output Enable (Active Low)
        ///     Pin 6 - A2                      Pin 19 - A10
        ///     Pin 7 - A1                      Pin 18 - Chip Enable (Active Low)
        ///     Pin 8 - A0                      Pin 17 - I/O 7
        ///     Pin 9 - I/O 0                   Pin 16 - I/O 6
        ///     Pin 10 - I/O 1                  Pin 15 - I/O 5
        ///     Pin 11 - I/O 2                  Pin 14 - I/O 4
        ///     Pin 12 - Vss (NC for this)      Pin 13 - I/O 3
        /// </summary>
        private void RegisterDescriptions()
        {
            _pinDescriptions.Add(1, "A7");
            _pinDescriptions.Add(2, "A6");
            _pinDescriptions.Add(3, "A5");
            _pinDescriptions.Add(4, "A4");
            _pinDescriptions.Add(5, "A3");
            _pinDescriptions.Add(6, "A2");
            _pinDescriptions.Add(7, "A1");
            _pinDescriptions.Add(8, "A0");
            _pinDescriptions.Add(9, "I/O 0");
            _pinDescriptions.Add(10, "I/O 1");
            _pinDescriptions.Add(11, "I/O 2");
            _pinDescriptions.Add(12, "Vss (Not Required)");
            _pinDescriptions.Add(13, "I/O 3");
            _pinDescriptions.Add(14, "I/O 4");
            _pinDescriptions.Add(15, "I/O 5");
            _pinDescriptions.Add(16, "I/O 6");
            _pinDescriptions.Add(17, "I/O 7");
            _pinDescriptions.Add(18, "!Chip Enable");
            _pinDescriptions.Add(19, "A10");
            _pinDescriptions.Add(20, "!Output Enable");
            _pinDescriptions.Add(21, "!Write Enable");
            _pinDescriptions.Add(22, "A9");
            _pinDescriptions.Add(23, "A8");
            _pinDescriptions.Add(24, "Vcc (Not Required)");
        }

        private void RegisterWireOffsets()
        {
            _inputWireOffsets.Add(1, new WireOffset {XOffset = 10, YOffset = 50});
            _inputWireOffsets.Add(2, new WireOffset {XOffset = 20, YOffset = 50});
            _inputWireOffsets.Add(3, new WireOffset {XOffset = 30, YOffset = 50});
            _inputWireOffsets.Add(4, new WireOffset {XOffset = 40, YOffset = 50});
            _inputWireOffsets.Add(5, new WireOffset {XOffset = 50, YOffset = 50});
            _inputWireOffsets.Add(6, new WireOffset {XOffset = 60, YOffset = 50});
            _inputWireOffsets.Add(7, new WireOffset {XOffset = 70, YOffset = 50});
            _inputWireOffsets.Add(8, new WireOffset {XOffset = 80, YOffset = 50});
            _outputWireOffsets.Add(9, new WireOffset {XOffset = 90, YOffset = 50});
            _outputWireOffsets.Add(10, new WireOffset {XOffset = 100, YOffset = 50});
            _outputWireOffsets.Add(11, new WireOffset {XOffset = 110, YOffset = 50});
            _inputWireOffsets.Add(12, new WireOffset {XOffset = 120, YOffset = 50});
            _outputWireOffsets.Add(13, new WireOffset {XOffset = 120, YOffset = 10});
            _outputWireOffsets.Add(14, new WireOffset {XOffset = 110, YOffset = 10});
            _outputWireOffsets.Add(15, new WireOffset {XOffset = 100, YOffset = 10});
            _outputWireOffsets.Add(16, new WireOffset {XOffset = 90, YOffset = 10});
            _outputWireOffsets.Add(17, new WireOffset {XOffset = 80, YOffset = 10});
            _inputWireOffsets.Add(18, new WireOffset {XOffset = 70, YOffset = 10});
            _inputWireOffsets.Add(19, new WireOffset {XOffset = 60, YOffset = 10});
            _inputWireOffsets.Add(20, new WireOffset {XOffset = 50, YOffset = 10});
            _inputWireOffsets.Add(21, new WireOffset {XOffset = 40, YOffset = 10});
            _inputWireOffsets.Add(22, new WireOffset {XOffset = 30, YOffset = 10});
            _inputWireOffsets.Add(23, new WireOffset {XOffset = 20, YOffset = 10});
            _inputWireOffsets.Add(24, new WireOffset {XOffset = 10, YOffset = 10});
        }

        #endregion Register pins

        #region Creating Context Menus

        private void CreateInputMenu()
        {
            _inputMenu = new ContextMenu();
            if (_inputWireOffsets.Count > 0)
                foreach (var item in _inputWireOffsets.Keys)
                    if (!_inputWires.ContainsKey(item))
                    {
                        var menuItem = new MenuItem();
                        menuItem.Header = "Input " + item + " - " + _pinDescriptions[item];
                        menuItem.Click += (obj, e) =>
                        {
                            var wireOffset = _inputWireOffsets[item];
                            RegisterInputWire(item, _wireStatus.GetWire());
                            _wireStatus.SetEnd(Canvas.GetLeft(_image) + wireOffset.XOffset,
                                Canvas.GetTop(_image) + wireOffset.YOffset, this);
                        };
                        _inputMenu.Items.Add(menuItem);
                    }
        }

        private void CreateOutputMenu()
        {
            _outputMenu = new ContextMenu();
            if (_outputWireOffsets.Count > 0)
                foreach (var item in _outputWireOffsets.Keys)
                    if (!_outputWires.ContainsKey(item))
                    {
                        var menuItem = new MenuItem();
                        menuItem.Header = "Output " + item + " - " + _pinDescriptions[item];
                        menuItem.Click += (obj, e) =>
                        {
                            var wireOffset = _outputWireOffsets[item];
                            _wireStatus.SetStart(Canvas.GetLeft(_image) + wireOffset.XOffset,
                                Canvas.GetTop(_image) + wireOffset.YOffset);
                            RegisterOutputWire(item, _wireStatus.GetWire());
                        };
                        _outputMenu.Items.Add(menuItem);
                    }
        }

        #endregion Creating Context Menus

        #region Base methods

        public void CreateControl()
        {
            _image = new Image
            {
                Source = UiIconConverter.BitmapToBitmapImage(Logic_Resources._28C16),
                Stretch = Stretch.Fill,
                Width = 130,
                Height = 60
            };
        }

        public void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_wireStatus.GetSelected())
            {
                if (_wireStatus.GetWire() == null)
                {
                    CreateOutputMenu();
                    _outputMenu.IsOpen = true;
                    _outputMenu.PlacementTarget = _image;
                }
                else
                {
                    CreateInputMenu();
                    _inputMenu.IsOpen = true;
                    _inputMenu.PlacementTarget = _image;
                }
            }
            else if (DragHandler.GetInstance().IsActive)
            {
                DragHandler.GetInstance().MouseDown(this, e);
            }
            else if (e.ClickCount == 2)
            {
                new EEPROMEditor(_business).Show();
            }

            e.Handled = true;
        }

        public void Control_MouseMove(object sender, MouseEventArgs e)
        {
        }

        public void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        public UIElement GetControl()
        {
            return _image;
        }

        public ICanvasElement GetNew()
        {
            return new EEPROM28C16();
        }

        public int GetOffset()
        {
            return 0;
        }

        public int GetSnap()
        {
            return 10;
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(SaveLoadTags.CANVAS_ELEMENT_NODE);
            writer.WriteElementString(SaveLoadTags.TYPE, GetType().Name);
            writer.WriteElementString(SaveLoadTags.TOP, Canvas.GetTop(GetControl()).ToString());
            writer.WriteElementString(SaveLoadTags.LEFT, Canvas.GetLeft(GetControl()).ToString());

            writer.WriteStartElement(SaveLoadTags.INPUT_WIRES_NODE);
            foreach (var inputWirePair in _inputWires)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.INPUT, inputWirePair.Key.ToString());
                writer.WriteElementString(SaveLoadTags.WIRE_ID, inputWirePair.Value.GetID().ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteStartElement(SaveLoadTags.OUTPUT_WIRES_NODE);
            foreach (var outputWirePair in _outputWires)
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

        public void SetPlaced()
        {
            Panel.SetZIndex(_image, 3);

            _image.MouseDown += Control_MouseDown;
            _image.MouseMove += Control_MouseMove;
            _image.MouseUp += Control_MouseUp;
        }

        public void UpdatePosition(double topX, double topY)
        {
        }

        public void WireStatusChanged(Wire wire, bool status)
        {
            
        }

        public IWireBusinessObserver GetBusiness()
        {
            return _business;
        }

        #endregion Base methods
    }
}