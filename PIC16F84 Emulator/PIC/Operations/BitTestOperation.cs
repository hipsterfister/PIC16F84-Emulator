using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class TestOperation : BaseOperation
    {
        private const short CYCLES = 2;
        private TestOperator op;
        private short targetAddress;
        private short sourceAddress;

        /// <summary>
        /// Creates a new TestOperation. Use only if d == 1 (INCFSZ f,d / DECFSZ f,d)
        /// </summary>
        /// <param name="_sourceAddress">f</param>
        /// <param name="_op">INC / DEC</param>
        /// <param name="_registerFileMap"></param>
        /// <param name="_address">code address</param>
        public TestOperation(short _sourceAddress, TestOperator _op, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            op = _op;
            targetAddress = _sourceAddress;
            sourceAddress = _sourceAddress;
        }

        /// <summary>
        /// Creates a new TestOperation. (INCFSZ f,d / DECFSZ f,d)
        /// </summary>
        /// <param name="_sourceAddress">f</param>
        /// <param name="_op">INC / DEC</param>
        /// <param name="_dValue">d</param>
        /// <param name="_registerFileMap"></param>
        /// <param name="_address">code address</param>
        public TestOperation(short _sourceAddress, TestOperator _op, bool _dValue, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            op = _op;
            sourceAddress = _sourceAddress;
            if (_dValue)
            {
                targetAddress = _sourceAddress;
            }
            else
            {
                targetAddress = Register.RegisterConstants.WORKING_REGISTER_ADDRESS;
            }

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
            if (value == 0)
            {
                registerFileMap.incrementProgramCounter();
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
