using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class BaseOperation
    {
        protected RegisterFileMap registerFileMap;
        protected short neededCycles;
        protected short address;

        public delegate void execute();

        public short cycles
        {
            get
            {
                return neededCycles;
            }
            set
            {
                cycles = value;
            }
        }

        public BaseOperation(RegisterFileMap _registerFileMap, short _neededCycles, short _address)
        {
            this.registerFileMap = _registerFileMap;
            this.neededCycles = _neededCycles;
            this.address = _address;
        }
    }
}
