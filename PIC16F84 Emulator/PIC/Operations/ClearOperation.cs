using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class ClearOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following instructions:
         *      > CLRF
         *      > CLRW
         *      > CLRWDT
         *   Simply create a new instance with the corresponding address and call execute();
         */

        private short targetAddress;
        private const short WDT_ADDRESS = 0x00;

        public ClearOperation(short _targetAddress, RegisterFileMap _registerFileMap) :
            base(_registerFileMap)
        {
            this.targetAddress = _targetAddress;
        }

        public void execute()
        {
            registerFileMap.Set(0x00, targetAddress);
            
            byte statusRegister = registerFileMap.Get(RegisterConstants.STATUS_ADDRESS);

            if (targetAddress != WDT_ADDRESS)
            {
                statusRegister = (byte)(statusRegister | 0x04); // set Z (Zero)
            }
            else
            {
                statusRegister = (byte)(statusRegister | 0x1C); // set Z, PD, TO
                // TODO: WDT Prescaler -> 0 (CLRWDT instruction)
            }

            registerFileMap.Set(statusRegister, RegisterConstants.STATUS_ADDRESS);
        }
    }
}
