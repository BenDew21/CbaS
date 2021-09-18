using CBaSCore.Logic.Business.Gate_Business;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Base_Classes;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.UI.Controls.Logic_Gates
{
    public class NOTGate : BaseGate<NotGateWireBusiness>
    {
        public NOTGate() : base(Logic_Resources.NOT)
        {
        }

        protected override void RegisterOffsets()
        {
            inputWireOffsets.Add(1, new WireOffset {XOffset = 0, YOffset = 20});
            outputWireOffsets.Add(1, new WireOffset {XOffset = 60, YOffset = 20});
        }

        protected override BaseGate<NotGateWireBusiness> GetNewControl()
        {
            return new NOTGate();
        }
    }
}