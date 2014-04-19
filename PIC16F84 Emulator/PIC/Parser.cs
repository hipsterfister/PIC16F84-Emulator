using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Operations;

namespace PIC16F84_Emulator.PIC.Parser
{
    class Parser
    {
        /*
         *  This Class parses a 14-bit PIC-command, splitted in two bytes:
         *      > xx11 1111   2222 2222
         *        OPERATION   PARAMETER
         *  Create a new instance and call nextCmd(OPERATION,PARAMETERS)
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
        private const int RETURN        = 0x0008;
        private const int SLEEP         = 0x0063;
        private const int NOP_1         = 0x0000;
        private const int NOP_2         = 0x0020;
        private const int NOP_3         = 0x0040;
        private const int NOP_4         = 0x0060;


        public BaseOperation getNextOperation(short _codeAdress)
        {
            try
            {
                // TODO: obligatorischen Programmspeicher durch den richtigen ersetzen
                Data.DataAdapter<short>[] Pr0gramStorage;
                // TODO: obligatorisches W-Register durch das richtige ersetzen
                byte wreg = 0x00;
                
                short target = 0;
                short source = 0;
                short address = _codeAdress;
                short bit = 0;
                byte byte1, byte2;
                ArithmeticOperator ArithOp = 0;
                BitOperator BitOp = 0;
                LogicOperator LogOp = 0;
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
                        // parameter > 127 ? => F-Register Address = Parameter
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        // operating bytes
                        byte1 = wreg;
                        byte2 = registerFileMap.Get(parameter);
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case ADDWL_1:
                    case ADDWL_2:
                        ArithOp = ArithmeticOperator.PLUS;
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        target = wreg;
                        byte1 = wreg;
                        byte2 = (byte)parameter;
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case INCF:
                        ArithOp = ArithmeticOperator.PLUS;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = 0x01;
                        byte2 = registerFileMap.Get(parameter);
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case SUBWF:
                        ArithOp = ArithmeticOperator.MINUS;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
                        byte2 = wreg;
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case SUBLW_1:
                    case SUBLW_2:
                        ArithOp = ArithmeticOperator.MINUS;
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        target = wreg;
                        byte1 = (byte)parameter;
                        byte2 = wreg;
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case DECF:
                        ArithOp = ArithmeticOperator.MINUS;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
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
                        target = (short)(parameter & 0x007F);
                        // bit-number
                        bit = (short)(operation+parameter & 0x0380);
                        return new BitOperation(target, bit, BitOp, registerFileMap, address);
                    case BSF_1:
                    case BSF_2:
                    case BSF_3:
                    case BSF_4:
                        BitOp = BitOperator.BITSET;
                        target = (short)(parameter & 0x007F);
                        bit = (short)(operation + parameter & 0x0380);
                        return new BitOperation(target, bit, BitOp, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- CALL OPERATIONS ----------------------------- */

                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- CLEAR OPERATIONS ---------------------------- */
                    case CLRF_CLRW:
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        return new ClearOperation(target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- COMPLEMENT OPERATIONS ----------------------- */
                    case COMF:
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        return new ComplementOperation(target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- LOGIC OPERATIONS ---------------------------- */
                    case ANDWF:
                        LogOp = LogicOperator.AND;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case ANDWL:
                        LogOp = LogicOperator.AND;
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        target = wreg;
                        byte1 = (byte)parameter;
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case IORWF:
                        LogOp = LogicOperator.IOR;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case IORLW:
                        LogOp = LogicOperator.IOR;
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        target = wreg;
                        byte1 = (byte)parameter;
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case XORWF:
                        LogOp = LogicOperator.XOR;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case XORLW:
                        LogOp = LogicOperator.XOR;
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        target = wreg;
                        byte1 = (byte)parameter;
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- MOVE OPERATIONS ----------------------------- */
                    case MOVF:
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        source = (short)(parameter & 0x007F);
                        return new MoveOperation(source, target, registerFileMap, address);
                    case MOVLW_1:
                    case MOVLW_2:
                    case MOVLW_3:
                    case MOVLW_4:
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        byte1 = (byte)parameter;
                        return new MoveOperation(byte1, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- ROTATE OPERATIONS --------------------------- */
                    case RLF:
                        RotDir = RotationDirection.LEFT;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        source = (short)(parameter & 0x007F);
                        return new RotateOperation(source, target, RotDir, registerFileMap, address);
                    case RRF:
                        RotDir = RotationDirection.RIGHT;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        source = (short)(parameter & 0x007F);
                        return new RotateOperation(source, target, RotDir, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- SWAP OPERATIONS ----------------------------- */
                    case SWAPF:
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        source = (short)(parameter & 0x007F);
                        return new SwapOperation(source, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    case 0x0000:
                        if(parameter > 127)
                        {
                            // MOVWF
                            target = (short)(parameter & 0x007F);
                            byte1 = wreg;
                            return new MoveOperation(wreg, target, registerFileMap, address);
                        }
                        switch (parameter)
                        {
                            case CLRWDT:
                                // TODO: CLRWDT ADDRESS
                                /* target == CLRWDT_ADDRESS
                                 * return new ClearOperation(target, registerFileMap, address);
                                 */
                                break;
                            case RETFIE:
                                break;
                            case RETURN:
                                break;
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

            }
            catch (Exception ex)
            {
            }
        }

        public Parser(Register.RegisterFileMap _registerFileMap)
        {
            this.registerFileMap = _registerFileMap;
        }
   
    }
}
