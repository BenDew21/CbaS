namespace CBaSCore.Logic.UI.Utility_Classes
{
    public class EEPROMRow
    {
        public string Row { get; set; }
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
            Row = address;
            Zero = "00";
            One = "00";
            Two = "00";
            Three = "00";
            Four = "00";
            Five = "00";
            Six = "00";
            Seven = "00";
            Eight = "00";
            Nine = "00";
            A = "00";
            B = "00";
            C = "00";
            D = "00";
            E = "00";
            F = "00";
        }

        public string GetValueInRegister(string column)
        {
            switch (column)
            {
                case "0":
                    return Zero;

                case "1":
                    return One;

                case "2":
                    return Two;

                case "3":
                    return Three;

                case "4":
                    return Four;

                case "5":
                    return Five;

                case "6":
                    return Six;

                case "7":
                    return Seven;

                case "8":
                    return Eight;

                case "9":
                    return Nine;

                case "A":
                    return A;

                case "B":
                    return B;

                case "C":
                    return C;

                case "D":
                    return D;

                case "E":
                    return E;

                case "F":
                    return F;

                default:
                    return "";
            }
        }

        public void SetValueInRegister(string column, string newValue)
        {
            switch (column)
            {
                case "0":
                    Zero = newValue;
                    break;

                case "1":
                    One = newValue;
                    break;

                case "2":
                    Two = newValue;
                    break;

                case "3":
                    Three = newValue;
                    break;

                case "4":
                    Four = newValue;
                    break;

                case "5":
                    Five = newValue;
                    break;

                case "6":
                    Six = newValue;
                    break;

                case "7":
                    Seven = newValue;
                    break;

                case "8":
                    Eight = newValue;
                    break;

                case "9":
                    Nine = newValue;
                    break;

                case "A":
                    A = newValue;
                    break;

                case "B":
                    B = newValue;
                    break;

                case "C":
                    C = newValue;
                    break;

                case "D":
                    D = newValue;
                    break;

                case "E":
                    E = newValue;
                    break;

                case "F":
                    F = newValue;
                    break;
            }
        }
    }
}