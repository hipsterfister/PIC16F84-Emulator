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
                programMemory[x].Value = 0;
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
