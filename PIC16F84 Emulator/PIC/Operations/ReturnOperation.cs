using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class ReturnOperation : BaseOperation
    {
        private const short CYCLES = 2;
        private ReturnOperator op;
        private byte arg;

        /// <summary>
        /// Creates a new instance of ReturnOperation without an argument. Use this for RETURN, RETFIE.
        /// </summary>
        /// <param name="_op">enum RETURN / RETFIE</param>
        /// <param name="_registerFileMap"></param>
        /// <param name="_address"></param>
        public ReturnOperation(ReturnOperator _op, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            op = _op;
            arg = 0;
            if (_op == ReturnOperator.RETLW)
            {
                new Exception("Cannot create ReturnOperation with Operator RETLW without passing an argument. (Hint: use different constructor).");
            }
        }

        /// <summary>
        /// Creates a new instance of ReturnOperation with an argument. Use this for RETFIE.
        /// </summary>
        /// <param name="_op"></param>
        /// <param name="_arg"></param>
        /// <param name="_registerFileMap"></param>
        /// <param name="_address"></param>
        public ReturnOperation(ReturnOperator _op, byte _arg, Register.RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            op = _op;
            arg = _arg;
        }

        public override void execute()
        {
            registerFileMap.setProgramCounter(registerFileMap.popStack());

            if(op == ReturnOperator.RETFIE) {
                    // 1 -> GIE
                    registerFileMap.setBit(Register.RegisterConstants.INTCON_ADDRESS, Register.RegisterConstants.INTCON_GIE_MASK);
            }

            if (op == ReturnOperator.RETLW) {
                    // arg -> W-Register
                    registerFileMap.Set(arg, Register.RegisterConstants.WORKING_REGISTER_ADDRESS);
            }
        }
    }

    public enum ReturnOperator
    {
        RETURN,
        RETFIE,
        RETLW
    }
}
