using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.GUI
{
    public class ProgramView
    {
        public const int NO_ADDRESS_VALUE = -1;

        private int linesOfCode;
        public List<string> source;
        private short[] address;

        public ProgramView(string pathOfProgramListing)
        {
            string[] lines = System.IO.File.ReadAllLines(pathOfProgramListing, Encoding.GetEncoding(1252));
            linesOfCode = lines.Length;
            source = new List<string>();
            address = new short[linesOfCode];
            string temp = "";
            string tempAddress = "";
            for (int i = 0; i < linesOfCode; i++)
            {
                temp = lines[i];
                temp = temp.Remove(0, 27);
                source.Add(temp);

                tempAddress = lines[i].Substring(0, 4);
                tempAddress = tempAddress.Trim();
                if (tempAddress != "")
                {
                    address[i] = Int16.Parse(tempAddress, System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    address[i] = NO_ADDRESS_VALUE;
                }
            }
        }


        public void print() {
            for (int i = 0; i < linesOfCode; i++)
            {
                System.Console.WriteLine(source[i] + "|||" + address[i]);
            }
        }

        public int getLineByAddress(short _address)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (address[i] == _address)
                    return i;
            }
            return -1;
        }

        public short getAddressByLine(int _line)
        {
            return address[_line];
        }

    }
}
