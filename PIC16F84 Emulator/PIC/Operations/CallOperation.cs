using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Operations;
using PIC16F84_Emulator.PIC.Register;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Operations
{
    class CallOperation : BaseOperation
    {
        private const short CYCLES = 2;

        private short targetOperationAddress;
        private ProgramCounter programCounter;
        private OperationStack operationStack;

        public CallOperation(short _targetOperationAddress, OperationStack _operationStack, ProgramCounter _programCounter, RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.targetOperationAddress = _targetOperationAddress;
            this.programCounter = _programCounter;
            this.operationStack = _operationStack;
        }

        public override void execute()
        {
            this.operationStack.push(this.programCounter.value);
            short highValue = (short) ((registerFileMap.Get(Register.RegisterConstants.PCLATH_ADDRESS) & 0x24) * 0x100);
            this.programCounter.value = (short)(highValue + this.targetOperationAddress);
        }
    }
}
