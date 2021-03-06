using Combinatorics_Calculator.Framework.UI.Base_Classes;
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
        // Wires
        private Dictionary<int, Wire> _inputWires = new Dictionary<int, Wire>();

        // Wire pixel offsets
        private Dictionary<int, WireOffset> _inputWireOffsets = new Dictionary<int, WireOffset>();

        private Dictionary<int, Rectangle> _wireToSegment = new Dictionary<int, Rectangle>();

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
            _inputWireOffsets.Add(1, new WireOffset { XOffset = 0, YOffset = 150 });
            _inputWireOffsets.Add(2, new WireOffset { XOffset = 20, YOffset = 150 });
            _inputWireOffsets.Add(3, new WireOffset { XOffset = 40, YOffset = 150 });
            _inputWireOffsets.Add(4, new WireOffset { XOffset = 60, YOffset = 150 });
            _inputWireOffsets.Add(5, new WireOffset { XOffset = 80, YOffset = 150 });
            _inputWireOffsets.Add(6, new WireOffset { XOffset = 80, YOffset = 10 });
            _inputWireOffsets.Add(7, new WireOffset { XOffset = 60, YOffset = 10 });
            _inputWireOffsets.Add(8, new WireOffset { XOffset = 40, YOffset = 10 });
            _inputWireOffsets.Add(9, new WireOffset { XOffset = 20, YOffset = 10 });
            _inputWireOffsets.Add(10, new WireOffset { XOffset = 0, YOffset = 10 });
        }

        private void RegisterMappings()
        {
            _wireToSegment.Add(1, SegmentE);
            _wireToSegment.Add(2, SegmentD);
            _wireToSegment.Add(4, SegmentC);
            _wireToSegment.Add(6, SegmentB);
            _wireToSegment.Add(7, SegmentA);
            _wireToSegment.Add(9, SegmentF);
            _wireToSegment.Add(10, SegmentG);
        }

        private void UpdateSegments()
        {
            foreach (KeyValuePair<int, Rectangle> pair in _wireToSegment)
            {
                Rectangle rec = pair.Value;
                if (_inputWires.ContainsKey(pair.Key))
                {
                    Wire wire = _inputWires[pair.Key];
                    if (wire.GetStatus())
                    {
                        rec.Fill = _activeSegment;
                    }
                    else
                    {
                        rec.Fill = _emptySegment;
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
            // Intentionally left blank
        }

        public void Save(XmlWriter writer)
        {
            // Intentionally left blank
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            // Intentionally left blank
        }

        public void WireStatusChanged(Wire wire, bool status)
        {
            UpdateSegments();
        }
    }
}
