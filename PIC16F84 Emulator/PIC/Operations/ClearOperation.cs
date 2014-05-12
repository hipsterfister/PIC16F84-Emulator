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
            registerFileMap.setZeroFlag();
        }
    }
}
