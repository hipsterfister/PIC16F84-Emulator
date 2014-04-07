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
            int temp;
            byte result;

            temp = arg1 + arg2; // needed if temp > 0xFF to set carry flag
            result = (byte)temp;


            if (temp > 0xFF) // overflow
            {
                registerFileMap.setCarryFlag();
            }
            else
            {
                registerFileMap.clearCarryFlag();
            }
            
            if (result == 0)
            {
                registerFileMap.setZeroFlag();
            }
            else
            {
                registerFileMap.clearZeroFlag();
            }


            if( (arg1 % 0x10) + (arg2 % 0x10) > 0xF )   // byte % 0x10 cuts off the 4 most significant bits. If the sum of these is still greater then 0000 1111 (0x0F), a carry out on the lower bits happened
            {
                registerFileMap.setDigitCarry();
            }
            else
            {
                registerFileMap.clearDigitCarry();
            }

            registerFileMap.Set(result, targetAddress);
        }

        public void executeSubtract()
        {
            byte result;
            try
            {
                checked
                {
                    result = (byte)(arg1 - arg2);
                }
                // No exception occured -> result is in range of 1 byte.
                registerFileMap.setCarryFlag(); // for subtraction carry-bit is inverted.
            }
            catch (OverflowException)
            {
                result = (byte)(~(arg1 - arg2) + 1); // 2er Komplement
                registerFileMap.clearCarryFlag();
            }

            if (result == 0)
            {
                registerFileMap.setZeroFlag();
            }
            else
            {
                registerFileMap.clearZeroFlag();
            }

            if ((arg1 % 0x10) > (arg2 % 0x10))
            {
                registerFileMap.setDigitCarry();
            }
            else
            {
                registerFileMap.clearDigitCarry();
            }

            registerFileMap.Set(result, targetAddress);
        }
    }

    enum ArithmeticOperator
    {
        PLUS,
        MINUS
    }
}
