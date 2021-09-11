using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Base_Classes;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.UI.Controls.Logic_Gates
{
    public class NOTGate : BaseGate
    {
        public NOTGate() : base(Logic_Resources.NOT)
        {
        }

        protected override void RegisterOffsets()
        {
            inputWireOffsets.Add(1, new WireOffset {XOffset = 0, YOffset = 20});
            outputWireOffsets.Add(1, new WireOffset {XOffset = 60, YOffset = 20});
        }

        public override void CalculateOutput()
        {
            var input = inputWires[1].GetStatus();
            if (outputWires.ContainsKey(1)) outputWires[1].ToggleStatus(!input);
        }

        public override BaseGate GetNewControl()
        {
            return new NOTGate();
        }
    }
}