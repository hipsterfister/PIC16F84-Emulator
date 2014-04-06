using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class ComplementOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the followin instruction:
         *      > COMF
         */
        short targetAddress;
        byte data; // needed because targetAddress can differ from sourceAddress....

        ComplementOperation(short _targetAddress, Register.RegisterFileMap _registerFileMap) :
            base(_registerFileMap)
        {
            this.targetAddress = _targetAddress;
            this.data = _registerFileMap.Get(_targetAddress);
        }

        ComplementOperation(short _sourceAddress, short _targetAddress, Register.RegisterFileMap _registerFileMap) :
            base(_registerFileMap)
        {
            this.targetAddress = _targetAddress;
            this.data = _registerFileMap.Get(_sourceAddress);
        }

        public void execute()
        {
            byte result = (byte) (~data);
            byte statusRegister = registerFileMap.Get(Register.RegisterConstants.STATUS_ADDRESS);

            if (result == 0)
            {
                statusRegister = (byte)(statusRegister | 0x04); // set Z (Zero)
            }
            else
            {
                statusRegister = (byte)(statusRegister & 0xFB); // clear Z (Zero)
            }

            registerFileMap.Set(result, targetAddress);
            registerFileMap.Set(statusRegister, Register.RegisterConstants.STATUS_ADDRESS);
        }
    }
}
