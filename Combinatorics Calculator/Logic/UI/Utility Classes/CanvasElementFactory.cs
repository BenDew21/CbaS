using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Logic.UI.Controls;
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
                default:
                    return null;
            }
        }
    }
}
