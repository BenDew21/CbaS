using CBaSCore.Logic.Business;
using CBaSCore.Logic.Business.Gate_Business;
using NUnit.Framework;

namespace CBaS_Test.Control_Tests.Segment_Display_Tests
{
    public class SevenSegmentDisplayTest
    {
        private readonly InputControlBusiness _pin1Input = new();
        private readonly InputControlBusiness _pin2Input = new();
        private readonly InputControlBusiness _pin3Input = new();
        private readonly InputControlBusiness _pin4Input = new();
        private readonly InputControlBusiness _pin5Input = new();
        private readonly InputControlBusiness _pin6Input = new();
        private readonly InputControlBusiness _pin7Input = new();
        private readonly InputControlBusiness _pin8Input = new();
        private readonly InputControlBusiness _pin9Input = new();
        private readonly InputControlBusiness _pin10Input = new();

        private readonly WireBusiness _pin1Wire = new();
        private readonly WireBusiness _pin2Wire = new();
        private readonly WireBusiness _pin3Wire = new();
        private readonly WireBusiness _pin4Wire = new();
        private readonly WireBusiness _pin5Wire = new();
        private readonly WireBusiness _pin6Wire = new();
        private readonly WireBusiness _pin7Wire = new();
        private readonly WireBusiness _pin8Wire = new();
        private readonly WireBusiness _pin9Wire = new();
        private readonly WireBusiness _pin10Wire = new();
        
        private readonly SevenSegmentDisplayBusiness _display = new();
        
        [SetUp]
        public void SetUp()
        {
            _pin1Input.SetOutputWire(_pin1Wire);
            _pin2Input.SetOutputWire(_pin2Wire);
            _pin3Input.SetOutputWire(_pin3Wire);
            _pin4Input.SetOutputWire(_pin4Wire);
            _pin5Input.SetOutputWire(_pin5Wire);
            _pin6Input.SetOutputWire(_pin6Wire);
            _pin7Input.SetOutputWire(_pin7Wire);
            _pin8Input.SetOutputWire(_pin8Wire);
            _pin9Input.SetOutputWire(_pin9Wire);
            _pin10Input.SetOutputWire(_pin10Wire);
            
            _display.RegisterInputWire(1, _pin1Wire);
            _display.RegisterInputWire(2, _pin2Wire);
            _display.RegisterInputWire(3, _pin3Wire);
            _display.RegisterInputWire(4, _pin4Wire);
            _display.RegisterInputWire(5, _pin5Wire);
            _display.RegisterInputWire(6, _pin6Wire);
            _display.RegisterInputWire(7, _pin7Wire);
            _display.RegisterInputWire(8, _pin8Wire);
            _display.RegisterInputWire(9, _pin9Wire);
            _display.RegisterInputWire(10, _pin10Wire);
            
            _pin1Wire.RegisterWireObserver(_display);
            _pin2Wire.RegisterWireObserver(_display);
            _pin3Wire.RegisterWireObserver(_display);
            _pin4Wire.RegisterWireObserver(_display);
            _pin5Wire.RegisterWireObserver(_display);
            _pin6Wire.RegisterWireObserver(_display);
            _pin7Wire.RegisterWireObserver(_display);
            _pin8Wire.RegisterWireObserver(_display);
            _pin9Wire.RegisterWireObserver(_display);
            _pin10Wire.RegisterWireObserver(_display);
            
            _pin1Input.Outputting = false;
            _pin2Input.Outputting = false;
            _pin3Input.Outputting = false;
            _pin4Input.Outputting = false;
            _pin5Input.Outputting = false;
            _pin6Input.Outputting = false;
            _pin7Input.Outputting = false;
            _pin8Input.Outputting = false;
            _pin9Input.Outputting = false;
            _pin10Input.Outputting = false;
        }

        /// <summary>
        /// Test One with Common Cathode Display (sections b (4) and c (6) need lighting)
        /// </summary>
        [Test]
        public void One_Common_Cathode_Test()
        {
            // var pin1Input = new InputControlBusiness();
            // var pin2Input = new InputControlBusiness();
            // var pin3Input = new InputControlBusiness();
            // var pin4Input = new InputControlBusiness();
            // var pin5Input = new InputControlBusiness();
            // var pin6Input = new InputControlBusiness();
            // var pin7Input = new InputControlBusiness();
            // var pin8Input = new InputControlBusiness();
            // var pin9Input = new InputControlBusiness();
            // var pin10Input = new InputControlBusiness();
            //
            // var pin1Wire = new WireBusiness();
            // var pin2Wire = new WireBusiness();
            // var pin3Wire = new WireBusiness();
            // var pin4Wire = new WireBusiness();
            // var pin5Wire = new WireBusiness();
            // var pin6Wire = new WireBusiness();
            // var pin7Wire = new WireBusiness();
            // var pin8Wire = new WireBusiness();
            // var pin9Wire = new WireBusiness();
            // var pin10Wire = new WireBusiness();
            //
            // var display = new SevenSegmentDisplayBusiness();
            
            _pin1Input.Outputting = false;
            _pin2Input.Outputting = false;
            _pin3Input.Outputting = false;
            _pin4Input.Outputting = true;
            _pin5Input.Outputting = false;
            _pin6Input.Outputting = true;
            _pin7Input.Outputting = false;
            _pin8Input.Outputting = false;
            _pin9Input.Outputting = false;
            _pin10Input.Outputting = false;

            Assert.AreEqual(false, _display.SegmentA, "Segment A should not be lit");
            Assert.AreEqual(true, _display.SegmentB, "Segment B should be lit");
            Assert.AreEqual(true, _display.SegmentC, "Segment C should be lit");
            Assert.AreEqual(false, _display.SegmentD, "Segment D should not be lit");
            Assert.AreEqual(false, _display.SegmentE, "Segment E should not be lit");
            Assert.AreEqual(false, _display.SegmentF, "Segment F should not be lit");
            Assert.AreEqual(false, _display.SegmentG, "Segment G should not be lit");
            Assert.AreEqual(false, _display.SegmentDP, "Segment DP should not be lit");
        }
    }
}