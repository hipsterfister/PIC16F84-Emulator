using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class LogicOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following (logical) Instructions: 
         *      > ANDWF, ANDLW
         *      > IORWF, IORLW
         *      > XORWF, XORLW
         *  Simply create a new instance and call execute() 
         */
        byte arg1, arg2;
        LogicOperator op;
        short targetAddress;

        public LogicOperation(byte _arg1, byte _arg2, LogicOperator _op, short _targetAddress, RegisterFileMap _registerFileMap) :
            base(_registerFileMap)
        {
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.op = _op;
            this.targetAddress = _targetAddress;
        }

        public void execute()
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
            }

            if (result == 0)
            {
                registerFileMap.setZeroFlag();
            }
            else
            {
                registerFileMap.clearZeroFlag();
            }

            registerFileMap.Set(result, targetAddress);
        }
    }

    enum LogicOperator
    {
        AND,
        IOR,
        XOR
    }
}
