using Combinatorics_Calculator.Drawing.UI.Controls;
using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Logic.UI.Controls;
using Combinatorics_Calculator.Logic.UI.Controls.Logic_Gates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Combinatorics_Calculator.Logic.UI.Utility_Classes
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
                case "DiagramLabel":
                    return new DiagramLabel();
                default:
                    return null;
            }
        }
    }
}
