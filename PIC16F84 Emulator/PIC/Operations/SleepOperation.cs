using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class SleepOperation : BaseOperation
    {
        private const short CYCLES = 1;
        private PIC pic;

        /// <summary>
        /// Creates a new SleepOperation
        /// </summary>
        /// <param name="_pic">needed to put into sleep</param>
        /// <param name="_registerFileMap"></param>
        /// <param name="_address">code address</param>
        public SleepOperation(PIC _pic, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            pic = _pic;
        }

        public override void execute()
        {
            // STEP 1: Stop
            pic.stopExecution();
            // STEP 2: Set Status Bits
            registerFileMap.Set(0, Register.RegisterConstants.WDT_REGISTER_ADDRESS);
            registerFileMap.setBit(Register.RegisterConstants.STATUS_ADDRESS, Register.RegisterConstants.STATUS_TO_MASK);
            registerFileMap.clearBit(Register.RegisterConstants.STATUS_ADDRESS, Register.RegisterConstants.STATUS_PD_MASK);
        }
    }
}