﻿namespace CBaSCore.Logic.Business.Gate_Business
{
    public class BusinessANDGate : BaseBusinessGate
    {
        public override void CalculateOutput()
        {
            if (outputWires.ContainsKey(1)) outputWires[1].ToggleStatus(inputWires[1].Status && inputWires[2].Status);
        }
    }
}