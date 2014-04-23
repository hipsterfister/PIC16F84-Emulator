using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Register;
using PIC16F84_Emulator.PIC.Operations;

namespace PIC16F84_Emulator.PIC.Parser
{
    class Parser
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

        /* -------------------------------------- */
        /* BYTE-ORIENTED FILE REGISTER OPERATIONS */
        private const int ADDWF         = 0x0700;
        private const int ANDWF         = 0x0500;
        private const int CLRF_CLRW     = 0x0100;
        private const int COMF          = 0x0900;
        private const int DECF          = 0x0300;
        private const int DECFSZ        = 0x0B00;
        private const int INCF          = 0x0A00;
        private const int INCFSZ        = 0x0F00;
        private const int IORWF         = 0x0400;
        private const int MOVF          = 0x0800;
        private const int MOVWF         = 0x0000;
        private const int NOP_1         = 0x0000;
        private const int NOP_2         = 0x0020;
        private const int NOP_3         = 0x0040;
        private const int NOP_4         = 0x0060;
        private const int RLF           = 0x0D00;
        private const int RRF           = 0x0C00;
        private const int SUBWF         = 0x0200;
        private const int SWAPF         = 0x0E00;
        private const int XORWF         = 0x0600;
        /* -------------------------------------- */

        /* -------------------------------------- */
        /* BIT-ORIENTED FILE REGISTER OPERATIONS  */
        private const int BCF_1         = 0x1000;
        private const int BCF_2         = 0x1100;
        private const int BCF_3         = 0x1200;
        private const int BCF_4         = 0x1300;
        private const int BSF_1         = 0x1400;
        private const int BSF_2         = 0x1500;
        private const int BSF_3         = 0x1600;
        private const int BSF_4         = 0x1700;
        private const int BTFSC_1       = 0x1800;
        private const int BTFSC_2       = 0x1900;
        private const int BTFSC_3       = 0x1A00;
        private const int BTFSC_4       = 0x1B00;
        private const int BTFSS_1       = 0x1C00;
        private const int BTFSS_2       = 0x1D00;
        private const int BTFSS_3       = 0x1E00;
        private const int BTFSS_4       = 0x1F00;
        /* -------------------------------------- */

        /* -------------------------------------- */
        /* --- LITERAL AND CONTROL OPERATIONS --- */
        private const int ADDLW_1       = 0x3E00;
        private const int ADDLW_2       = 0x3F00;
        private const int ANDLW         = 0x3900;
        private const int CALL_1        = 0x2000;
        private const int CALL_2        = 0x2100;
        private const int CALL_3        = 0x2200;
        private const int CALL_4        = 0x2300;
        private const int CALL_5        = 0x2400;
        private const int CALL_6        = 0x2500;
        private const int CALL_7        = 0x2600;
        private const int CALL_8        = 0x2700;
        private const int CLRWDT        = 0x0064;
        private const int GOTO_1        = 0x2800;
        private const int GOTO_2        = 0x2900;
        private const int GOTO_3        = 0x2A00;
        private const int GOTO_4        = 0x2B00;
        private const int GOTO_5        = 0x2C00;
        private const int GOTO_6        = 0x2D00;
        private const int GOTO_7        = 0x2E00;
        private const int GOTO_8        = 0x2F00;
        private const int IORLW         = 0x3800;
        private const int MOVLW_1       = 0x3000;
        private const int MOVLW_2       = 0x3100;
        private const int MOVLW_3       = 0x3200;
        private const int MOVLW_4       = 0x3300;
        private const int RETFIE        = 0x0009;
        private const int RETLW_1       = 0x3400;
        private const int RETLW_2       = 0x3500;
        private const int RETLW_3       = 0x3600;
        private const int RETLW_4       = 0x3700;
        private const int RETURN        = 0x0008;
        private const int SLEEP         = 0x0063;
        private const int SUBLW_1       = 0x3C00;
        private const int SUBLW_2       = 0x3D00;
        private const int XORLW         = 0x3A00;
        /* -------------------------------------- */

