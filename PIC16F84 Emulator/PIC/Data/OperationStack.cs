using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;
using PIC16F84_Emulator.PIC.Operations;

namespace PIC16F84_Emulator.PIC.Data
{
    public class OperationStack
    {
        private DataAdapter<short>[] stack;
        private const short STACK_SIZE = 8;
        private short currentTopIndex = 0;

        public OperationStack()
        {
            this.stack = new DataAdapter<short>[STACK_SIZE];
            for (int i = 0; i< STACK_SIZE; i++)
            {
                this.stack[i] = new DataAdapter<short>();
            }
        }

        public void initializeValues()
        {
            currentTopIndex = 0;
            for (int i = 0; i < STACK_SIZE; i++)
            {
                this.stack[i].Value = 0;
            }
        }

        public short pop()
        {
            short result = this.stack[currentTopIndex].Value;
            if (currentTopIndex == 0)
            {
                currentTopIndex = STACK_SIZE - 1;
            }
            else
            {
                currentTopIndex--;
            }
            return result;
        }

        public void push(short _operation)
        {
            if (currentTopIndex == STACK_SIZE - 1)
            {
                currentTopIndex = 0;
            }
            else
            {
                currentTopIndex++;
            }
            this.stack[currentTopIndex].Value = _operation;
        }


    }
}
