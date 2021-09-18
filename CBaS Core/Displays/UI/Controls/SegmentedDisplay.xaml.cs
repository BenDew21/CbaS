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
using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.Business;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Displays.UI.Controls
{
    /// <summary>
    ///     Interaction logic for SegmentedDisplay.xaml
    ///     Wire 1 - G
    ///     Wire 2 - F
    /// </summary>
    public partial class SegmentedDisplay : UserControl, ICanvasElement, IWireObserver
    {
        private readonly SolidColorBrush _activeSegment = new(Color.FromRgb(255, 0, 0));
        private readonly ContextMenu _activeTypeMenu = new();

        private readonly Dictionary<string, bool> _activeTypes = new();

        private readonly SolidColorBrush _emptySegment = new(Color.FromRgb(202, 198, 193));
        private ContextMenu _inputMenu;

        // Wire pixel offsets
        private readonly Dictionary<int, WireOffset> _inputWireOffsets = new();

        // Wires
        private readonly Dictionary<int, Wire> _inputWires = new();
        private bool _isToggling;
        private MenuItem _selectedItem;
        private readonly WireStatus _wireStatus = WireStatus.GetInstance();

        private readonly Dictionary<int, Shape> _wireToSegment = new();

        private readonly SevenSegmentDisplayBusiness _business = new();
        
        public SegmentedDisplay()
        {
            InitializeComponent();
            RegisterOffsets();
            RegisterMappings();
            CreateContextMenu();
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
            Panel.SetZIndex(this, 3);
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

            foreach (var pair in inputWires)
            {
                RegisterInputWire(pair.Key, pair.Value);
            }

            _business.UpdateSegments();
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

            foreach (var offsetPair in _inputWireOffsets) _inputWires[offsetPair.Key].SetEnd(topX + offsetPair.Value.XOffset, topY + offsetPair.Value.YOffset, this);
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

        public void WireStatusChanged(Wire wire, bool status)
        {
            
        }

        public IWireBusinessObserver GetBusiness()
        {
            return _business;
        }

        public void Update()
        {
            SegmentA.Fill = _business.SegmentA ? _activeSegment : _emptySegment;
            SegmentB.Fill = _business.SegmentB ? _activeSegment : _emptySegment;
            SegmentC.Fill = _business.SegmentC ? _activeSegment : _emptySegment;
            SegmentD.Fill = _business.SegmentD ? _activeSegment : _emptySegment;
            SegmentE.Fill = _business.SegmentE ? _activeSegment : _emptySegment;
            SegmentF.Fill = _business.SegmentF ? _activeSegment : _emptySegment;
            SegmentG.Fill = _business.SegmentG ? _activeSegment : _emptySegment;
            SegmentDP.Fill = _business.SegmentDP ? _activeSegment : _emptySegment;
        }

        private void UpdateSegmentColour()
        {
            
        }
        
        private void RegisterOffsets()
        {
            _inputWireOffsets.Add(1, new WireOffset {XOffset = 10, YOffset = 70});
            _inputWireOffsets.Add(2, new WireOffset {XOffset = 20, YOffset = 70});
            _inputWireOffsets.Add(3, new WireOffset {XOffset = 30, YOffset = 70});
            _inputWireOffsets.Add(4, new WireOffset {XOffset = 40, YOffset = 70});
            _inputWireOffsets.Add(5, new WireOffset {XOffset = 50, YOffset = 70});
            _inputWireOffsets.Add(6, new WireOffset {XOffset = 50, YOffset = 10});
            _inputWireOffsets.Add(7, new WireOffset {XOffset = 40, YOffset = 10});
            _inputWireOffsets.Add(8, new WireOffset {XOffset = 30, YOffset = 10});
            _inputWireOffsets.Add(9, new WireOffset {XOffset = 20, YOffset = 10});
            _inputWireOffsets.Add(10, new WireOffset {XOffset = 10, YOffset = 10});
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

        protected void CreateInputMenu()
        {
            _inputMenu = new ContextMenu();
            if (_inputWireOffsets.Count > 0)
                foreach (var item in _inputWireOffsets.Keys)
                    // TODO: Add check in here to see if key is used, if so, then dont add
                    if (!_inputWires.ContainsKey(item))
                    {
                        var menuItem = new MenuItem();
                        menuItem.Header = "Input " + item;
                        menuItem.Click += (obj, e) =>
                        {
                            var wireOffset = _inputWireOffsets[item];
                            Debug.WriteLine("Adding {0} to input {1}", _wireStatus.GetWire().ID, item);
                            RegisterInputWire(item, _wireStatus.GetWire());
                            _wireStatus.SetEnd(Canvas.GetLeft(this) + wireOffset.XOffset,
                                Canvas.GetTop(this) + wireOffset.YOffset, this);
                        };
                        _inputMenu.Items.Add(menuItem);
                    }
        }

        private void RegisterInputWire(int key, Wire wire)
        {
            _inputWires.Add(key, wire);
            _business.RegisterInputWire(key, wire.GetBusiness() as WireBusiness);
        }
        
        private void CreateContextMenu()
        {
            foreach (var activeType in _activeTypes)
            {
                var menuItem = new MenuItem();
                menuItem.IsCheckable = true;
                menuItem.Header = activeType.Key;

                if (!activeType.Value) _selectedItem = menuItem;

                menuItem.Checked += (sender, e) =>
                {
                    if (!menuItem.Equals(_selectedItem))
                    {
                        _isToggling = true;
                        _selectedItem.IsChecked = false;
                        _selectedItem = menuItem;
                        _isToggling = false;
                        _business.IsCommonCathode = !_business.IsCommonCathode;
                        _business.UpdateSegments();
                    }
                    else
                    {
                        menuItem.IsChecked = true;
                    }
                };

                menuItem.Unchecked += (sender, e) =>
                {
                    if (menuItem.Equals(_selectedItem) && !_isToggling) menuItem.IsChecked = true;
                };

                if (!activeType.Value) menuItem.IsChecked = true;
                _activeTypeMenu.Items.Add(menuItem);
            }
        }
    }
}