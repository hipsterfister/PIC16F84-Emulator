using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Register;
using PIC16F84_Emulator.PIC.Operations;

namespace PIC16F84_Emulator.PIC.Parser
{
    public class Parser
    {
        /*
         *  This Class parses a 14-bit PIC-command, splitted in two bytes:
         *      > xx11 1111   2222 2222
         *        OPERATION   PARAMETER
         *  Create a new instance and call getNextOperation(codeAddress)
         *  The correct BaseOperation gets returned
         */
        
        private Register.RegisterFileMap registerFileMap;
        private Data.ProgamMemory programMemory;
        private Data.OperationStack operationStack;
        private Register.ProgramCounter programCounter;
        private PIC pic;

        public BaseOperation getNextOperation(short _codeAdress)
        {
            short target = 0;
            short source = 0;
            short address = _codeAdress;
            short bit = 0;
            byte byte1, byte2;
            ArithmeticOperator ArithOp = 0;
            BitOperator BitOp = 0;
            LogicOperator LogOp = 0;
            TestOperator TestOp = 0;
            BitTestOperator BitTestOp = 0;
            ReturnOperator RetOp = 0;
            RotationDirection RotDir = 0;

            // mask Operation-Byte --> xxxx xxxx 0000 0000
            short operation = (short)((short)programMemory[_codeAdress] & 0xFF00);
            // mask Parameter-Byte --> 0000 0000 xxxx xxxx
            short parameter = (short)((short)programMemory[_codeAdress] & 0x00FF);

            if (parameter < 0)
                throw new Exception("negative parameter");

            switch (operation)
            {
                /* ------------------------------------------------------ */
                /* -------- ARITHMETIC OPERATIONS ----------------------- */
                case ParserConstants.ADDWF:
                    // arithmetical operator
                    ArithOp = ArithmeticOperator.PLUS;
                    // target address
                    target = getTargetAddress(parameter);
                    // operating bytes
                    byte1 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    byte2 = registerFileMap.Get(getAddressFromParameter(parameter));
                    return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                case ParserConstants.ADDLW_1:
                case ParserConstants.ADDLW_2:
                    ArithOp = ArithmeticOperator.PLUS;
                    target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                    byte1 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    byte2 = getLiteralFromParameter(parameter);
                    return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                case ParserConstants.INCF:
                    ArithOp = ArithmeticOperator.PLUS;
                    target = getTargetAddress(parameter);
                    byte1 = 0x01;
                    byte2 = registerFileMap.Get(getAddressFromParameter(parameter));
                    return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                case ParserConstants.SUBWF:
                    ArithOp = ArithmeticOperator.MINUS;
                    target = getTargetAddress(parameter);
                    byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                    byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                case ParserConstants.SUBLW_1:
                case ParserConstants.SUBLW_2:
                    ArithOp = ArithmeticOperator.MINUS;
                    target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                    byte1 = getLiteralFromParameter(parameter);
                    byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                case ParserConstants.DECF:
                    ArithOp = ArithmeticOperator.MINUS;
                    target = getTargetAddress(parameter);
                    byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                    byte2 = 0x01;
                    return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- BIT OPERATIONS ------------------------------ */
                case ParserConstants.BCF_1:
                case ParserConstants.BCF_2:
                case ParserConstants.BCF_3:
                case ParserConstants.BCF_4:
                    // bit operator
                    BitOp = BitOperator.BITCLEAR;
                    // target address
                    target = getAddressFromParameter(parameter);
                    // bit-number
                    bit = getBitNumberFromOperationCall(operation, parameter);
                    return new BitOperation(target, bit, BitOp, registerFileMap, address);
                case ParserConstants.BSF_1:
                case ParserConstants.BSF_2:
                case ParserConstants.BSF_3:
                case ParserConstants.BSF_4:
                    BitOp = BitOperator.BITSET;
                    target = getAddressFromParameter(parameter);
                    bit = getBitNumberFromOperationCall(operation, parameter);
                    return new BitOperation(target, bit, BitOp, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- CALL OPERATIONS ----------------------------- */
                case ParserConstants.CALL_1:
                case ParserConstants.CALL_2:
                case ParserConstants.CALL_3:
                case ParserConstants.CALL_4:
                case ParserConstants.CALL_5:
                case ParserConstants.CALL_6:
                case ParserConstants.CALL_7:
                case ParserConstants.CALL_8:
                    target = getTargetAddress(operation, parameter);
                    return new CallOperation(target, operationStack, programCounter, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- GOTO OPERATION ------------------------------ */
                case ParserConstants.GOTO_1:
                case ParserConstants.GOTO_2:
                case ParserConstants.GOTO_3:
                case ParserConstants.GOTO_4:
                case ParserConstants.GOTO_5:
                case ParserConstants.GOTO_6:
                case ParserConstants.GOTO_7:
                case ParserConstants.GOTO_8:
                    target = getTargetAddress(operation, parameter);
                    return new GotoOperation(target, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- CLEAR OPERATIONS ---------------------------- */
                case ParserConstants.CLRF_CLRW:
                    target = getTargetAddress(parameter);
                    return new ClearOperation(target, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- COMPLEMENT OPERATIONS ----------------------- */
                case ParserConstants.COMF:
                    target = getTargetAddress(parameter);
                    return new ComplementOperation(target, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- LOGIC OPERATIONS ---------------------------- */
                case ParserConstants.ANDWF:
                    LogOp = LogicOperator.AND;
                    target = getTargetAddress(parameter);
                    byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                    byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                case ParserConstants.ANDLW:
                    LogOp = LogicOperator.AND;
                    target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                    byte1 = getLiteralFromParameter(parameter);
                    byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                case ParserConstants.IORWF:
                    LogOp = LogicOperator.IOR;
                    target = getTargetAddress(parameter);
                    byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                    byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                case ParserConstants.IORLW:
                    LogOp = LogicOperator.IOR;
                    target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                    byte1 = getLiteralFromParameter(parameter);
                    byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                case ParserConstants.XORWF:
                    LogOp = LogicOperator.XOR;
                    target = getTargetAddress(parameter);
                    byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                    byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                case ParserConstants.XORLW:
                    LogOp = LogicOperator.XOR;
                    target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                    byte1 = getLiteralFromParameter(parameter);
                    byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                    return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- MOVE OPERATIONS ----------------------------- */
                case ParserConstants.MOVF:
                    target = getTargetAddress(parameter);
                    source = getAddressFromParameter(parameter);
                    return new MoveOperation(source, target, registerFileMap, address);
                case ParserConstants.MOVLW_1:
                case ParserConstants.MOVLW_2:
                case ParserConstants.MOVLW_3:
                case ParserConstants.MOVLW_4:
                    byte1 = getLiteralFromParameter(parameter);
                    target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                    return new MoveOperation(byte1, target, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- ROTATE OPERATIONS --------------------------- */
                case ParserConstants.RLF:
                    RotDir = RotationDirection.LEFT;
                    target = getTargetAddress(parameter);
                    source = getAddressFromParameter(parameter);
                    return new RotateOperation(source, target, RotDir, registerFileMap, address);
                case ParserConstants.RRF:
                    RotDir = RotationDirection.RIGHT;
                    target = getTargetAddress(parameter);
                    source = getAddressFromParameter(parameter);
                    return new RotateOperation(source, target, RotDir, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- SWAP OPERATIONS ----------------------------- */
                case ParserConstants.SWAPF:
                    target = getTargetAddress(parameter);
                    source = getAddressFromParameter(parameter);
                    return new SwapOperation(source, target, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- BIT TEST OPERATIONS ----------------------------- */
                case ParserConstants.DECFSZ:
                    TestOp = TestOperator.DECFSZ;
                    target = getTargetAddress(parameter);
                    source = getAddressFromParameter(parameter);
                    return new TestOperation(source, TestOp, target, registerFileMap, address);
                case ParserConstants.INCFSZ:
                    TestOp = TestOperator.INCFSZ;
                    target = getTargetAddress(parameter);
                    source = getAddressFromParameter(parameter);
                    return new TestOperation(source, TestOp, target, registerFileMap, address);
                case ParserConstants.BTFSC_1:
                case ParserConstants.BTFSC_2:
                case ParserConstants.BTFSC_3:
                case ParserConstants.BTFSC_4:
                    BitTestOp = BitTestOperator.BTFSC;
                    source = getAddressFromParameter(parameter);
                    bit = getBitNumberFromOperationCall(operation, parameter);
                    return new BitTestOperation(source, bit, BitTestOp, registerFileMap, address);
                case ParserConstants.BTFSS_1:
                case ParserConstants.BTFSS_2:
                case ParserConstants.BTFSS_3:
                case ParserConstants.BTFSS_4:
                    BitTestOp = BitTestOperator.BTFSS;
                    source = getAddressFromParameter(parameter);
                    bit = getBitNumberFromOperationCall(operation, parameter);
                    return new BitTestOperation(source, bit, BitTestOp, registerFileMap, address);
                /* ------------------------------------------------------ */

                /* ------------------------------------------------------ */
                /* -------- RETURN OPERATIONS --------------------------- */
                case ParserConstants.RETLW_1:
                case ParserConstants.RETLW_2:
                case ParserConstants.RETLW_3:
                case ParserConstants.RETLW_4:
                    RetOp = ReturnOperator.RETLW;
                    byte1 = getLiteralFromParameter(parameter);
                    return new ReturnOperation(operationStack, RetOp, byte1, registerFileMap, address);
                /* ------------------------------------------------------ */

                case 0x0000:
                    if(parameter > 127)
                    {
                        // MOVWF
                        target = getAddressFromParameter(parameter);
                        byte1 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new MoveOperation(byte1, target, registerFileMap, address);
                    }
                    switch (parameter)
                    {
                        case ParserConstants.CLRWDT:
                            target = RegisterConstants.WDT_REGISTER_ADDRESS;
                            return new ClearOperation(target, registerFileMap, address);
                        case ParserConstants.RETFIE:
                            RetOp = ReturnOperator.RETFIE;
                            return new ReturnOperation(operationStack, RetOp, registerFileMap, address);
                        case ParserConstants.RETURN:
                            RetOp = ReturnOperator.RETURN;
                            return new ReturnOperation(operationStack, RetOp, registerFileMap, address);
                        case ParserConstants.SLEEP:
                            return new SleepOperation(pic, registerFileMap, address);
                        case ParserConstants.NOP_1:
                        case ParserConstants.NOP_2:
                        case ParserConstants.NOP_3:
                        case ParserConstants.NOP_4:
                            return new NopOperation(registerFileMap, address);
                    }                      
                    break;

                default:
                    throw new Exception("unknown operation");
            }

            throw new Exception("unknown error");
        }

        private short getTargetAddress(short _parameter)
        {
            // CHECK IF D VALUE = 1 (p = DFFF FFFF => IF D = 1 => p > 127)
            // IF D = 1 => target = parameter | IF D = 0 => target = W-REG
            short target = _parameter > 127 ? getAddressFromParameter(_parameter) : RegisterConstants.WORKING_REGISTER_ADDRESS;
            return target;
        }

        private short getTargetAddress(short _operation, short _parameter)
        {
            // xx10 0kkk kkkk kkkk => 0000 0kkk kkkk kkkk
            short target = (short)((_operation & 0x0700) + _parameter);
            if (target > 2047)
                throw new Exception("too large address");
            return target;
        }

        private short getAddressFromParameter(short _parameter)
        {
            // xxxx xxxx DFFF FFFF & 0000 0000 0111 1111 => ADDRESS (0000 0000 0xxx xxxx)
            return (short)(_parameter & 0x007F);
        }

        private byte getLiteralFromParameter(short _parameter)
        {
            if (_parameter > 255)
                throw new Exception("too large value");
            return (byte)_parameter;
        }

        private short getBitNumberFromOperationCall(short _operation, short _parameter)
        {
            // xxxx xxBB Bxxx xxxx => 0000 0000 0000 0BBB
            return (short)(((_operation + _parameter) & 0x0380) >> 7);
        }

        public Parser(PIC _pic)
        {
            this.pic = _pic;
            this.registerFileMap = pic.getRegisterFileMap();
            this.programMemory = pic.getProgramMemory();
            this.operationStack = pic.getOperationStack();
            this.programCounter = pic.getProgramCounter();
        }
   
    }
}
