﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;
using PIC16F84_Emulator.PIC.Ports;

namespace PIC16F84_Emulator.PIC.Register
{
    public class RegisterFileMap
    {
        protected DataAdapter<byte>[] Data;

        public RegisterFileMap()
        {
            Data = new DataAdapter<byte>[RegisterConstants.REGISTER_FILE_MAP_SIZE];  // Data[256] <= Working Register, 257 <= WDT
            for (int X = 0; X < Data.Length; X++)
            {
                Data[X] = new DataAdapter<byte>();
            }

            // I/O Ports as IOAdapter
            Data[RegisterConstants.PORTA_ADDRESS] = new IOAdapter(this, RegisterConstants.PORTA_ADDRESS);
            Data[RegisterConstants.PORTB_ADDRESS] = new IOAdapter(this, RegisterConstants.PORTB_ADDRESS);

            initializeValues();
        }

        public void initializeValues()
        {
            for (int X = 0; X < Data.Length; X++)
            {
                Data[X].Value = 0;
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

        public void Set(byte _data, int _position)
        {
            int position = _position;
            if (IsBank1() && position < 0x80)
                position += 0x80;
            if (isIndirect(position))
            {
                position = Data[RegisterConstants.FSR_ADDRESS].Value;
            }
            this.Data[position].Value = _data;

            // TODO: Überarbeiten (hinter if stecken, prüfen ob für weitere Register notwendig...)
            // Überlegung: über onChange events?
            switch (position) // STATUS-Register spiegeln
            {
                case RegisterConstants.STATUS_ADDRESS:
                    this.Data[RegisterConstants.STATUS_BANK1_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.STATUS_BANK1_ADDRESS:
                    this.Data[RegisterConstants.STATUS_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.INDF_ADDRESS:
                    this.Data[RegisterConstants.INDF_BANK1_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.INDF_BANK1_ADDRESS:
                    this.Data[RegisterConstants.INDF_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.PCLATH_ADDRESS:
                    this.Data[RegisterConstants.PCLATH_BANK1_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.PCLATH_BANK1_ADDRESS:
                    this.Data[RegisterConstants.PCLATH_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.FSR_ADDRESS:
                    this.Data[RegisterConstants.FSR_BANK1_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.FSR_BANK1_ADDRESS:
                    this.Data[RegisterConstants.FSR_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.PCL_ADDRESS:
                    this.Data[RegisterConstants.PCL_BANK1_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.PCL_BANK1_ADDRESS:
                    this.Data[RegisterConstants.PCL_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.INTCON_ADDRESS:
                    this.Data[RegisterConstants.INTCON_BANK1_ADDRESS].Value = _data;
                    break;
                case RegisterConstants.INTCON_BANK1_ADDRESS:
                    this.Data[RegisterConstants.INTCON_ADDRESS].Value = _data;
                    break;
                default:
                    break;
            }
        }

        public byte Get(int _position)
        {
            int position = _position;
            if (IsBank1() && position < 0x80)
                position += 0x80;
            if (isIndirect(position))
            {
                position = Data[RegisterConstants.FSR_ADDRESS].Value;
            }
            return Data[position].Value;
        }

        public bool IsBank1()
        {
            return (Data[RegisterConstants.STATUS_ADDRESS].Value & (1 << 5)) != 0;
        }

        private bool isIndirect(int position)
        {
            return (position == RegisterConstants.INDF_ADDRESS || position == RegisterConstants.INDF_BANK1_ADDRESS);
        }

        /// <summary>
        /// Sets the specified bits.
        /// </summary>
        /// <param name="_targetAddress"></param>
        /// <param name="_bitMask">Bitmask, selected bits == 1</param>
        public void setBit(short _targetAddress, byte _bitMask) {
            this.Data[_targetAddress].Value = (byte)(this.Data[_targetAddress].Value | _bitMask);
        }

        /// <summary>
        /// Clears the specified bits.
        /// </summary>
        /// <param name="_targetAddress"></param>
        /// <param name="_bitMask">Bitmask, selected bits == 1</param>
        public void clearBit(short _targetAddress, byte _bitMask)
        {
            this.Data[_targetAddress].Value = (byte)(this.Data[_targetAddress].Value & ~_bitMask);
        }

        /// <summary>
        /// Updates the Carry bit of STATUS register
        /// </summary>
        /// <param name="_value">value to write (1=true, 0=false)</param>
        public void updateCarryFlag(bool _value) {
            if (_value)
            {
                this.setCarryFlag();
            }
            else
            {
                this.clearCarryFlag();
            }
        }

        /// <summary>
        /// Sets the Carry bit of STATUS register
        /// </summary>
        public void setCarryFlag() {
            this.setBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_CARRY_MASK);
        }

        /// <summary>
        /// Clears the Carry bit of STATUS register
        /// </summary>
        public void clearCarryFlag() {
           this.clearBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_CARRY_MASK);
        }

        /// <summary>
        /// Updates the Zero bit of STATUS register
        /// </summary>
        /// <param name="_value">value to write (1=true, 0=false)</param>
        public void updateZeroFlag(bool _value)
        {
            if (_value)
            {
                this.setZeroFlag();
            }
            else
            {
                this.clearZeroFlag();
            }
        }
        
        /// <summary>
        /// Sets the Zero bit of STATUS register
        /// </summary>
        public void setZeroFlag() {
            this.setBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_ZERO_MASK);
        }

        /// <summary>
        /// Clears the Zero bit of STATUS register
        /// </summary>
        public void clearZeroFlag() {
            this.clearBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_ZERO_MASK);
        }

        /// <summary>
        /// Updates the Digit Carry bit (DC) of STATUS register
        /// </summary>
        /// <param name="_value">value to write (1=true, 0=false)</param>
        public void updateDigitCarry(bool _value)
        {
            if (_value)
            {
                this.setDigitCarry();
            }
            else
            {
                this.clearDigitCarry();
            }
        }
        
        /// <summary>
        /// Sets the Digit Carry (DC) bit of STATUS register
        /// </summary>
        public void setDigitCarry() {
            this.setBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_DIGIT_CARRY_MASK);
        }

        /// <summary>
        /// Clears the Digit Carry (DC) bit of STATUS register
        /// </summary>
        public void clearDigitCarry() {
            this.clearBit(RegisterConstants.STATUS_ADDRESS, RegisterConstants.STATUS_DIGIT_CARRY_MASK);
        }

        /// <summary>
        /// Register a listener with the DataAdapter of the corresponding address
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="address"></param>
        public void registerDataListener(DataAdapter<byte>.OnDataChanged listener, short address) {
            Data[address].DataChanged += listener;
        }

        /// <summary>
        /// Unregister a listener with the DataAdapter of the corresponding address
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="address"></param>
        public void unregisterDataListener(DataAdapter<byte>.OnDataChanged listener, short address)
        {
            Data[address].DataChanged -= listener;
        }

        public DataAdapter<byte> getAdapter(int address)
        {
            return Data[address];
        }
    }
}