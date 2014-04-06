using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Register
{
    public class RegisterFileMap
    {
        protected DataAdapter<byte>[] Data;

        public RegisterFileMap()
        {
            Data = new DataAdapter<byte>[256];
            for (int X = 0; X < Data.Length; X++)
            {
                Data[X] = new DataAdapter<byte>();
            }
            // initialize Special Function Registers
            // Bank 0
            Data[RegisterConstants.PCL_ADDRESS].Value = RegisterConstants.PCL_INITIAL_VALUE;
            Data[RegisterConstants.STATUS_ADDRESS].Value = RegisterConstants.STATUS_INITIAL_VALUE;
            Data[RegisterConstants.PCLATH_ADDRESS].Value = RegisterConstants.PCLATH_INITIAL_VALUE;
            Data[RegisterConstants.INTCON_ADDRESS].Value = RegisterConstants.INTCON_INITIAL_VALUE;
            // Bank 1
            Data[RegisterConstants.OPTION_REG_BANK1_ADDRESS].Value = RegisterConstants.OPTION_REG_INITIAL_VALUE;
            Data[RegisterConstants.PCL_BANK1_ADDRESS].Value = RegisterConstants.PCL_INITIAL_VALUE;
            Data[RegisterConstants.STATUS_BANK1_ADDRESS].Value = RegisterConstants.STATUS_INITIAL_VALUE;
            Data[RegisterConstants.TRISA_BANK1_ADDRESS].Value = RegisterConstants.TRISA_INITIAL_VALUE;
            Data[RegisterConstants.TRISB_BANK1_ADDRESS].Value = RegisterConstants.TRISB_INITIAL_VALUE;
            Data[RegisterConstants.EECON1_BANK1_ADDRESS].Value = RegisterConstants.EECON1_INITIAL_VALUE;
            Data[RegisterConstants.PCLATH_BANK1_ADDRESS].Value = RegisterConstants.PCLATH_INITIAL_VALUE;
            Data[RegisterConstants.INTCON_BANK1_ADDRESS].Value = RegisterConstants.INTCON_INITIAL_VALUE;
        }

        public void Set(byte Data, int Position)
        {
            if (IsBank1())
                Position += 0x80;
            this.Data[Position].Value = Data;

            // TODO: Überarbeiten (hinter if stecken, prüfen ob für weitere Register notwendig...)
            switch (Position) // STATUS-Register spiegeln
            {
                case RegisterConstants.STATUS_ADDRESS:
                    this.Data[RegisterConstants.STATUS_BANK1_ADDRESS].Value = Data;
                    break;
                case RegisterConstants.STATUS_BANK1_ADDRESS:
                    this.Data[RegisterConstants.STATUS_ADDRESS].Value = Data;
                    break;
            }
        }

        public byte Get(int Position)
        {
            if (IsBank1())
                Position += 0x80;
            return Data[Position].Value;
        }

        public bool IsBank1()
        {
            return (Data[2].Value & (1 << 5)) != 0;
        }
    }
}

