using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Handlers;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Displays.UI.Controls
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

        // Wires
        private Dictionary<int, Wire> _inputWires = new Dictionary<int, Wire>();

        // Wire pixel offsets
        private Dictionary<int, WireOffset> _inputWireOffsets = new Dictionary<int, WireOffset>();

        private Dictionary<int, Shape> _wireToSegment = new Dictionary<int, Shape>();

        private SolidColorBrush _emptySegment = new SolidColorBrush(Color.FromRgb(202, 198, 193));

        private SolidColorBrush _activeSegment = new SolidColorBrush(Color.FromRgb(255, 0, 0));

        public SegmentedDisplay()
        {
            InitializeComponent();
            RegisterOffsets();
            RegisterMappings();
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
        }

        private void UpdateSegments()
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
                            _inputWires.Add(item, _wireStatus.GetWire());
                            _wireStatus.SetEnd(Canvas.GetLeft(this) + wireOffset.XOffset,
                                Canvas.GetTop(this) + wireOffset.YOffset, this);
                        };
                        _inputMenu.Items.Add(menuItem);
                    }
                }
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
            if (_wireStatus.GetSelected() && _wireStatus.GetWire() != null)
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
