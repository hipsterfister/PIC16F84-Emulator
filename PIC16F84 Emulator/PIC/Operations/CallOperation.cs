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
        private short followingOperationAddress;
        private OperationStack operationStack;

        public CallOperation(short _targetOperationAddress, short _followingOperationAddress, OperationStack _operationStack, RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.targetOperationAddress = _targetOperationAddress;
            this.followingOperationAddress = _followingOperationAddress;
            this.operationStack = _operationStack;
        }

        public override void execute()
        {
            this.operationStack.push(this.followingOperationAddress);
            registerFileMap.setProgramCounter(this.targetOperationAddress);
        }
    }
}
