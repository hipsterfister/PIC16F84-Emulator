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
        public const short STACK_SIZE = 8;
        private short currentTopIndex;

        internal delegate void onStackChange();
        internal event onStackChange stackChangeEvent;

        public OperationStack()
        {
            this.stack = new DataAdapter<short>[STACK_SIZE];
            for (int i = 0; i< STACK_SIZE; i++)
            {
                this.stack[i] = new DataAdapter<short>();
            }

            initializeValues();
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
            this.stack[currentTopIndex].Value = 0;
            if (currentTopIndex == 0)
            {
                currentTopIndex = STACK_SIZE - 1;
            }
            else
            {
                currentTopIndex--;
            }

            if (stackChangeEvent != null)
            {
                stackChangeEvent();
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

            if (stackChangeEvent != null)
            {
                stackChangeEvent();
            }
        }

        internal short[] getStackAsSortedArray()
        {
            short[] sortedArray = new short[STACK_SIZE];

            int index = currentTopIndex;

            for (int i = 0; i < STACK_SIZE; i++)
            {
                sortedArray[i] = stack[index].Value;
                index--;
                if (index < 0)
                    index = STACK_SIZE-1;
            }

            return sortedArray;
        }

        internal void registerStackUpdateListener(onStackChange listener)
        {
            stackChangeEvent += listener;
        }

        internal void unregisterStackUpdateListener(onStackChange listener)
        {
            stackChangeEvent -= listener;
        }

    }
}
