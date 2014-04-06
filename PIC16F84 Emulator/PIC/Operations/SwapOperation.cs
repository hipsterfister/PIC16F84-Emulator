using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class SwapOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following instruction:
         *      > SWAPF
         */ 
        short targetAddress;
        byte data;

        public SwapOperation(short _sourceAddress, short _targetAddress, Register.RegisterFileMap _registerFileMap) :
            base(_registerFileMap)
        {
            this.data = _registerFileMap.Get(_sourceAddress);
            this.targetAddress = _targetAddress;
        }

        public void execute()
        {
            byte result = (byte)((data << 4) | (data >> 4));
            registerFileMap.Set(result, targetAddress);
        }
    }
}
