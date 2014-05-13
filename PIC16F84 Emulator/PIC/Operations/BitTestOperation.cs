using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class BitTestOperation : BaseOperation
    {
        private const short CYCLES = 1;
        private BitTestOperator op;
        private Register.ProgramCounter programCounter;
        private bool bitValue;

        /// <summary>
        /// Creates a new BitTestOperation (1,2 Cycle Instruction)
        /// </summary>
        /// <param name="_sourceAddress">The byte's address containing the targeted bit</param>
        /// <param name="_bitNumber">[0,7]</param>
        /// <param name="_op">BTFSC / BTFSS</param>
        /// <param name="_registerFileMap"></param>
        /// <param name="_address">code address</param>
        public BitTestOperation(short _sourceAddress, short _bitNumber, BitTestOperator _op, Register.ProgramCounter _programCounter, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            programCounter = _programCounter;
            op = _op;
            bitValue = (registerFileMap.Get(_sourceAddress) & (1 << _bitNumber)) != 0;
        }


        public override void execute()
        {
            bool condition = false;
            switch (op)
            {
                case BitTestOperator.BTFSC:
                    if (!bitValue)
                    {
                        condition = true;
                    }
                    break;
                case BitTestOperator.BTFSS:
                    if (bitValue)
                    {
                        condition = true;
                    }
                    break;
                default:
                    break;
            }

            // If the result (value) is zero -> skip next operation.
            if (condition)
            {
                programCounter.value = (byte)(programCounter.value + 2);
                // pc won't be incremented after modifying it here.
                this.cycles = 2;
            }

        }
    }

    public enum BitTestOperator
    {
        BTFSC,
        BTFSS
    }
}