        public BaseOperation getNextOperation(short _codeAdress)
        {
            try
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
                    case ADDWF:
                        // arithmetical operator
                        ArithOp = ArithmeticOperator.PLUS;
                        // target address
                        target = getTargetAddress(parameter);
                        // operating bytes
                        byte1 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        byte2 = registerFileMap.Get(getAddressFromParameter(parameter));
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case ADDLW_1:
                    case ADDLW_2:
                        ArithOp = ArithmeticOperator.PLUS;
                        target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                        byte1 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        byte2 = getLiteralFromParameter(parameter);
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case INCF:
                        ArithOp = ArithmeticOperator.PLUS;
                        target = getTargetAddress(parameter);
                        byte1 = 0x01;
                        byte2 = registerFileMap.Get(getAddressFromParameter(parameter));
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case SUBWF:
                        ArithOp = ArithmeticOperator.MINUS;
                        target = getTargetAddress(parameter);
                        byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case SUBLW_1:
                    case SUBLW_2:
                        ArithOp = ArithmeticOperator.MINUS;
                        target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                        byte1 = getLiteralFromParameter(parameter);
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case DECF:
                        ArithOp = ArithmeticOperator.MINUS;
                        target = getTargetAddress(parameter);
                        byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                        byte2 = 0x01;
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- BIT OPERATIONS ------------------------------ */
                    case BCF_1:
                    case BCF_2:
                    case BCF_3:
                    case BCF_4:
                        // bit operator
                        BitOp = BitOperator.BITCLEAR;
                        // target address
                        target = getAddressFromParameter(parameter);
                        // bit-number
                        bit = getBitNumberFromOperationCall(operation, parameter);
                        return new BitOperation(target, bit, BitOp, registerFileMap, address);
                    case BSF_1:
                    case BSF_2:
                    case BSF_3:
                    case BSF_4:
                        BitOp = BitOperator.BITSET;
                        target = getAddressFromParameter(parameter);
                        bit = getBitNumberFromOperationCall(operation, parameter);
                        return new BitOperation(target, bit, BitOp, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- CALL OPERATIONS ----------------------------- */
                    case CALL_1:
                    case CALL_2:
                    case CALL_3:
                    case CALL_4:
                    case CALL_5:
                    case CALL_6:
                    case CALL_7:
                    case CALL_8:
                        target = getTargetAddress(operation, parameter);
                        return new CallOperation(target, operationStack, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- GOTO OPERATION ------------------------------ */
                    case GOTO_1:
                    case GOTO_2:
                    case GOTO_3:
                    case GOTO_4:
                    case GOTO_5:
                    case GOTO_6:
                    case GOTO_7:
                    case GOTO_8:
                        target = getTargetAddress(operation, parameter);
                        return new GotoOperation(target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- CLEAR OPERATIONS ---------------------------- */
                    case CLRF_CLRW:
                        target = getTargetAddress(parameter);
                        return new ClearOperation(target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- COMPLEMENT OPERATIONS ----------------------- */
                    case COMF:
                        target = getTargetAddress(parameter);
                        return new ComplementOperation(target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- LOGIC OPERATIONS ---------------------------- */
                    case ANDWF:
                        LogOp = LogicOperator.AND;
                        target = getTargetAddress(parameter);
                        byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case ANDLW:
                        LogOp = LogicOperator.AND;
                        target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                        byte1 = getLiteralFromParameter(parameter);
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case IORWF:
                        LogOp = LogicOperator.IOR;
                        target = getTargetAddress(parameter);
                        byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case IORLW:
                        LogOp = LogicOperator.IOR;
                        target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                        byte1 = getLiteralFromParameter(parameter);
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case XORWF:
                        LogOp = LogicOperator.XOR;
                        target = getTargetAddress(parameter);
                        byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case XORLW:
                        LogOp = LogicOperator.XOR;
                        target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                        byte1 = getLiteralFromParameter(parameter);
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- MOVE OPERATIONS ----------------------------- */
                    case MOVF:
                        target = getTargetAddress(parameter);
                        source = getAddressFromParameter(parameter);
                        return new MoveOperation(source, target, registerFileMap, address);
                    case MOVLW_1:
                    case MOVLW_2:
                    case MOVLW_3:
                    case MOVLW_4:
                        byte1 = getLiteralFromParameter(parameter);
                        return new MoveOperation(byte1, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- ROTATE OPERATIONS --------------------------- */
                    case RLF:
                        RotDir = RotationDirection.LEFT;
                        target = getTargetAddress(parameter);
                        source = getAddressFromParameter(parameter);
                        return new RotateOperation(source, target, RotDir, registerFileMap, address);
                    case RRF:
                        RotDir = RotationDirection.RIGHT;
                        target = getTargetAddress(parameter);
                        source = getAddressFromParameter(parameter);
                        return new RotateOperation(source, target, RotDir, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- SWAP OPERATIONS ----------------------------- */
                    case SWAPF:
                        target = getTargetAddress(parameter);
                        source = getAddressFromParameter(parameter);
                        return new SwapOperation(source, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- BIT TEST OPERATIONS ----------------------------- */
                    case DECFSZ:
                        TestOp = TestOperator.DECFSZ;
                        target = getTargetAddress(parameter);
                        source = getAddressFromParameter(parameter);
                        return new TestOperation(source, TestOp, target, registerFileMap, address);
                    case INCFSZ:
                        TestOp = TestOperator.INCFSZ;
                        target = getTargetAddress(parameter);
                        source = getAddressFromParameter(parameter);
                        return new TestOperation(source, TestOp, target, registerFileMap, address);
                    case BTFSC_1:
                    case BTFSC_2:
                    case BTFSC_3:
                    case BTFSC_4:
                        BitTestOp = BitTestOperator.BTFSC;
                        source = getAddressFromParameter(parameter);
                        bit = getBitNumberFromOperationCall(operation, parameter);
                        return new BitTestOperation(source, bit, BitTestOp, registerFileMap, address);
                    case BTFSS_1:
                    case BTFSS_2:
                    case BTFSS_3:
                    case BTFSS_4:
                        BitTestOp = BitTestOperator.BTFSS;
                        source = getAddressFromParameter(parameter);
                        bit = getBitNumberFromOperationCall(operation, parameter);
                        return new BitTestOperation(source, bit, BitTestOp, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- RETURN OPERATIONS --------------------------- */
                    case RETLW_1:
                    case RETLW_2:
                    case RETLW_3:
                    case RETLW_4:
                        RetOp = ReturnOperator.RETLW;
                        byte1 = getLiteralFromParameter(parameter);
                        return new ReturnOperation(RetOp, byte1, registerFileMap, address);
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
                            case CLRWDT:
                                target = RegisterConstants.WDT_REGISTER_ADDRESS;
                                return new ClearOperation(target, registerFileMap, address);
                            case RETFIE:
                                RetOp = ReturnOperator.RETFIE;
                                return new ReturnOperation(RetOp, registerFileMap, address);
                            case RETURN:
                                RetOp = ReturnOperator.RETURN;
                                return new ReturnOperation(RetOp, registerFileMap, address);
                            case SLEEP:
                                break;
                            case NOP_1:
                            case NOP_2:
                            case NOP_3:
                            case NOP_4:
                                return new NopOperation(registerFileMap, address);
                        }                      
                        break;

                    default:
                        throw new Exception("unknown operation");
                }

                throw new Exception("unknown error");

            }
            catch (Exception ex)
            {
                return null;
            }
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

        public Parser(Register.RegisterFileMap _registerFileMap, Data.ProgamMemory _programMemory, Data.OperationStack _operationStack, Register.ProgramCounter _programCounter)
        {
            this.registerFileMap = _registerFileMap;
            this.programMemory = _programMemory;
            this.operationStack = _operationStack;
            this.programCounter = _programCounter;
        }
   
    }
}
