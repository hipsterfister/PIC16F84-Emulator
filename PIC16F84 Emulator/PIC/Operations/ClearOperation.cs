using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class ClearOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following instructions:
         *      > CLRF
         *      > CLRW
         *      > CLRWDT
         *   Simply create a new instance with the corresponding address and call execute
         */

        private short targetAddress;
        private const short CYCLES = 1;

        public ClearOperation(short _targetAddress, RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.targetAddress = _targetAddress;
        }

        public override void execute()
        {
            registerFileMap.Set(0x00, targetAddress);

            if (targetAddress == RegisterConstants.WDT_REGISTER_ADDRESS)
            {
                registerFileMap.setBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_TO_MASK);
                registerFileMap.setBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_PD_MASK);
                registerFileMap.clearBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.OPTION_PS2_MASK);
                registerFileMap.clearBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.OPTION_PS1_MASK);
                registerFileMap.clearBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.OPTION_PS0_MASK);
            }
            else
            {
                registerFileMap.setZeroFlag();
            }
        }
    }
}
