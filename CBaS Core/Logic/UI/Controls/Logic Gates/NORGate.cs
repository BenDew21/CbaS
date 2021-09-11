using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Base_Classes;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.UI.Controls.Logic_Gates
{
    public class NORGate : BaseGate
    {
        public NORGate() : base(Logic_Resources.NOR)
        {
        }

        public override void CalculateOutput()
        {
            if (outputWires.ContainsKey(1)) outputWires[1].ToggleStatus(!(inputWires[1].GetStatus() || inputWires[2].GetStatus()));
        }

        protected override void RegisterOffsets()
        {
            inputWireOffsets.Add(1, new WireOffset {XOffset = 0.0, YOffset = 10.0});
            inputWireOffsets.Add(2, new WireOffset {XOffset = 0.0, YOffset = 30.0});
            outputWireOffsets.Add(1, new WireOffset {XOffset = 60.0, YOffset = 20.0});
        }

        public override BaseGate GetNewControl()
        {
            return new NORGate();
        }
    }
}