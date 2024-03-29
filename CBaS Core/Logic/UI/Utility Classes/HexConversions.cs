﻿using System.Collections.Generic;
using System.Linq;

namespace CBaSCore.Logic.UI.Utility_Classes
{
    public static class HexConversions
    {
        private static readonly Dictionary<string, string> hexToBinary = new()
        {
            {"0", "0000"},
            {"1", "0001"},
            {"2", "0010"},
            {"3", "0011"},
            {"4", "0100"},
            {"5", "0101"},
            {"6", "0110"},
            {"7", "0111"},
            {"8", "1000"},
            {"9", "1001"},
            {"A", "1010"},
            {"B", "1011"},
            {"C", "1100"},
            {"D", "1101"},
            {"E", "1110"},
            {"F", "1111"}
        };

        public static string HexToBinary(string hex)
        {
            return hexToBinary[hex];
        }

        public static string BinaryToHex(string binary)
        {
            return hexToBinary.FirstOrDefault(x => x.Value.Equals(binary)).Key;
        }
    }
}