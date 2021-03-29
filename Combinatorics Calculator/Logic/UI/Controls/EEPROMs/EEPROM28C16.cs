using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Handlers;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Logic.UI.Controls.EEPROMs
{
    public class EEPROM28C16 : ICanvasElement, IWireObserver, IActivatableControl
    {
        private static int CHIP_ENABLED_PIN = 18;
        private static int OUTPUT_ENABLED_PIN = 20;
        private static int WRITE_ENABLED_PIN = 21;

        private bool _isActive = false;

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
        private int[] _addressColumnLines = { 5, 6, 7, 8 };
        private int[] _addressRowLines = { 19, 22, 23, 1, 2, 3, 4 };
        private int[] _ioLines = { 13, 11, 10, 9, 17, 16, 15, 14 };


        public EEPROM28C16()
        {
            CreateControl();
            CreateList();
            RegisterWireOffsets();
            RegisterDescriptions();
            CreateInputMenu();
            CreateOutputMenu();
            LoadFromFile();
        }

        private void CreateList()
        {
            for (int i = 0; i <= 127; i++)
            {
                _rows.Add(new EEPROMRow(i.ToString("X")));
            }
        }

        private void LoadFromFile()
        {
            string path = @"C:\CC Example Project\Binary Files\HexEEPROMNew.BIN";
            byte[] bytes = File.ReadAllBytes(path);
            int iterator = 0;

            EEPROMRow row = _rows.Find(e => e.Row.Equals(iterator.ToString("X")));

            for (int i = 0; i < bytes.Length; i++)
            {
                string register = (i % 16).ToString("X");
                string value = bytes[i].ToString("X");

                if (value.Length == 1) value = "0" + value;

                if (register.Equals("0") && i > 0)
                {
                    // Debug.WriteLine("");
                    iterator++;
                    row = _rows.Find(e => e.Row.Equals(iterator.ToString("X")));
                }

                if (row == null) break;

                // Debug.Write(register + ": " + value + " ");
                row.SetValueInRegister(register, value);
            }
        }



        private void Handle()
        {
            // The EEPROM is accessed by:
            // A0 to A3 - Column selector from 0 to FF with A0 lsb and A3 msb
            // A4 to A10 - Row selector from 0 to 7F0 with A4 lsb and A10 msb

            if (_isActive && IsActive())
            {
                Tuple<string, string> rowAndColumn = GetRowAndColumn();

                string rowHex = rowAndColumn.Item1;
                string columnHex = rowAndColumn.Item2;

                EEPROMRow row = _rows.Find(e => e.Row.Equals(rowHex));

                if (ShouldOutput())
                {
                    string value = row.GetValueInRegister(columnHex);
                    // Debug.WriteLine("Value in address: " + value);

                    char lsb = value[1];
                    char msb = value[0];

                    string lower4Val = HexConversions.HexToBinary(Convert.ToString(lsb));
                    string upper4Val = HexConversions.HexToBinary(Convert.ToString(msb));

                    // Debug.WriteLine("upper4Val: " + upper4Val);
                    // Debug.WriteLine("lower4Val: " + lower4Val);

                    Output(upper4Val, lower4Val);
                }
                else if (ShouldInput())
                {
                    string inputHex = GetInputHex();
                    row.SetValueInRegister(columnHex, inputHex);
                }
            }
        }

        #region I/O Handlers
        private void Output(string msBinaryString, string lsBinaryString)
        {
            for (int i = 0; i < 4; i++)
            {
                char lowerOutputStatus = lsBinaryString[i];
                char upperOutputStatus = msBinaryString[i];

                // Debug.WriteLine("lowerOutputStatus: {0}", lowerOutputStatus);
                // Debug.WriteLine("upperOutputStatus: {0}", upperOutputStatus);

                _outputWires[_ioLines[i]].ToggleStatus(lowerOutputStatus.Equals('1'));
                _outputWires[_ioLines[i + 4]].ToggleStatus(upperOutputStatus.Equals('1'));
            }
        }

        private string GetInputHex()
        {
            string msBinaryString = "";
            string lsBinaryString = "";

            for (int i = 3; i >= 0; i--)
            {
                lsBinaryString += (_outputWires.ContainsKey(i) && _outputWires[i].GetStatus()) ? "1" : "0";
            }

            for (int i = 7; i >= 4; i--)
            {
                msBinaryString += (_outputWires.ContainsKey(i) && _outputWires[i].GetStatus()) ? "1" : "0";
            }

            return HexConversions.BinaryToHex(msBinaryString) + HexConversions.BinaryToHex(lsBinaryString);
        }

        private Tuple<string, string> GetRowAndColumn()
        {
            string inputBinaryRow = "";
            string inputBinaryColumn = "";

            foreach (var address in _addressRowLines)
            {
                inputBinaryRow += (_inputWires.ContainsKey(address) && _inputWires[address].GetStatus()) ? "1" : "0";
            }

            foreach (var address in _addressColumnLines)
            {
                inputBinaryColumn += (_inputWires.ContainsKey(address) && _inputWires[address].GetStatus()) ? "1" : "0";
            }

            // Debug.WriteLine("Row: {0}, Column {1}", inputBinaryRow, inputBinaryColumn);
            //Debug.WriteLine("Row Hex: {0}, Column Hex {1}", Convert.ToInt16(inputBinaryRow, 2).ToString("X"),
            //    Convert.ToInt16(inputBinaryColumn, 2).ToString("X"));

            string rowHex = Convert.ToInt16(inputBinaryRow, 2).ToString("X");
            string columnHex = Convert.ToInt16(inputBinaryColumn, 2).ToString("X");

            return new Tuple<string, string>(rowHex, columnHex);
        }
        #endregion

        #region Read/Write/Active handlers
        private bool IsActive()
        {
            return _inputWires.ContainsKey(EEPROM28C16.CHIP_ENABLED_PIN) &&
                !_inputWires[EEPROM28C16.CHIP_ENABLED_PIN].GetStatus();
        }

        private bool WriteEnabled()
        {
            return (!_inputWires.ContainsKey(EEPROM28C16.WRITE_ENABLED_PIN)
                || !_inputWires[EEPROM28C16.WRITE_ENABLED_PIN].GetStatus());
        }

        private bool ShouldOutput()
        {
            return (!_inputWires.ContainsKey(EEPROM28C16.OUTPUT_ENABLED_PIN)
                || !_inputWires[EEPROM28C16.OUTPUT_ENABLED_PIN].GetStatus())
                && !WriteEnabled();
        }

        private bool ShouldInput()
        {
            return (!_inputWires.ContainsKey(EEPROM28C16.OUTPUT_ENABLED_PIN)
                || !_inputWires[EEPROM28C16.OUTPUT_ENABLED_PIN].GetStatus())
                && WriteEnabled();
        }
        #endregion

        #region Registering wires
        private void RegisterInputWire(int port, Wire wire)
        {
            _inputWires.Add(port, wire);
        }

        private void RegisterOutputWire(int port, Wire wire)
        {
            _outputWires.Add(port, wire);
        }
        #endregion

        #region Register pins
        /// <summary>
        /// Pin 1 - A7                      Pin 24 - Vcc (NC for this)
        /// Pin 2 - A6                      Pin 23 - A8
        /// Pin 3 - A5                      Pin 22 - A9
        /// Pin 4 - A4                      Pin 21 - Write Enable (Active Low)
        /// Pin 5 - A3                      Pin 20 - Output Enable (Active Low)
        /// Pin 6 - A2                      Pin 19 - A10
        /// Pin 7 - A1                      Pin 18 - Chip Enable (Active Low)
        /// Pin 8 - A0                      Pin 17 - I/O 7
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
        #endregion

        #region Creating Context Menus
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
        #endregion

        #region Base methods
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
            else if (e.ClickCount == 2)
            {
                new EEPROMEditor(_rows).Show();
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
            Handle();
        }

        public void Activate()
        {
            _isActive = true;
            Handle();
        }
        #endregion
    }
}

