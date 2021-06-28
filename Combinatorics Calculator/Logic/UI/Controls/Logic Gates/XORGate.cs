﻿using CBaS_Core.Logic.Resources;
using CBaS_Core.Logic.UI.Base_Classes;

namespace CBaS_Core.Logic.UI.Controls.Logic_Gates
{
    public class XORGate : BaseGate
    {
        public XORGate() : base(Logic_Resources.XOR)
        {
        }

        public override void CalculateOutput()
        {
            if (outputWires.ContainsKey(1))
            {
                outputWires[1].ToggleStatus(inputWires[1].GetStatus() ^ inputWires[2].GetStatus());
            }
        }

        protected override void RegisterOffsets()
        {
            inputWireOffsets.Add(1, new Utility_Classes.WireOffset { XOffset = 0.0, YOffset = 10.0 });
            inputWireOffsets.Add(2, new Utility_Classes.WireOffset { XOffset = 0.0, YOffset = 30.0 });
            outputWireOffsets.Add(1, new Utility_Classes.WireOffset { XOffset = 60.0, YOffset = 20.0 });
        }

        public override BaseGate GetNewControl()
        {
            return new XORGate();
        }
    }
}