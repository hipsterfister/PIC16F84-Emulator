using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class NopOperation : BaseOperation
    {
        private const short CYCLES = 1;

        public NopOperation(Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {

        }

        public override void execute()
        {
            // do nothing
        }
    }
}
