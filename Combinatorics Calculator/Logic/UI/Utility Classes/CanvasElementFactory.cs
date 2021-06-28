using CBaS_Core.Displays.UI.Controls;
using CBaS_Core.Drawing.UI.Controls;
using CBaS_Core.Framework.UI.Base_Classes;
using CBaS_Core.Logic.UI.Controls;
using CBaS_Core.Logic.UI.Controls.EEPROMs;
using CBaS_Core.Logic.UI.Controls.Logic_Gates;

namespace CBaS_Core.Logic.UI.Utility_Classes
{
    public class CanvasElementFactory
    {
        public ICanvasElement CreateFromName(string name)
        {
            switch (name)
            {
                case "InputControl":
                    return new InputControl();

                case "OutputControl":
                    return new OutputControl();

                case "ANDGate":
                    return new ANDGate();

                case "NANDGate":
                    return new NANDGate();

                case "ORGate":
                    return new ORGate();

                case "NORGate":
                    return new NORGate();

                case "XORGate":
                    return new XORGate();

                case "XNORGate":
                    return new XNORGate();

                case "NOTGate":
                    return new NOTGate();

                case "SquareWaveGenerator":
                    return new SquareWaveGenerator();

                case "SegmentedDisplay":
                    return new SegmentedDisplay();

                case "DiagramLabel":
                    return new DiagramLabel();

                case "EEPROM28C16":
                    return new EEPROM28C16();

                default:
                    return null;
            }
        }
    }
}