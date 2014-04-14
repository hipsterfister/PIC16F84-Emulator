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
                    case 0x0700:
                        // ADDWF
                        // arithmetical operator
                        ArithOp = ArithmeticOperator.PLUS;
                        // target address
                        // parameter > 127 ? => F-Register Address = Parameter
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        // operating bytes
                        byte1 = wreg;
                        byte2 = registerFileMap.Get(parameter);
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case 0x3E00:
                    case 0x3F00:
                        // ADDLW
                        ArithOp = ArithmeticOperator.PLUS;
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        target = wreg;
                        byte1 = wreg;
                        byte2 = (byte)parameter;
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case 0x0A00:
                        // INCF
                        ArithOp = ArithmeticOperator.PLUS;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = 0x01;
                        byte2 = registerFileMap.Get(parameter);
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case 0x0200:
                        // SUBWF
                        ArithOp = ArithmeticOperator.MINUS;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
                        byte2 = wreg;
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case 0x3C00:
                    case 0x3D00:
                        // SUBLW
                        ArithOp = ArithmeticOperator.MINUS;
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        target = wreg;
                        byte1 = (byte)parameter;
                        byte2 = wreg;
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    case 0x0300:
                        // DECF
                        ArithOp = ArithmeticOperator.MINUS;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
                        byte2 = 0x01;
                        return new ArithmeticOperation(byte1, byte2, ArithOp, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- BIT OPERATIONS ------------------------------ */
                    case 0x1000:
                    case 0x1100:
                    case 0x1200:
                    case 0x1300:
                        // BCF
                        // bit operator
                        BitOp = BitOperator.BITCLEAR;
                        // target address
                        target = (short)(parameter & 0x007F);
                        // bit-number
                        bit = (short)(operation+parameter & 0x0380);
                        return new BitOperation(target, bit, BitOp, registerFileMap, address);
                    case 0x1400:
                    case 0x1500:
                    case 0x1600:
                    case 0x1700:
                        // BSF
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
                    case 0x0100:
                        // CLRF
                        // CLRW
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        return new ClearOperation(target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- COMPLEMENT OPERATIONS ----------------------- */
                    case 0x0900:
                        // COMF
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        return new ComplementOperation(target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- LOGIC OPERATIONS ---------------------------- */
                    case 0x0500:
                        // ANDWF
                        LogOp = LogicOperator.AND;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case 0x3900:
                        // ANDLW
                        LogOp = LogicOperator.AND;
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        target = wreg;
                        byte1 = (byte)parameter;
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case 0x0400:
                        // IORWF
                        LogOp = LogicOperator.IOR;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case 0x3800:
                        // IORLW
                        LogOp = LogicOperator.IOR;
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        target = wreg;
                        byte1 = (byte)parameter;
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case 0x0600:
                        // XORWF
                        LogOp = LogicOperator.XOR;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        byte1 = registerFileMap.Get(parameter);
                        byte2 = (byte)wreg;
                        return new LogicOperation(byte1, byte2, LogOp, target, registerFileMap, address);
                    case 0x3A00:
                        // XORLW
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
                    case 0x0800:
                        // MOVF
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        source = (short)(parameter & 0x007F);
                        return new MoveOperation(source, target, registerFileMap, address);
                    case 0x3000:
                    case 0x3100:
                    case 0x3200:
                    case 0x3300:
                        // MOVLW
                        if (parameter > 255)
                            throw new Exception("too long, too large, too big");
                        byte1 = (byte)parameter;
                        return new MoveOperation(byte1, target, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- ROTATE OPERATIONS --------------------------- */
                    case 0x0D00:
                        // RLF
                        RotDir = RotationDirection.LEFT;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        source = (short)(parameter & 0x007F);
                        return new RotateOperation(source, target, RotDir, registerFileMap, address);
                    case 0x0C00:
                        // RRF
                        RotDir = RotationDirection.RIGHT;
                        target = parameter > 127 ? (short)(parameter & 0x007F) : wreg;
                        source = (short)(parameter & 0x007F);
                        return new RotateOperation(source, target, RotDir, registerFileMap, address);
                    /* ------------------------------------------------------ */

                    /* ------------------------------------------------------ */
                    /* -------- SWAP OPERATIONS ----------------------------- */
                        // SWAPF
                    case 0x0E00:
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
                            case 0x0064:
                                // CLRWDT
                                // TODO: CLRWDT ADDRESS
                                /* target == CLRWDT_ADDRESS
                                 * return new ClearOperation(target, registerFileMap, address);
                                 */
                                break;
                            case 0x0009:
                                // RETFIE
                                break;
                            case 0x0008:
                                // RETURN
                                break;
                            case 0x0063:
                                // SLEEP
                                break;
                            case 0x0000:
                            case 0x0020:
                            case 0x0040:
                            case 0x0060:
                                // NOP
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
