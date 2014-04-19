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
        private const int ADDWF         = 0x0700;
        private const int ADDWL_1       = 0x3E00;
        private const int ADDWL_2       = 0x3F00;
        private const int INCF          = 0x0A00;
        private const int SUBWF         = 0x0200;
        private const int SUBLW_1       = 0x3C00;
        private const int SUBLW_2       = 0x3D00;
        private const int DECF          = 0x0300;
        private const int BCF_1         = 0x1000;
        private const int BCF_2         = 0x1100;
        private const int BCF_3         = 0x1200;
        private const int BCF_4         = 0x1300;
        private const int BSF_1         = 0x1400;
        private const int BSF_2         = 0x1500;
        private const int BSF_3         = 0x1600;
        private const int BSF_4         = 0x1700;
        private const int CLRF_CLRW     = 0x0100;
        private const int COMF          = 0x0900;
        private const int ANDWF         = 0x0500;
        private const int ANDWL         = 0x3900;
        private const int IORWF         = 0x0400;
        private const int IORLW         = 0x3800;
        private const int XORWF         = 0x0600;
        private const int XORLW         = 0x3A00;
        private const int MOVF          = 0x0800;
        private const int MOVLW_1       = 0x3000;
        private const int MOVLW_2       = 0x3100;
        private const int MOVLW_3       = 0x3200;
        private const int MOVLW_4       = 0x3300;
        private const int RLF           = 0x0D00;
        private const int RRF           = 0x0C00;
        private const int SWAPF         = 0x0E00;
        private const int CLRWDT        = 0x0064;
        private const int RETFIE        = 0x0009;
        private const int RETLW_1       = 0x3400;
        private const int RETLW_2       = 0x3500;
        private const int RETLW_3       = 0x3600;
        private const int RETLW_4       = 0x3700;
        private const int RETURN        = 0x0008;
        private const int SLEEP         = 0x0063;
        private const int NOP_1         = 0x0000;
        private const int NOP_2         = 0x0020;
        private const int NOP_3         = 0x0040;
        private const int NOP_4         = 0x0060;
        private const int BTFSC_1       = 0x1800;
        private const int BTFSC_2       = 0x1900;
        private const int BTFSC_3       = 0x1A00;
        private const int BTFSC_4       = 0x1B00;
        private const int BTFSS_1       = 0x1C00;
        private const int BTFSS_2       = 0x1D00;
        private const int BTFSS_3       = 0x1E00;
        private const int BTFSS_4       = 0x1F00;
        private const int DECFSZ        = 0x0B00;
        private const int INCFSZ        = 0x0F00;

        public BaseOperation getNextOperation(short _codeAdress)
        {
            try
            {
                // TODO: obligatorischen Programmspeicher durch den richtigen ersetzen
                Data.DataAdapter<short>[] Pr0gramStorage;
                Pr0gramStorage = new Data.DataAdapter<short>[1];
                Pr0gramStorage[0] = new Data.DataAdapter<short>();
                
                short target = 0;
                short source = 0;
                short address = _codeAdress;
                short bit = 0;
                byte byte1, byte2;
                ArithmeticOperator ArithOp = 0;
                BitOperator BitOp = 0;
                LogicOperator LogOp = 0;
                TestOperator TestOp = 0;
                ReturnOperator RetOp = 0;
                RotationDirection RotDir = 0;

                // mask Operation-Byte --> xxxx xxxx 0000 0000
                short operation = (short)((short)Pr0gramStorage.GetValue(_codeAdress) & 0xFF00);
                // mask Parameter-Byte --> 0000 0000 xxxx xxxx
                short parameter = (short)((short)Pr0gramStorage.GetValue(_codeAdress) & 0x00FF);

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
                        target = checkDValue(parameter);
                        // operating bytes
                        byte1 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        byte2 = registerFileMap.Get(getAddressFromParameter(parameter));
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case ADDWL_1:
                    case ADDWL_2:
                        ArithOp = ArithmeticOperator.PLUS;
                        target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                        byte1 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        byte2 = getLiteralFromParameter(parameter);
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case INCF:
                        ArithOp = ArithmeticOperator.PLUS;
                        target = checkDValue(parameter);
                        byte1 = 0x01;
                        byte2 = registerFileMap.Get(getAddressFromParameter(parameter));
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case SUBWF:
                        ArithOp = ArithmeticOperator.MINUS;
                        target = checkDValue(parameter);
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
                        target = checkDValue(parameter);
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
                        bit = (short)(operation + parameter & 0x0380); // xxxx xxBB Bxxx xxxx => 0000 00xx x000 0000
                        return new BitOperation(target, bit, BitOp, registerFileMap, address);
                    case BSF_1:
                    case BSF_2:
                    case BSF_3:
                    case BSF_4:
                        BitOp = BitOperator.BITSET;
                        target = getAddressFromParameter(parameter);
                        bit = (short)(operation + parameter & 0x0380);
                        return new BitOperation(target, bit, BitOp, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- CALL OPERATIONS ----------------------------- */
                        // TODO: CALL OPERATIONS
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- CLEAR OPERATIONS ---------------------------- */
                    case CLRF_CLRW:
                        target = checkDValue(parameter);
                        return new ClearOperation(target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- COMPLEMENT OPERATIONS ----------------------- */
                    case COMF:
                        target = checkDValue(parameter);
                        return new ComplementOperation(target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- LOGIC OPERATIONS ---------------------------- */
                    case ANDWF:
                        LogOp = LogicOperator.AND;
                        target = checkDValue(parameter);
                        byte1 = registerFileMap.Get(getAddressFromParameter(parameter));
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case ANDWL:
                        LogOp = LogicOperator.AND;
                        target = RegisterConstants.WORKING_REGISTER_ADDRESS;
                        byte1 = getLiteralFromParameter(parameter);
                        byte2 = registerFileMap.Get(RegisterConstants.WORKING_REGISTER_ADDRESS);
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case IORWF:
                        LogOp = LogicOperator.IOR;
                        target = checkDValue(parameter);
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
                        target = checkDValue(parameter);
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
                        target = checkDValue(parameter);
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
                        target = checkDValue(parameter);
                        source = getAddressFromParameter(parameter);
                        return new RotateOperation(source, target, RotDir, registerFileMap, address);
                    case RRF:
                        RotDir = RotationDirection.RIGHT;
                        target = checkDValue(parameter);
                        source = getAddressFromParameter(parameter);
                        return new RotateOperation(source, target, RotDir, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- SWAP OPERATIONS ----------------------------- */
                    case SWAPF:
                        target = checkDValue(parameter);
                        source = getAddressFromParameter(parameter);
                        return new SwapOperation(source, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- BIT TEST OPERATIONS ----------------------------- */
                    case DECFSZ:
                        TestOp = TestOperator.DECFSZ;
                        target = checkDValue(parameter);
                        source = getAddressFromParameter(parameter);
                        return new TestOperation(source, TestOp, registerFileMap, address);
                    case INCFSZ:
                        TestOp = TestOperator.INCFSZ;
                        target = checkDValue(parameter);
                        source = getAddressFromParameter(parameter);
                        return new TestOperation(source, TestOp, registerFileMap, address);
                    case BTFSC_1:
                    case BTFSC_2:
                    case BTFSC_3:
                    case BTFSC_4:
                        // TODO: BTFSC OPERATION
                    case BTFSS_1:
                    case BTFSS_2:
                    case BTFSS_3:
                    case BTFSS_4:
                        // TODO: BTFSS OPERATION
                        break;
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

        private short checkDValue(short _parameter)
        {
            // CHECK IF D VALUE = 1 (p = DFFF FFFF => IF D = 1 => p > 127)
            // IF D = 1 => target = parameter | IF D = 0 => target = W-REG
            short target = _parameter > 127 ? getAddressFromParameter(_parameter) : RegisterConstants.WORKING_REGISTER_ADDRESS;
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
                throw new Exception("too long, too large, too big");
            return (byte)_parameter;
        }

        public Parser(Register.RegisterFileMap _registerFileMap)
        {
            this.registerFileMap = _registerFileMap;
        }
   
    }
}
