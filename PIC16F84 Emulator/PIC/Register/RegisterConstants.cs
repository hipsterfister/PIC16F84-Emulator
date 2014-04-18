using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Register
{
    class RegisterConstants
    {
        // Register Addresses
        // Bank 0
        public const short INDF_ADDRESS = 0x00;
        public const short TMR0_ADDRESS = 0x01;
        public const short PCL_ADDRESS = 0x02;
        public const short STATUS_ADDRESS = 0x03;
        public const short FSR_ADDRESS = 0x04;
        public const short PORTA_ADDRESS = 0x05;
        public const short PORTB_ADDRESS = 0x06;
        public const short EEDATA_ADDRESS = 0x08;
        public const short EEADR_ADDRESS = 0x09;
        public const short PCLATH_ADDRESS = 0x0A;
        public const short INTCON_ADDRESS = 0x0B;
        // Bank 1
        public const short INDF_BANK1_ADDRESS = 0x80;
        public const short OPTION_REG_BANK1_ADDRESS = 0x81;
        public const short PCL_BANK1_ADDRESS = 0x82;
        public const short STATUS_BANK1_ADDRESS = 0x83;
        public const short FSR_BANK1_ADDRESS = 0x84;
        public const short TRISA_BANK1_ADDRESS = 0x85;
        public const short TRISB_BANK1_ADDRESS = 0x86;
        public const short EECON1_BANK1_ADDRESS = 0x88;
        public const short EECON2_BANK1_ADDRESS = 0x89;
        public const short PCLATH_BANK1_ADDRESS = 0x8A;
        public const short INTCON_BANK1_ADDRESS = 0x8B;
        // Special
        public const short WORKING_REGISTER_ADDRESS = 0x100;
        public const short WDT_REGISTER_ADDRESS = 0x101;

        // Notable Addresses
        public const short INTERRUPT_VECTOR_ADDRESS = 0x4;

        // Initial Values
        // Note: These are not organized by Banks, because mirrored Registers share their initial value.
        public const byte PCL_INITIAL_VALUE = 0x00;
        public const byte STATUS_INITIAL_VALUE = 0x18;
        public const byte PCLATH_INITIAL_VALUE = 0x00;
        public const byte INTCON_INITIAL_VALUE = 0x00;
        public const byte OPTION_REG_INITIAL_VALUE = 0xFF;
        public const byte TRISA_INITIAL_VALUE = 0x1F;
        public const byte TRISB_INITIAL_VALUE = 0xFF;
        public const byte EECON1_INITIAL_VALUE = 0x00;

        // STATUS Flags (read as bit-mask)
        public const byte STATUS_CARRY_MASK = 0x01;
        public const byte STATUS_DIGIT_CARRY_MASK = 0x02;
        public const byte STATUS_ZERO_MASK = 0x04;
        public const byte STATUS_PD_MASK = 0x08;
        public const byte STATUS_TO_MASK = 0x10;

        // INTON Flags (read as bit-mask)
        public const byte INTCON_GIE_MASK = 0x80;
        // Source: page 8-9 (pdf pages, not page numbers)
    }

}
