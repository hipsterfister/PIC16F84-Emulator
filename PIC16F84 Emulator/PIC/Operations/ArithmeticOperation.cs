using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class ArithmeticOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following (arithmetic) Instructions: 
         *      > ADDWF, ADDLW
         *      > INC
         *      > SUBWF, SUBLW
         *      > DEC
         *  Simply create a new instance and call execute() 
         */

        private byte arg1, arg2;
        private ArithmeticOperator op;
        private short targetAddress;

        private const short CYCLES = 1;

        public ArithmeticOperation(byte _arg1, byte _arg2, ArithmeticOperator _op, short _target, RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.op = _op;
            this.targetAddress = _target;
        }

        public ArithmeticOperation(byte _arg1, byte _arg2, ArithmeticOperator _op, short _target, RegisterFileMap _registerFileMap, bool _conditional, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.arg1 = _arg1;
            this.arg2 = _arg2;
            this.op = _op;
            this.targetAddress = _target;
        }

        public override void execute()
        {
            switch (op)
            {
                case ArithmeticOperator.PLUS:
                    executeAdd();
                    break;
                case ArithmeticOperator.MINUS:
                    executeSubtract();
                    break;
                default:
                    break;
            }
        }

        public void executeAdd()
        {
            int temp;
            byte result;

            temp = arg1 + arg2; // needed if temp > 0xFF to set carry flag
            result = (byte)temp;

            registerFileMap.updateCarryFlag(temp > 0xFF);
            registerFileMap.updateZeroFlag(result == 0);
            registerFileMap.updateDigitCarry((arg1 % 0x10) + (arg2 % 0x10) > 0xF); 
            // byte % 0x10 cuts off the 4 most significant bits. If the sum of these is still greater then 0000 1111 (0x0F), a carry out on the lower bits happened
            
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
                registerFileMap.setCarryFlag(); // meaning of carry bit is inverted for subtraction
            }
            catch (OverflowException)
            {
                result = (byte)(~(arg2 - arg1) + 1); // 2s-complement
                registerFileMap.clearCarryFlag();
            }

            registerFileMap.updateZeroFlag(result == 0);
            registerFileMap.updateDigitCarry(((arg1 % 0x10) + ((~arg2+1) % 0x10)) > 0xF); // meaning of digit carry bit is inverted for subtraction (like the carry bit)

            registerFileMap.Set(result, targetAddress);
        }
    }

    public enum ArithmeticOperator
    {
        PLUS,
        MINUS
    }
}
