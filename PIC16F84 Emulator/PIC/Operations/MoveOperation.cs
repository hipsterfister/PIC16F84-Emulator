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
        private byte data;
        private short targetAddress;

        private const short CYCLES = 1;

        public MoveOperation(byte _data, short _targetAddress, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.data = _data;
            this.targetAddress = _targetAddress;
        }

        public MoveOperation(short _sourceAddress, short _targetAddress, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
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
