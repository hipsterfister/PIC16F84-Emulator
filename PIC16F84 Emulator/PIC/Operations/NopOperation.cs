using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class NopOperation : BaseOperation
    {
        public NopOperation(Register.RegisterFileMap _registerFileMap):
            base(_registerFileMap)
        {

        }

        public void execute()
        {

        }
    }
}
