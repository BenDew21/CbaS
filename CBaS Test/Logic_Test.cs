using CBaS_Core.Logic.UI.Controls;
using CBaS_Core.Logic.UI.Controls.Wiring;
using CBaS_Core.Logic.UI.Utility_Classes;
using CBaS_Core.Project.Storage;
using NUnit.Framework;

namespace CBaS_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Input_Output()
        {
            var circuit = new Circuit();
            var input = new InputControl();
            var output = new OutputControl();

            var wire = new Wire();

            input.SetOutputWire(wire);
            output.SetInputWire(wire);

            wire.SetStart(0, 0);
            wire.SetEnd(10, 10, output);

            Assert.Pass();
        }
    }
}