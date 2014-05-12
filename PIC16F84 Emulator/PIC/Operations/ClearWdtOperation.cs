using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class ClearWdtOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following instruction:
         *      > CLRWDT
         *   Simply create a new instance with the corresponding address and call execute
         */

        private short targetAddress;
        private const short CYCLES = 1;
        private PIC pic;

        public ClearWdtOperation(PIC _pic, RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.pic = _pic;
        }

        public override void execute()
        {
            // STEP 1: clear WDT
            pic.resetWDT();
            // STEP 2: update Status Flags                
            registerFileMap.setBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_TO_MASK);
            registerFileMap.setBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_PD_MASK);
            registerFileMap.clearBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.OPTION_PS2_MASK);
            registerFileMap.clearBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.OPTION_PS1_MASK);
            registerFileMap.clearBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.OPTION_PS0_MASK);
        }
    }
}
