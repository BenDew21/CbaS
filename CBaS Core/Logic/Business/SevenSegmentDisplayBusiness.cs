using System.Collections.Generic;
using CBaSCore.Displays.UI.Controls;

namespace CBaSCore.Logic.Business
{
    public class SevenSegmentDisplayBusiness : IWireBusinessObserver
    {
        public bool SegmentA { get; set; }
        public bool SegmentB { get; set; }
        public bool SegmentC { get; set; }
        public bool SegmentD { get; set; }
        public bool SegmentE { get; set; }
        public bool SegmentF { get; set; }
        public bool SegmentG { get; set; }
        public bool SegmentDP { get; set; }
        public bool IsCommonCathode { get; set; }
        
        private Dictionary<int, WireBusiness> _inputWires = new();

        private readonly Dictionary<int, bool> _wireToSegment = new();

        private SegmentedDisplay _parent;
        
        public SevenSegmentDisplayBusiness()
        {
            IsCommonCathode = true;
            RegisterMappings();
        }

        public void SetParent(SegmentedDisplay parent)
        {
            _parent = parent;
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
        
        public void UpdateSegments()
        {
            if (!_inputWires.ContainsKey(8)) return;
            
            var typeWire = _inputWires[8];
            if (typeWire.Status ^ IsCommonCathode)
            {
                foreach (var (key, _) in _wireToSegment)
                {
                    if (_inputWires.ContainsKey(key))
                    {
                        UpdateSegment(key, _inputWires[key].Status);
                    }
                }
            }
            else
            {
                foreach (var (key, _) in _wireToSegment)
                {
                    if (_inputWires.ContainsKey(key))
                    {
                        UpdateSegment(key, !_inputWires[key].Status);
                    }
                }
            }

            _parent?.Update();
        }

        private void UpdateSegment(int key, bool value)
        {
            switch (key)
            {
                case 1:
                    SegmentE = value;
                    break;
                case 2:
                    SegmentD = value;
                    break;
                case 4:
                    SegmentC = value;
                    break;
                case 5:
                    SegmentDP = value;
                    break;
                case 6:
                    SegmentB = value;
                    break;
                case 7:
                    SegmentA = value;
                    break;
                case 9:
                    SegmentF = value;
                    break;
                case 10:
                    SegmentG = value;
                    break;
            }
        }

        public void RegisterInputWire(int key, WireBusiness wireBusiness)
        {
            _inputWires.Add(key, wireBusiness);
        }
        
        public void WireStatusChanged(WireBusiness wire, bool status)
        {
            UpdateSegments();
        }
    }
}