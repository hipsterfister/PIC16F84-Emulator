using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    class BaseOperation
    {
        protected RegisterFileMap registerFileMap;
        delegate void execute();

        public BaseOperation(RegisterFileMap _registerFileMap)
        {
            this.registerFileMap = _registerFileMap;
        }
    }
}
