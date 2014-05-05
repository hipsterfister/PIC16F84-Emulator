using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class LogicOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following (logical) Instructions: 
         *      > ANDWF, ANDLW
         *      > IORWF, IORLW
         *      > XORWF, XORLW
         *  Simply create a new instance and call execute() 
         */
        private byte arg1, arg2;
        private LogicOperator op;
        private short targetAddress;

        private const short CYCLES = 1;

        public LogicOperation(byte _arg1, byte _arg2, LogicOperator _op, short _targetAddress, RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.op = _op;
            this.targetAddress = _targetAddress;
        }

        public override void execute()
        {
            byte result = 0x0;
            switch (op)
            {
                case LogicOperator.AND:
                    result = (byte)(arg1 & arg2);
                    break;
                case LogicOperator.IOR:
                    result = (byte)(arg1 | arg2);
                    break;
                case LogicOperator.XOR:
                    result = (byte)(arg1 ^ arg2);
                    break;
                default:
                    break;
            }

            registerFileMap.updateZeroFlag(result == 0);

            registerFileMap.Set(result, targetAddress);
        }
    }

    public enum LogicOperator
    {
        AND,
        IOR,
        XOR
    }
}
