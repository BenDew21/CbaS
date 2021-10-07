using CBaSCore.Logic.Business.Gate_Business;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Base_Classes;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.UI.Controls.Logic_Gates
{
    public class XNORGate : BaseGate<XnorGateWireBusiness>
    {
        public XNORGate() : base(Logic_Resources.XNOR)
        {
        }
        
        protected override void RegisterOffsets()
        {
            inputWireOffsets.Add(1, new WireOffset {XOffset = 0.0, YOffset = 10.0});
            inputWireOffsets.Add(2, new WireOffset {XOffset = 0.0, YOffset = 30.0});
            outputWireOffsets.Add(1, new WireOffset {XOffset = 60.0, YOffset = 20.0});
        }

        protected override BaseGate<XnorGateWireBusiness> GetNewControl()
        {
            return new XNORGate();
        }
    }
}