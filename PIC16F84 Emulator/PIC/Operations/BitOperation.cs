using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class BitOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following instructions:
         *      > BSF
         *      > BCF
         */
        private short targetAddress;
        private short bitNumber; // [0,7]
        private BitOperator op;

        private const short CYCLES = 1;

        public BitOperation(short _targetAddress, short _bitNumber, BitOperator _op, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.targetAddress = _targetAddress;
            this.bitNumber = _bitNumber;
            this.op = _op;
        }

        public override void execute()
        {
            byte result = registerFileMap.Get(targetAddress);
            byte modifyByte = (byte) (1 << bitNumber); // 0001.0000
            switch (op)
            {
                case BitOperator.BITSET:
                    result = (byte) (result | modifyByte); // xxx1.xxxx
                    break;
                case BitOperator.BITCLEAR:
                    modifyByte = (byte) (0xFF ^ modifyByte); // 0001.0000 XOR 1111.1111 = 1110.1111 <3
                    result = (byte)(result & modifyByte); // xxx0.xxxx
                    break;
                default:
                    break;
            }
            registerFileMap.Set(result, targetAddress);
        }
    }

    public enum BitOperator
    {
        BITSET,
        BITCLEAR
    }
}
