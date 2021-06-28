using CBaS_Core.Framework.UI.Base_Classes;
using CBaS_Core.Framework.UI.Handlers;
using CBaS_Core.Framework.UI.Utility_Classes;
using CBaS_Core.Logic.UI.Controls.Wiring;
using CBaS_Core.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace CBaS_Core.Displays.UI.Controls
{
    /// <summary>
    /// Interaction logic for SegmentedDisplay.xaml
    /// Wire 1 - G
    /// Wire 2 - F
    /// </summary>
    public partial class SegmentedDisplay : UserControl, ICanvasElement, IWireObserver
    {
        private WireStatus _wireStatus = WireStatus.GetInstance();
        private ContextMenu _inputMenu;
        private MenuItem _selectedItem;
        private ContextMenu _activeTypeMenu = new ContextMenu();
        private bool _isToggling = false;
        private bool _isCommonCathode = true;

        // Wires
        private Dictionary<int, Wire> _inputWires = new Dictionary<int, Wire>();

        // Wire pixel offsets
        private Dictionary<int, WireOffset> _inputWireOffsets = new Dictionary<int, WireOffset>();

        private Dictionary<int, Shape> _wireToSegment = new Dictionary<int, Shape>();

        private SolidColorBrush _emptySegment = new SolidColorBrush(Color.FromRgb(202, 198, 193));

        private SolidColorBrush _activeSegment = new SolidColorBrush(Color.FromRgb(255, 0, 0));

        private Dictionary<string, bool> _activeTypes = new Dictionary<string, bool>();

        public SegmentedDisplay()
        {
            InitializeComponent();
            RegisterOffsets();
            RegisterMappings();
            CreateContextMenu();
        }

        private void RegisterOffsets()
        {
            _inputWireOffsets.Add(1, new WireOffset { XOffset = 10, YOffset = 70 });
            _inputWireOffsets.Add(2, new WireOffset { XOffset = 20, YOffset = 70 });
            _inputWireOffsets.Add(3, new WireOffset { XOffset = 30, YOffset = 70 });
            _inputWireOffsets.Add(4, new WireOffset { XOffset = 40, YOffset = 70 });
            _inputWireOffsets.Add(5, new WireOffset { XOffset = 50, YOffset = 70 });
            _inputWireOffsets.Add(6, new WireOffset { XOffset = 50, YOffset = 10 });
            _inputWireOffsets.Add(7, new WireOffset { XOffset = 40, YOffset = 10 });
            _inputWireOffsets.Add(8, new WireOffset { XOffset = 30, YOffset = 10 });
            _inputWireOffsets.Add(9, new WireOffset { XOffset = 20, YOffset = 10 });
            _inputWireOffsets.Add(10, new WireOffset { XOffset = 10, YOffset = 10 });
        }

        private void RegisterMappings()
        {
            _wireToSegment.Add(1, SegmentE);
            _wireToSegment.Add(2, SegmentD);
            _wireToSegment.Add(4, SegmentC);
            _wireToSegment.Add(5, SegmentDP);
            _wireToSegment.Add(6, SegmentB);
            _wireToSegment.Add(7, SegmentA);
            _wireToSegment.Add(9, SegmentF);
            _wireToSegment.Add(10, SegmentG);

            _activeTypes.Add("Common Cathode", false);
            _activeTypes.Add("Common Anode", true);
        }

        private void UpdateSegments()
        {
            if (_inputWires.ContainsKey(8))
            {
                Wire typeWire = _inputWires[8];
                if (typeWire.GetStatus() ^ _isCommonCathode)
                {
                    foreach (KeyValuePair<int, Shape> pair in _wireToSegment)
                    {
                        Shape shape = pair.Value;
                        if (_inputWires.ContainsKey(pair.Key))
                        {
                            Wire wire = _inputWires[pair.Key];
                            if (wire.GetStatus())
                            {
                                shape.Fill = _activeSegment;
                                shape.Stroke = _activeSegment;
                            }
                            else
                            {
                                shape.Fill = _emptySegment;
                                shape.Stroke = _emptySegment;
                            }
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<int, Shape> pair in _wireToSegment)
                    {
                        Shape shape = pair.Value;
                        shape.Fill = _emptySegment;
                        shape.Stroke = _emptySegment;
                    }
                }
            }
        }

        protected void CreateInputMenu()
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
                        menuItem.Header = "Input " + item;
                        menuItem.Click += (obj, e) =>
                        {
                            WireOffset wireOffset = _inputWireOffsets[item];
                            Debug.WriteLine("Adding {0} to input {1}", _wireStatus.GetWire().ID, item);
                            _inputWires.Add(item, _wireStatus.GetWire());
                            _wireStatus.SetEnd(Canvas.GetLeft(this) + wireOffset.XOffset,
                                Canvas.GetTop(this) + wireOffset.YOffset, this);
                        };
                        _inputMenu.Items.Add(menuItem);
                    }
                }
            }
        }

        private void CreateContextMenu()
        {
            foreach (KeyValuePair<string, bool> activeType in _activeTypes)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.IsCheckable = true;
                menuItem.Header = activeType.Key;

                if (!activeType.Value)
                {
                    _selectedItem = menuItem;
                }

                menuItem.Checked += (sender, e) =>
                {
                    if (!menuItem.Equals(_selectedItem))
                    {
                        _isToggling = true;
                        _selectedItem.IsChecked = false;
                        _selectedItem = menuItem;
                        _isToggling = false;
                        _isCommonCathode = !_isCommonCathode;
                        UpdateSegments();
                    }
                    else
                    {
                        menuItem.IsChecked = true;
                    }
                };

                menuItem.Unchecked += (sender, e) =>
                {
                    if (menuItem.Equals(_selectedItem) && !_isToggling)
                    {
                        menuItem.IsChecked = true;
                    }
                };

                if (!activeType.Value)
                {
                    menuItem.IsChecked = true;
                }
                _activeTypeMenu.Items.Add(menuItem);
            }
        }

        public UIElement GetControl()
        {
            return this;
        }

        public void CreateControl()
        {
            // Intentionally left blank
        }

        public ICanvasElement GetNew()
        {
            return new SegmentedDisplay();
        }

        public void SetPlaced()
        {
            Canvas.SetZIndex(this, 3);
            MouseDown += Control_MouseDown;
            MouseMove += Control_MouseMove;
            MouseUp += Control_MouseUp;
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(SaveLoadTags.CANVAS_ELEMENT_NODE);
            writer.WriteElementString(SaveLoadTags.TYPE, "SegmentedDisplay");
            writer.WriteElementString(SaveLoadTags.TOP, Canvas.GetTop(this).ToString());
            writer.WriteElementString(SaveLoadTags.LEFT, Canvas.GetLeft(this).ToString());
            writer.WriteStartElement(SaveLoadTags.INPUT_WIRES_NODE);

            foreach (var wires in _inputWires)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.INPUT, wires.Key.ToString());
                writer.WriteElementString(SaveLoadTags.WIRE_ID, wires.Value.ID.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            Canvas.SetTop(this, Convert.ToInt32(element.Element("Top").Value));
            Canvas.SetLeft(this, Convert.ToInt32(element.Element("Left").Value));

            _inputWires = inputWires;
            UpdateSegments();
        }

        public void WireStatusChanged(Wire wire, bool status)
        {
            UpdateSegments();
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

            foreach (KeyValuePair<int, WireOffset> offsetPair in _inputWireOffsets)
            {
                _inputWires[offsetPair.Key].SetEnd(topX + offsetPair.Value.XOffset, topY + offsetPair.Value.YOffset, this);
            }
        }

        public void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _activeTypeMenu.IsOpen = true;
                _activeTypeMenu.PlacementTarget = this;
            }
            else if (_wireStatus.GetSelected() && _wireStatus.GetWire() != null)
            {
                CreateInputMenu();
                _inputMenu.IsOpen = true;
                _inputMenu.PlacementTarget = this;
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
    }
}