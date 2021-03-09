using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Handlers;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Logic.UI.Controls.EEPROMs
{
    public class EEPROM28C16 : ICanvasElement, IWireObserver
    {
        private System.Windows.Controls.Image _image;
        private List<EEPROMRow> _rows = new List<EEPROMRow>();

        private WireStatus _wireStatus = WireStatus.GetInstance();

        private ContextMenu _inputMenu;
        private ContextMenu _outputMenu;

        // Wires
        private Dictionary<int, Wire> _inputWires = new Dictionary<int, Wire>();
        private Dictionary<int, Wire> _outputWires = new Dictionary<int, Wire>();

        // Wire pixel offsets
        private Dictionary<int, WireOffset> _inputWireOffsets = new Dictionary<int, WireOffset>();
        private Dictionary<int, WireOffset> _outputWireOffsets = new Dictionary<int, WireOffset>();

        private Dictionary<int, string> _pinDescriptions = new Dictionary<int, string>();

        public EEPROM28C16()
        {
            CreateControl();
            CreateList();
            RegisterWireOffsets();
            RegisterDescriptions();
            CreateInputMenu();
            CreateOutputMenu();
        }

        private void CreateList()
        {
            for (long i = 0; i < 32753; i++)
            {
                EEPROMRow row = new EEPROMRow(i.ToString("X"));

                if (i == 0)
                {
                    row.Zero = "AA";
                    row.One = "07";
                }

                _rows.Add(row);
            }
        }

        private void CreateInputMenu()
        {
            _inputMenu = new ContextMenu();
            if (_inputWireOffsets.Count > 0)
            {
                foreach (var item in _inputWireOffsets.Keys)
                {
                    // TODO: Add check in here to see if key is used, if so, then dont add
                    if (!_inputWires.ContainsKey(item))
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Header = "Input " + item + " - " + _pinDescriptions[item];
                        menuItem.Click += (obj, e) =>
                        {
                            WireOffset wireOffset = _inputWireOffsets[item];
                            RegisterInputWire(item, _wireStatus.GetWire());
                            _wireStatus.SetEnd(Canvas.GetLeft(_image) + wireOffset.XOffset,
                                Canvas.GetTop(_image) + wireOffset.YOffset, this);
                        };
                        _inputMenu.Items.Add(menuItem);
                    }
                }
            }
        }

        private void CreateOutputMenu()
        {
            _outputMenu = new ContextMenu();
            if (_outputWireOffsets.Count > 0)
            {
                foreach (var item in _outputWireOffsets.Keys)
                {
                    if (!_outputWires.ContainsKey(item))
                    {
                        // TODO: Add check in here to see if key is used, if so, then dont add
                        MenuItem menuItem = new MenuItem();
                        menuItem.Header = "Output " + item + " - " + _pinDescriptions[item];
                        menuItem.Click += (obj, e) =>
                        {
                            WireOffset wireOffset = _outputWireOffsets[item];
                            _wireStatus.SetStart(Canvas.GetLeft(_image) + wireOffset.XOffset,
                                Canvas.GetTop(_image) + wireOffset.YOffset);
                            RegisterOutputWire(item, _wireStatus.GetWire());
                        };
                        _outputMenu.Items.Add(menuItem);
                    }
                }
            }
        }

        /// <summary>
        /// Pin 1 - A7                      Pin 24 - Vcc (NC for this)
        /// Pin 2 - A7                      Pin 23 - A8
        /// Pin 3 - A7                      Pin 22 - A9
        /// Pin 4 - A7                      Pin 21 - Write Enable (Active Low)
        /// Pin 5 - A7                      Pin 20 - Output Enable (Active Low)
        /// Pin 6 - A7                      Pin 19 - A10
        /// Pin 7 - A7                      Pin 18 - Chip Enable (Active Low)
        /// Pin 8 - A7                      Pin 17 - I/O 7
        /// Pin 9 - I/O 0                   Pin 16 - I/O 6
        /// Pin 10 - I/O 1                  Pin 15 - I/O 5
        /// Pin 11 - I/O 2                  Pin 14 - I/O 4
        /// Pin 12 - Vss (NC for this)      Pin 13 - I/O 3
        /// </summary>
        /// 
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
            _pinDescriptions.Add(9, "A7");
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
            _inputWireOffsets.Add(1, new WireOffset { XOffset = 10, YOffset = 50 });
            _inputWireOffsets.Add(2, new WireOffset { XOffset = 20, YOffset = 50 });
            _inputWireOffsets.Add(3, new WireOffset { XOffset = 30, YOffset = 50 });
            _inputWireOffsets.Add(4, new WireOffset { XOffset = 40, YOffset = 50 });
            _inputWireOffsets.Add(5, new WireOffset { XOffset = 50, YOffset = 50 });
            _inputWireOffsets.Add(6, new WireOffset { XOffset = 60, YOffset = 50 });
            _inputWireOffsets.Add(7, new WireOffset { XOffset = 70, YOffset = 50 });
            _inputWireOffsets.Add(8, new WireOffset { XOffset = 80, YOffset = 50 });
            _outputWireOffsets.Add(9, new WireOffset { XOffset = 90, YOffset = 50 });
            _outputWireOffsets.Add(10, new WireOffset { XOffset = 100, YOffset = 50 });
            _outputWireOffsets.Add(11, new WireOffset { XOffset = 110, YOffset = 50 });
            _inputWireOffsets.Add(12, new WireOffset { XOffset = 120, YOffset = 50 });
            _outputWireOffsets.Add(13, new WireOffset { XOffset = 120, YOffset = 10 });
            _outputWireOffsets.Add(14, new WireOffset { XOffset = 110, YOffset = 10 });
            _outputWireOffsets.Add(15, new WireOffset { XOffset = 100, YOffset = 10 });
            _outputWireOffsets.Add(16, new WireOffset { XOffset = 90, YOffset = 10 });
            _outputWireOffsets.Add(17, new WireOffset { XOffset = 80, YOffset = 10 });
            _inputWireOffsets.Add(18, new WireOffset { XOffset = 70, YOffset = 10 });
            _inputWireOffsets.Add(19, new WireOffset { XOffset = 60, YOffset = 10 });
            _inputWireOffsets.Add(20, new WireOffset { XOffset = 50, YOffset = 10 });
            _inputWireOffsets.Add(21, new WireOffset { XOffset = 40, YOffset = 10 });
            _inputWireOffsets.Add(22, new WireOffset { XOffset = 30, YOffset = 10 });
            _inputWireOffsets.Add(23, new WireOffset { XOffset = 20, YOffset = 10 });
            _inputWireOffsets.Add(24, new WireOffset { XOffset = 10, YOffset = 10 });
        }

        private void RegisterInputWire(int port, Wire wire)
        {
            _inputWires.Add(port, wire);
        }

        private void RegisterOutputWire(int port, Wire wire)
        {
            _outputWires.Add(port, wire);
        }

        public void CreateControl()
        {
            _image = new System.Windows.Controls.Image
            {
                Source = UIIconConverter.BitmapToBitmapImage(Logic_Resources._28C16),
                Stretch = System.Windows.Media.Stretch.Fill,
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
            foreach (KeyValuePair<int, Wire> inputWirePair in _inputWires)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.INPUT, inputWirePair.Key.ToString());
                writer.WriteElementString(SaveLoadTags.WIRE_ID, inputWirePair.Value.ID.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement(SaveLoadTags.OUTPUT_WIRES_NODE);
            foreach (KeyValuePair<int, Wire> outputWirePair in _outputWires)
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
            _inputWires = inputWires;
            _outputWires = outputWires;
        }

        public void SetPlaced()
        {
            Canvas.SetZIndex(_image, 3);

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
    }
}
