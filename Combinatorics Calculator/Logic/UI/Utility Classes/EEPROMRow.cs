using System;
using System.Collections.Generic;
using System.Text;

namespace Combinatorics_Calculator.Logic.UI.Utility_Classes
{
    public class EEPROMRow
    {
        public string Address { get; set; }
        public string Zero { get; set; }
        public string One { get; set; }
        public string Two { get; set; }
        public string Three { get; set; }
        public string Four { get; set; }
        public string Five { get; set; }
        public string Six { get; set; }
        public string Seven { get; set; }
        public string Eight { get; set; }
        public string Nine { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string F { get; set; }

        public EEPROMRow(string address)
        {
            Address = address;
            Zero = "FF";
            One = "FF";
            Two = "FF";
            Three = "FF";
            Four = "FF";
            Five = "FF";
            Six = "FF";
            Seven = "FF";
            Eight = "FF";
            Nine = "FF";
            A = "FF";
            B = "FF";
            C = "FF";
            D = "FF";
            E = "FF";
            F = "FF";
        }
    }
}
