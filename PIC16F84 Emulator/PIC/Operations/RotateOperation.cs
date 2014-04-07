using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class RotateOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following operations:
         *      > RLF
         *      > RRF
         *  Simply create a new instance and call execute()
         */

        byte data;
        short targetAddress;
        RotationDirection direction;

        public RotateOperation(short _sourceAddress, short _targetAddress, RotationDirection _direction, Register.RegisterFileMap _registerFileMap) :
            base(_registerFileMap)
        {
            this.data = _registerFileMap.Get(_sourceAddress);
            this.targetAddress = _targetAddress;
            this.direction = _direction;
        }

        public void execute()
        {
            int temp;
            byte result = 0x00;
            byte statusRegister = registerFileMap.Get(Register.RegisterConstants.STATUS_ADDRESS);
            byte carry = (byte)(statusRegister | 0x01);

            switch (direction)
            {
                case RotationDirection.LEFT:
                    temp = data << 1;
                    temp += carry;
                    result = (byte)(temp & 0xFF);
                    carry = (byte) (temp / 0xFF);
                    break;
                case RotationDirection.RIGHT:
                    temp = data >> 1; // least significant bit is lost
                    temp = data | (carry * 0x80); // cary * 0x80 = C000.0000
                    carry = (byte) (data % 2); // carry = least significant bit of data
                    result = (byte)temp;
                    break;
            }

            if (carry == 0)
            {
                registerFileMap.clearCarryFlag();
            }
            else
            {
                registerFileMap.setCarryFlag();
            }

            registerFileMap.Set(result, targetAddress);
        }

    }

    enum RotationDirection {
        LEFT,
        RIGHT
    }
}
