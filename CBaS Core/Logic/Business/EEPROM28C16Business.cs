using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.Business
{
    public class EEPROM28C16Business : IWireBusinessObserver
    {
        public List<EEPROMRow> Rows { get; } = new();

        private const int CHIP_ENABLED_PIN = 18;
        private const int OUTPUT_ENABLED_PIN = 20;
        private const int WRITE_ENABLED_PIN = 21;

        private readonly Dictionary<int, WireBusiness> _inputWires = new();
        private readonly Dictionary<int, WireBusiness> _outputWires = new();
        
        private readonly int[] _addressColumnLines = {5, 6, 7, 8};
        private readonly int[] _addressRowLines = {19, 22, 23, 1, 2, 3, 4};
        private readonly int[] _ioLines = {13, 11, 10, 9, 17, 16, 15, 14};

        public EEPROM28C16Business()
        {
            CreateList();
            LoadFromFile();
        }

        #region Processing

        private void Handle()
        {
            // The EEPROM is accessed by:
            // A0 to A3 - Column selector from 0 to FF with A0 lsb and A3 msb
            // A4 to A10 - Row selector from 0 to 7F0 with A4 lsb and A10 msb

            if (IsActive())
            {
                var rowAndColumn = GetRowAndColumn();

                var rowHex = rowAndColumn.Item1;
                var columnHex = rowAndColumn.Item2;

                var row = Rows.Find(e => e.Row.Equals(rowHex));

                if (row == null) return;
                
                if (ShouldOutput())
                {
                    var value = row.GetValueInRegister(columnHex);
                    // Debug.WriteLine("Value in address: " + value);

                    var lsb = value[1];
                    var msb = value[0];

                    var lower4Val = HexConversions.HexToBinary(Convert.ToString(lsb));
                    var upper4Val = HexConversions.HexToBinary(Convert.ToString(msb));

                    // Debug.WriteLine("upper4Val: " + upper4Val);
                    // Debug.WriteLine("lower4Val: " + lower4Val);

                    Output(upper4Val, lower4Val);
                }
                else if (ShouldInput())
                {
                    var inputHex = GetInputHex();
                    row.SetValueInRegister(columnHex, inputHex);
                }
            }
        }

        #endregion
        
        #region Test Methods, for the purpose of getting it to work

        private void CreateList()
        {
            for (var i = 0; i <= 127; i++) Rows.Add(new EEPROMRow(i.ToString("X")));
        }
        
        private void LoadFromFile()
        {
            var path = @"C:\CC Example Project\Binary Files\HexEEPROMNew.BIN";
            var bytes = File.ReadAllBytes(path);
            var iterator = 0;

            var row = Rows.Find(e => e.Row.Equals(iterator.ToString("X")));

            for (var i = 0; i < bytes.Length; i++)
            {
                var register = (i % 16).ToString("X");
                var value = bytes[i].ToString("X");

                if (value.Length == 1) value = "0" + value;

                if (register.Equals("0") && i > 0)
                {
                    // Debug.WriteLine("");
                    iterator++;
                    row = Rows.Find(e => e.Row.Equals(iterator.ToString("X")));
                }

                if (row == null) break;

                // Debug.Write(register + ": " + value + " ");
                row.SetValueInRegister(register, value);
            }
        }

        #endregion
        
        #region I/O Handlers

        private void Output(string msBinaryString, string lsBinaryString)
        {
            for (var i = 0; i < 4; i++)
            {
                var lowerOutputStatus = lsBinaryString[i];
                var upperOutputStatus = msBinaryString[i];

                // Debug.WriteLine("lowerOutputStatus: {0}", lowerOutputStatus);
                // Debug.WriteLine("upperOutputStatus: {0}", upperOutputStatus);

                _outputWires[_ioLines[i]].ToggleStatus(lowerOutputStatus.Equals('1'));
                _outputWires[_ioLines[i + 4]].ToggleStatus(upperOutputStatus.Equals('1'));
            }
        }

        private string GetInputHex()
        {
            var msBinaryString = "";
            var lsBinaryString = "";

            for (var i = 3; i >= 0; i--) lsBinaryString += _outputWires.ContainsKey(i) && _outputWires[i].Status ? "1" : "0";

            for (var i = 7; i >= 4; i--) msBinaryString += _outputWires.ContainsKey(i) && _outputWires[i].Status ? "1" : "0";

            return HexConversions.BinaryToHex(msBinaryString) + HexConversions.BinaryToHex(lsBinaryString);
        }

        private Tuple<string, string> GetRowAndColumn()
        {
            var inputBinaryRow = _addressRowLines.Aggregate("", (current, address) 
                => current + (_inputWires.ContainsKey(address) && _inputWires[address].Status ? "1" : "0"));

            var inputBinaryColumn = _addressColumnLines.Aggregate("", (current, address) 
                => current + (_inputWires.ContainsKey(address) && _inputWires[address].Status ? "1" : "0"));

            // Debug.WriteLine("Row: {0}, Column {1}", inputBinaryRow, inputBinaryColumn);
            //Debug.WriteLine("Row Hex: {0}, Column Hex {1}", Convert.ToInt16(inputBinaryRow, 2).ToString("X"),
            //    Convert.ToInt16(inputBinaryColumn, 2).ToString("X"));

            var rowHex = Convert.ToInt16(inputBinaryRow, 2).ToString("X");
            var columnHex = Convert.ToInt16(inputBinaryColumn, 2).ToString("X");

            return new Tuple<string, string>(rowHex, columnHex);
        }

        #endregion I/O Handlers
        
        #region Read/Write/Active handlers

        private bool IsActive()
        {
            return _inputWires.ContainsKey(CHIP_ENABLED_PIN) &&
                   !_inputWires[CHIP_ENABLED_PIN].Status;
        }

        private bool WriteEnabled()
        {
            return !_inputWires.ContainsKey(WRITE_ENABLED_PIN)
                   || !_inputWires[WRITE_ENABLED_PIN].Status;
        }

        private bool ShouldOutput()
        {
            return (!_inputWires.ContainsKey(OUTPUT_ENABLED_PIN)
                    || !_inputWires[OUTPUT_ENABLED_PIN].Status)
                   && !WriteEnabled();
        }

        private bool ShouldInput()
        {
            return (!_inputWires.ContainsKey(OUTPUT_ENABLED_PIN)
                    || !_inputWires[OUTPUT_ENABLED_PIN].Status)
                   && WriteEnabled();
        }

        #endregion Read/Write/Active handlers
        
        #region Registering wires

        public void RegisterInputWire(int port, WireBusiness wire)
        {
            _inputWires.Add(port, wire);
            Handle();
        }

        public void RegisterOutputWire(int port, WireBusiness wire)
        {
            _outputWires.Add(port, wire);
            Handle();
        }

        #endregion Registering wires

        #region Interface Methods

        public void WireStatusChanged(WireBusiness wire, bool status)
        {
            Handle();
        }

        #endregion
    }
}