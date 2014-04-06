using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class MoveOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following (Move) Operations:
         *      > MOVF (TODO: Muss implementiert werden)
         *      > MOVWF
         *      > MOVLW
         *  Simply create a new instance and call execute();
         */
        byte data;
        short targetAddress;

        public MoveOperation(byte _data, short _targetAddress, Register.RegisterFileMap _registerFileMap) :
            base(_registerFileMap)
        {
            this.data = _data;
            this.targetAddress = _targetAddress;
        }

        public MoveOperation(short _sourceAddress, short _targetAddress, Register.RegisterFileMap _registerFileMap) :
            base(_registerFileMap)
        {
            this.data = _registerFileMap.Get(_sourceAddress);
            this.targetAddress = _targetAddress;
        }

        public void execute()
        {
            registerFileMap.Set(data, targetAddress);
        }
    }
}
