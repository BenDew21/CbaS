using System.Diagnostics;
using CBaSCore.Logic.Business;
using CBaSCore.Logic.Business.Gate_Business;
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
            var input = new BusinessInputControl();
            var output = new BusinessOutputControl();

            var wire = new BusinessWire {ID = 1};

            input.SetOutputWire(wire);
            output.SetInputWire(wire);

            Assert.AreEqual(false, input.Outputting, "Expecting input to not be outputting");
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting");

            input.Outputting = true;
            
            Assert.AreEqual(true, input.Outputting, "Expecting input to be outputting");
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting");

            input.Outputting = false;
            
            Assert.AreEqual(false, input.Outputting, "Expecting input to not be outputting");
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting");
        }

        [Test]
        public void Test_Double_Wire_Input_Output()
        {
            var input = new BusinessInputControl();
            var output = new BusinessOutputControl();

            var wire = new BusinessWire {ID = 1};
            var wire2 = new BusinessWire {ID = 2};

            wire.AddOutputWire(wire2);
            
            input.SetOutputWire(wire);
            output.SetInputWire(wire2);

            Assert.AreEqual(false, input.Outputting, "Expecting input to not be outputting");
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting");

            input.Outputting = true;
            
            Assert.AreEqual(true, input.Outputting, "Expecting input to be outputting");
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting");

            input.Outputting = false;
            
            Assert.AreEqual(false, input.Outputting, "Expecting input to not be outputting");
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting");
        }
        
        [Test]
        public void Test_Double_Wire_Same_ID_Input_Output()
        {
            var input = new BusinessInputControl();
            var output = new BusinessOutputControl();

            var wire = new BusinessWire {ID = 1};
            var wire2 = new BusinessWire {ID = 2};

            wire.AddOutputWire(wire2);
            
            input.SetOutputWire(wire);
            output.SetInputWire(wire2);

            Assert.AreEqual(false, input.Outputting, "Expecting input to not be outputting");
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting");

            input.Outputting = true;
            
            Assert.AreEqual(true, input.Outputting, "Expecting input to be outputting");
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting");
        }

        [Test]
        public void Test_NOT_Gate()
        {
            var input = new BusinessInputControl();
            var notGate = new BusinessNOTGate();
            var output = new BusinessOutputControl();
            
            var wire = new BusinessWire {ID = 1};
            var wire2 = new BusinessWire {ID = 2};

            input.SetOutputWire(wire);
            wire.RegisterWireObserver(notGate);
            notGate.AddInputWire(1, wire);
            notGate.AddOutputWire(1, wire2);
            output.SetInputWire(wire2);

            input.Outputting = true;
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - high input");
            
            input.Outputting = false;
            Assert.AreEqual(true, output.Outputting, "Expecting output to not outputting - low input");
        }

        [Test]
        public void Test_AND_Gate()
        {
            var input1 = new BusinessInputControl();
            var input2 = new BusinessInputControl();
            
            var andGate = new BusinessANDGate();
            
            var output = new BusinessOutputControl();
            
            var inputWire1 = new BusinessWire {ID = 1};
            var inputWire2 = new BusinessWire {ID = 2};
            var outputWire = new BusinessWire {ID = 3};

            input1.SetOutputWire(inputWire1);
            input2.SetOutputWire(inputWire2);
            inputWire1.RegisterWireObserver(andGate);
            inputWire2.RegisterWireObserver(andGate);
            andGate.AddInputWire(1, inputWire1);
            andGate.AddInputWire(2, inputWire2);
            andGate.AddOutputWire(1, outputWire);
            output.SetInputWire(outputWire);

            input1.Outputting = true;
            input2.Outputting = false;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - one high one low input");
            
            input1.Outputting = false;
            input2.Outputting = true;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - one high one low input");
            
            input1.Outputting = true;
            input2.Outputting = true;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - two high inputs");
            
            input1.Outputting = false;
            input2.Outputting = false;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - two low inputs");
        }
        
        [Test]
        public void Test_OR_Gate()
        {
            var input1 = new BusinessInputControl();
            var input2 = new BusinessInputControl();
            
            var orGate = new BusinessORGate();
            
            var output = new BusinessOutputControl();
            
            var inputWire1 = new BusinessWire {ID = 1};
            var inputWire2 = new BusinessWire {ID = 2};
            var outputWire = new BusinessWire {ID = 3};

            input1.SetOutputWire(inputWire1);
            input2.SetOutputWire(inputWire2);
            inputWire1.RegisterWireObserver(orGate);
            inputWire2.RegisterWireObserver(orGate);
            orGate.AddInputWire(1, inputWire1);
            orGate.AddInputWire(2, inputWire2);
            orGate.AddOutputWire(1, outputWire);
            output.SetInputWire(outputWire);

            input1.Outputting = true;
            input2.Outputting = false;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - one high one low input");
            
            input1.Outputting = false;
            input2.Outputting = true;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - one high one low input");
            
            input1.Outputting = true;
            input2.Outputting = true;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - two high inputs");
            
            input1.Outputting = false;
            input2.Outputting = false;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - two low inputs");
        }

        [Test]
        public void Test_NAND_Gate()
        {
            var input1 = new BusinessInputControl();
            var input2 = new BusinessInputControl();
            
            var orGate = new BusinessNANDGate();
            
            var output = new BusinessOutputControl();
            
            var inputWire1 = new BusinessWire {ID = 1};
            var inputWire2 = new BusinessWire {ID = 2};
            var outputWire = new BusinessWire {ID = 3};

            input1.SetOutputWire(inputWire1);
            input2.SetOutputWire(inputWire2);
            inputWire1.RegisterWireObserver(orGate);
            inputWire2.RegisterWireObserver(orGate);
            orGate.AddInputWire(1, inputWire1);
            orGate.AddInputWire(2, inputWire2);
            orGate.AddOutputWire(1, outputWire);
            output.SetInputWire(outputWire);

            input1.Outputting = true;
            input2.Outputting = false;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - one high one low input");
            
            input1.Outputting = false;
            input2.Outputting = true;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - one high one low input");
            
            input1.Outputting = true;
            input2.Outputting = true;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - two high inputs");
            
            input1.Outputting = false;
            input2.Outputting = false;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - two low inputs");
        }
        
        [Test]
        public void Test_NOR_Gate()
        {
            var input1 = new BusinessInputControl();
            var input2 = new BusinessInputControl();
            
            var orGate = new BusinessNORGate();
            
            var output = new BusinessOutputControl();
            
            var inputWire1 = new BusinessWire {ID = 1};
            var inputWire2 = new BusinessWire {ID = 2};
            var outputWire = new BusinessWire {ID = 3};

            input1.SetOutputWire(inputWire1);
            input2.SetOutputWire(inputWire2);
            inputWire1.RegisterWireObserver(orGate);
            inputWire2.RegisterWireObserver(orGate);
            orGate.AddInputWire(1, inputWire1);
            orGate.AddInputWire(2, inputWire2);
            orGate.AddOutputWire(1, outputWire);
            output.SetInputWire(outputWire);

            input1.Outputting = true;
            input2.Outputting = false;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - one high one low input");
            
            input1.Outputting = false;
            input2.Outputting = true;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - one high one low input");
            
            input1.Outputting = true;
            input2.Outputting = true;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - two high inputs");
            
            input1.Outputting = false;
            input2.Outputting = false;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - two low inputs");
        }
        
        [Test]
        public void Test_XOR_Gate()
        {
            var input1 = new BusinessInputControl();
            var input2 = new BusinessInputControl();
            
            var orGate = new BusinessXORGate();
            
            var output = new BusinessOutputControl();
            
            var inputWire1 = new BusinessWire {ID = 1};
            var inputWire2 = new BusinessWire {ID = 2};
            var outputWire = new BusinessWire {ID = 3};

            input1.SetOutputWire(inputWire1);
            input2.SetOutputWire(inputWire2);
            inputWire1.RegisterWireObserver(orGate);
            inputWire2.RegisterWireObserver(orGate);
            orGate.AddInputWire(1, inputWire1);
            orGate.AddInputWire(2, inputWire2);
            orGate.AddOutputWire(1, outputWire);
            output.SetInputWire(outputWire);

            input1.Outputting = true;
            input2.Outputting = false;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - one high one low input");
            
            input1.Outputting = false;
            input2.Outputting = true;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - one high one low input");
            
            input1.Outputting = true;
            input2.Outputting = true;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - two high inputs");
            
            input1.Outputting = false;
            input2.Outputting = false;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - two low inputs");
        }
        
        [Test]
        public void Test_XNOR_Gate()
        {
            var input1 = new BusinessInputControl();
            var input2 = new BusinessInputControl();
            
            var orGate = new BusinessXNORGate();
            
            var output = new BusinessOutputControl();
            
            var inputWire1 = new BusinessWire {ID = 1};
            var inputWire2 = new BusinessWire {ID = 2};
            var outputWire = new BusinessWire {ID = 3};

            input1.SetOutputWire(inputWire1);
            input2.SetOutputWire(inputWire2);
            inputWire1.RegisterWireObserver(orGate);
            inputWire2.RegisterWireObserver(orGate);
            orGate.AddInputWire(1, inputWire1);
            orGate.AddInputWire(2, inputWire2);
            orGate.AddOutputWire(1, outputWire);
            output.SetInputWire(outputWire);

            input1.Outputting = true;
            input2.Outputting = false;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - one high one low input");
            
            input1.Outputting = false;
            input2.Outputting = true;
            
            Assert.AreEqual(false, output.Outputting, "Expecting output to not be outputting - one high one low input");
            
            input1.Outputting = true;
            input2.Outputting = true;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - two high inputs");
            
            input1.Outputting = false;
            input2.Outputting = false;
            
            Assert.AreEqual(true, output.Outputting, "Expecting output to be outputting - two low inputs");
        }

        [Test]
        public void Test_Adder()
        {
            var input1 = new BusinessInputControl();
            var input2 = new BusinessInputControl();

            var andOutput = new BusinessOutputControl();
            var xorOutput = new BusinessOutputControl();
            
            var andGate = new BusinessANDGate();
            var xorGate = new BusinessXORGate();

            var andInputWire1 = new BusinessWire {ID = 1};
            var andInputWire2 = new BusinessWire {ID = 2};
            
            var xorInputWire1 = new BusinessWire {ID = 3};
            var xorInputWire2 = new BusinessWire {ID = 4};

            var andOutputWire = new BusinessWire {ID = 5};
            var xorOutputWire = new BusinessWire {ID = 6};
            
            input1.SetOutputWire(andInputWire1);
            input2.SetOutputWire(andInputWire2);
            
            andOutput.SetInputWire(andOutputWire);
            xorOutput.SetInputWire(xorOutputWire);
            
            andInputWire1.RegisterWireObserver(andGate);
            andInputWire2.RegisterWireObserver(andGate);
            
            xorInputWire1.RegisterWireObserver(xorGate);
            xorInputWire2.RegisterWireObserver(xorGate);
            
            andInputWire1.AddOutputWire(xorInputWire1);
            andInputWire2.AddOutputWire(xorInputWire2);
            
            andGate.AddInputWire(1, andInputWire1);
            andGate.AddInputWire(2, andInputWire2);
            andGate.AddOutputWire(1, andOutputWire);
            
            xorGate.AddInputWire(1, xorInputWire1);
            xorGate.AddInputWire(2, xorInputWire2);
            xorGate.AddOutputWire(1, xorOutputWire);
            
            // TEST CASES
            
            input1.Outputting = true;
            input2.Outputting = false;
            
            Assert.AreEqual(false, andOutput.Outputting, "Expecting AND output to not be outputting - one high one low input");
            Assert.AreEqual(true, xorOutput.Outputting, "Expecting XOR output to  be outputting - one high one low input");
            
            input1.Outputting = false;
            input2.Outputting = true;
            
            Assert.AreEqual(false, andOutput.Outputting, "Expecting AND output to not be outputting - one high one low input");
            Assert.AreEqual(true, xorOutput.Outputting, "Expecting XOR output to  be outputting - one high one low input");
            
            input1.Outputting = true;
            input2.Outputting = true;
            
            Assert.AreEqual(true, andOutput.Outputting, "Expecting AND output to be outputting - two high inputs");
            Assert.AreEqual(false, xorOutput.Outputting, "Expecting XOR output to not be outputting - two high inputs");
            
            input1.Outputting = false;
            input2.Outputting = false;
            
            Assert.AreEqual(false, andOutput.Outputting, "Expecting AND output to not be outputting - two low inputs");
            Assert.AreEqual(false, xorOutput.Outputting, "Expecting XOR output to not be outputting - two low inputs");
        }
    }
}