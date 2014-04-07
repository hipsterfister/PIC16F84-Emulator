using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class ArithmeticOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following (arithmetic) Instructions: 
         *      > ADDWF, ADDLW
         *      > INC
         *      > SUBWF, SUBLW
         *      > DEC
         *  Simply create a new instance and call execute() 
         */

        byte arg1, arg2;
        ArithmeticOperator op;
        short targetAddress;
       // currently not in use... tbi bool conditional;

        ArithmeticOperation(byte _arg1, byte _arg2, ArithmeticOperator _op, short _target, RegisterFileMap _registerFileMap) :
            base(_registerFileMap)
        {
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.op = _op;
            this.targetAddress = _target;
            //this.conditional = false;
        }

        ArithmeticOperation(byte _arg1, byte _arg2, ArithmeticOperator _op, short _target, RegisterFileMap _registerFileMap, bool _conditional) :
            base(_registerFileMap)
        {
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.op = _op;
            this.targetAddress = _target;
            //this.conditional = _conditional;
        }

        public void execute()
        {
            // TODO: events?
            switch (op)
            {
                case ArithmeticOperator.PLUS:
                    executeAdd();
                    break;
                case ArithmeticOperator.MINUS:
                    executeSubtract();
                    break;
            }
        }

        public void executeAdd()
        {
            byte statusRegister = registerFileMap.Get(RegisterConstants.STATUS_ADDRESS);
            byte result;
            try
            {
                result = (byte) (arg1 + arg2);
                statusRegister = (byte)(statusRegister & 0xFE); // clear C (Carry) 
            }
            catch (OverflowException)
            {
                result = (byte) (arg1 + arg2 - 0x100);
                statusRegister = (byte)(statusRegister | 0x01); // set C (Carry)
            }

            if (result == 0)
            {
                statusRegister = (byte)(statusRegister | 0x04); // set Z (Zero)
            }
            else
            {
                statusRegister = (byte)(statusRegister & 0xFB); // clear Z (Zero)
            }


            if( (arg1 % 0x10) + (arg2 % 0x10) > 0xF )
            {
                statusRegister = (byte)(statusRegister | 0x02); // set DigitCarry
            }
            else
            {
                statusRegister = (byte)(statusRegister & 0xFD); // clear DigitCarry
            }

            registerFileMap.Set(statusRegister, RegisterConstants.STATUS_ADDRESS);
            registerFileMap.Set(result, targetAddress);
        }

        public void executeSubtract()
        {
            byte statusRegister = registerFileMap.Get(RegisterConstants.STATUS_ADDRESS);
            byte result;
            try
            {
                result = (byte)(arg1 - arg2);
                statusRegister = (byte)(statusRegister & 0xFE); // set C (Carry) 
            }
            catch (OverflowException)
            {
                result = (byte)(arg1 - arg2 + 0x100); // TODO: Ist das so?
                statusRegister = (byte)(statusRegister | 0x01); // set C (Carry)
            }

            if (result == 0)
            {
                statusRegister = (byte)(statusRegister | 0x04); // set Z (Zero)
            }
            else
            {
                statusRegister = (byte)(statusRegister & 0xFB); // clear Z (Zero)
            }

            if ((arg1 % 0x10) > (arg2 % 0x10))
            {
                statusRegister = (byte)(statusRegister | 0x02); // set DigitCarry
            }
            else
            {
                statusRegister = (byte)(statusRegister & 0xFD); // clear DigitCarry
            }

        }
    }

    enum ArithmeticOperator
    {
        PLUS,
        MINUS
    }
}
