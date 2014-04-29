using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class TestOperation : BaseOperation
    {
        private const short CYCLES = 1;
        private TestOperator op;
        private short targetAddress;
        private short sourceAddress;
        private Register.ProgramCounter programCounter;

        /// <summary>
        /// Creates a new TestOperation. Use only if d == 1 (INCFSZ f,d / DECFSZ f,d)
        /// </summary>
        /// <param name="_sourceAddress">f</param>
        /// <param name="_op">INC / DEC</param>
        /// <param name="_registerFileMap"></param>
        /// <param name="_address">code address</param>
        public TestOperation(short _sourceAddress, TestOperator _op, Register.ProgramCounter _programCounter, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            op = _op;
            targetAddress = _sourceAddress;
            sourceAddress = _sourceAddress;
            programCounter = _programCounter;
        }

        /// <summary>
        /// Creates a new TestOperation. (INCFSZ f,d / DECFSZ f,d)
        /// </summary>
        /// <param name="_sourceAddress">f</param>
        /// <param name="_op">INC / DEC</param>
        /// <param name="_targetAddress">the address where the result should be stored in</param>
        /// <param name="_registerFileMap"></param>
        /// <param name="_address">code address</param>
        public TestOperation(short _sourceAddress, TestOperator _op, short _targetAddress, Register.ProgramCounter _programCounter, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            op = _op;
            sourceAddress = _sourceAddress;
            targetAddress = _targetAddress;
            programCounter = _programCounter;
        }

        public override void execute()
        {
            short value = registerFileMap.Get(sourceAddress);
            switch (op)
            {
                case TestOperator.DECFSZ:
                    value--;
                    break;
                case TestOperator.INCFSZ:
                    value++;
                    break;
            }

            // If the result (value) is zero -> skip next operation.
            if (value == 0 || value > 0xFF)
            {
                programCounter.increment();
                this.cycles = 2;
            }

            registerFileMap.Set((byte) value, targetAddress);
        }
    }

    public enum TestOperator
    {
        DECFSZ,
        INCFSZ
    }
}
