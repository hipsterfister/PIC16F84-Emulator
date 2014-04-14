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

        public GotoOperation(short _targetAddress, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            targetAddress = _targetAddress;
        }

        public override void execute()
        {
            registerFileMap.setProgramCounter(targetAddress);
        }
    }
}
