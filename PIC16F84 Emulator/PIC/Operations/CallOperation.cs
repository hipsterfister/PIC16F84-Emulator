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

        private BaseOperation targetOperation;
        private BaseOperation followingOperation;
        private OperationStack operationStack;

        public CallOperation(BaseOperation _targetOperation, BaseOperation _followingOperation, OperationStack _operationStack, RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.targetOperation = _targetOperation;
            this.followingOperation = _followingOperation;
            this.operationStack = _operationStack;
        }

        public void execute()
        {
            this.operationStack.push(this.followingOperation);
            // TODO: PC
        }
    }
}
