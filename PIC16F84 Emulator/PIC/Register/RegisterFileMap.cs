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
        protected Data.OperationStack operationStack = new Data.OperationStack();
        protected ProgramCounter programCounter;

        public RegisterFileMap()
        {
            Data = new DataAdapter<byte>[258];  // Data[256] <= Working Register, 257 <= WDT
            for (int X = 0; X < Data.Length; X++)
            {
                Data[X] = new DataAdapter<byte>();
            }
            programCounter = new ProgramCounter(this);

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
        /// Sets the Program Counter's value to _value
        /// </summary>
        /// <param name="_value"></param>
        public void setProgramCounter(short _value)
        {
            this.programCounter.value = _value;
        }

        /// <summary>
        /// Increments the Program Counter's value by 1
        /// </summary>
        public void incrementProgramCounter()
        {
            this.programCounter.value++;
        }

        /// <summary>
        /// Returns the Program Counter's value
        /// </summary>
        /// <returns></returns>
        public short getProgramCounter()
        {
            return this.programCounter.value;
        }

        /// <summary>
        /// Pops the first element of the stack
        /// </summary>
        /// <returns></returns>
        public short popStack()
        {
            return operationStack.pop();
        }

        /// <summary>
        /// Pushes _value on top of the stack
        /// </summary>
        /// <param name="_value"></param>
        public void pushStack(short _value)
        {
            operationStack.push(_value);
        }
    }
}