using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC
{
    public class PIC
    {
        protected Register.RegisterFileMap registerMap = new Register.RegisterFileMap();
        protected Data.OperationStack operationStack = new Data.OperationStack();

    }
}
