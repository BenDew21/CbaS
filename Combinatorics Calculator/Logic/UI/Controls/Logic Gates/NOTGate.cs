using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Base_Classes;

namespace Combinatorics_Calculator.Logic.UI.Controls.Logic_Gates
{
    public class NOTGate : BaseGate
    {
        public NOTGate() : base(Logic_Resources.NOT)
        {
        }

        protected override void RegisterOffsets()
        {
            inputWireOffsets.Add(1, new Utility_Classes.WireOffset { XOffset = 0, YOffset = 20 });
            outputWireOffsets.Add(1, new Utility_Classes.WireOffset { XOffset = 60, YOffset = 20 });
        }

        public override void CalculateOutput()
        {
            bool input = inputWires[1].GetStatus();
            if (outputWires.ContainsKey(1))
            {
                outputWires[1].ToggleStatus(!input);
            }
        }

        public override BaseGate GetNewControl()
        {
            return new NOTGate();
        }
    }
}