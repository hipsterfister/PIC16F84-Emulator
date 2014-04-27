using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    public class ProgamMemory
    {
        protected DataAdapter<short>[] programMemory;
        public ProgamMemory()
        {
            programMemory = new DataAdapter<short>[ProgamMemoryConstants.SIZE_OF_PROGRAM_MEMORY];
            for (int x = 0; x < ProgamMemoryConstants.SIZE_OF_PROGRAM_MEMORY; x++)
            {
                programMemory[x] = new DataAdapter<short>();
                programMemory[x].Value = 0;
            }
        }

        public void readFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            short value = 0;
            short address = 0;
            string tempAddress = "";
            string tempValue = "";
            for (int x = 0; x < lines.Length; x++)
            {
                tempAddress = lines[x].Substring(0, 4);
                tempValue = lines[x].Substring(5, 4);
                tempAddress = tempAddress.Trim();

                if (tempAddress != "")
                {
                    try
                    {
                        address = Int16.Parse(tempAddress, System.Globalization.NumberStyles.HexNumber);
                        value = Int16.Parse(tempValue, System.Globalization.NumberStyles.HexNumber);
                    }
                    catch (Exception)
                    {
                        new Exception("Das Program-Listing enthält fehlerhafte Zeichenketten (Zeile: " + x + ")");
                    }
                }
                programMemory[address].Value = value;
            }
        }

        public short this [short address]
        {
            get
            {
                return programMemory[address].Value;
            }
            set
            {
                programMemory[address].Value = value;
            }
        }
    }

    class ProgamMemoryConstants
    {
        public const short SIZE_OF_PROGRAM_MEMORY = 1024; // datasheet 2.1 program memory organization
        public const short RESET_VECTOR_ADDRESS = 0;
        public const short INTERRUPT_VECTOR_ADDRESS = 4;
    }
}
