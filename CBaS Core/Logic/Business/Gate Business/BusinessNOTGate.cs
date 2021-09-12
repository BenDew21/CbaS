namespace CBaSCore.Logic.Business.Gate_Business
{
    public class BusinessNOTGate : BaseBusinessGate
    {
        public override void CalculateOutput()
        {
            var input = inputWires[1].Status;
            if (outputWires.ContainsKey(1)) outputWires[1].ToggleStatus(!input);
        }
    }
}