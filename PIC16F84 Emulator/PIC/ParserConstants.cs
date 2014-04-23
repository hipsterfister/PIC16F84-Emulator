using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC
{
    class ParserConstants
    {
        /* -------------------------------------- */
        /* BYTE-ORIENTED FILE REGISTER OPERATIONS */
        public const int ADDWF = 0x0700;
        public const int ANDWF = 0x0500;
        public const int CLRF_CLRW = 0x0100;
        public const int COMF = 0x0900;
        public const int DECF = 0x0300;
        public const int DECFSZ = 0x0B00;
        public const int INCF = 0x0A00;
        public const int INCFSZ = 0x0F00;
        public const int IORWF = 0x0400;
        public const int MOVF = 0x0800;
        public const int MOVWF = 0x0000;
        public const int NOP_1 = 0x0000;
        public const int NOP_2 = 0x0020;
        public const int NOP_3 = 0x0040;
        public const int NOP_4 = 0x0060;
        public const int RLF = 0x0D00;
        public const int RRF = 0x0C00;
        public const int SUBWF = 0x0200;
        public const int SWAPF = 0x0E00;
        public const int XORWF = 0x0600;
        /* -------------------------------------- */

        /* -------------------------------------- */
        /* BIT-ORIENTED FILE REGISTER OPERATIONS  */
        public const int BCF_1 = 0x1000;
        public const int BCF_2 = 0x1100;
        public const int BCF_3 = 0x1200;
        public const int BCF_4 = 0x1300;
        public const int BSF_1 = 0x1400;
        public const int BSF_2 = 0x1500;
        public const int BSF_3 = 0x1600;
        public const int BSF_4 = 0x1700;
        public const int BTFSC_1 = 0x1800;
        public const int BTFSC_2 = 0x1900;
        public const int BTFSC_3 = 0x1A00;
        public const int BTFSC_4 = 0x1B00;
        public const int BTFSS_1 = 0x1C00;
        public const int BTFSS_2 = 0x1D00;
        public const int BTFSS_3 = 0x1E00;
        public const int BTFSS_4 = 0x1F00;
        /* -------------------------------------- */

        /* -------------------------------------- */
        /* --- LITERAL AND CONTROL OPERATIONS --- */
        public const int ADDLW_1 = 0x3E00;
        public const int ADDLW_2 = 0x3F00;
        public const int ANDLW = 0x3900;
        public const int CALL_1 = 0x2000;
        public const int CALL_2 = 0x2100;
        public const int CALL_3 = 0x2200;
        public const int CALL_4 = 0x2300;
        public const int CALL_5 = 0x2400;
        public const int CALL_6 = 0x2500;
        public const int CALL_7 = 0x2600;
        public const int CALL_8 = 0x2700;
        public const int CLRWDT = 0x0064;
        public const int GOTO_1 = 0x2800;
        public const int GOTO_2 = 0x2900;
        public const int GOTO_3 = 0x2A00;
        public const int GOTO_4 = 0x2B00;
        public const int GOTO_5 = 0x2C00;
        public const int GOTO_6 = 0x2D00;
        public const int GOTO_7 = 0x2E00;
        public const int GOTO_8 = 0x2F00;
        public const int IORLW = 0x3800;
        public const int MOVLW_1 = 0x3000;
        public const int MOVLW_2 = 0x3100;
        public const int MOVLW_3 = 0x3200;
        public const int MOVLW_4 = 0x3300;
        public const int RETFIE = 0x0009;
        public const int RETLW_1 = 0x3400;
        public const int RETLW_2 = 0x3500;
        public const int RETLW_3 = 0x3600;
        public const int RETLW_4 = 0x3700;
        public const int RETURN = 0x0008;
        public const int SLEEP = 0x0063;
        public const int SUBLW_1 = 0x3C00;
        public const int SUBLW_2 = 0x3D00;
        public const int XORLW = 0x3A00;
        /* -------------------------------------- */
    }
}
