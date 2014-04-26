using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Register
{
    public class ProgramCounter
    {
        /// <summary>
        /// This class encapsulates the program counter which is split into the registers PCL and PCLATH.
        /// </summary>
        private RegisterFileMap registerFileMap;

        public ProgramCounter(RegisterFileMap _registerFileMap)
        {
            this.registerFileMap = _registerFileMap;
            this.value = 0x00; 
        }

        public short value
        {
            get
            {
                return (short)(registerFileMap.Get(RegisterConstants.PCLATH_ADDRESS) * 0x100 + registerFileMap.Get(RegisterConstants.PCL_ADDRESS));
            }
            set
            {
                registerFileMap.Set((byte)(value % 0x100), RegisterConstants.PCL_ADDRESS);
                registerFileMap.Set((byte)(value / 0x100), RegisterConstants.PCLATH_ADDRESS);
            }
        }
    }
}
