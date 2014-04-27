using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class GotoOperation : BaseOperation
    {
        private const short CYCLES = 2;
        private short targetAddress;
        private Register.ProgramCounter programCounter;

        public GotoOperation(short _targetAddress, Register.ProgramCounter _programCounter, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.programCounter = _programCounter;
            this.targetAddress = _targetAddress;
        }

        public override void execute()
        {
            programCounter.value = targetAddress;
        }
    }
}
