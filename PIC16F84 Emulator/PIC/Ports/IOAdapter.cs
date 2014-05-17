using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;
using PIC16F84_Emulator.PIC.Register;

namespace PIC16F84_Emulator.PIC.Ports
{
    /// <summary>
    /// This Class is the implementation for I/O PORTS
    ///     > PORTA (0x05) and TRISA (0x85)
    ///     > PORTB (0x06) and TRISB (0x86)
    /// 
    /// Structures (data sheet p.16 and p.18):
    /// 
    /// NAME  |  Bit 7  |  Bit 6  |  Bit 5  |  Bit 4  |  Bit 3  |  Bit 2  |  Bit 1  |  Bit 0  | 
    /// ------+---------+---------+---------+---------+---------+---------+---------+---------|
    /// PORTA |   ---   |   ---   |   ---   |RA4/T0CKl|   RA3   |   RA2   |   RA1   |   RA0   |
    /// TRISA |   ---   |   ---   |   ---   | TRISA4  | TRISA3  | TRISA2  | TRISA1  | TRISA0  |
    /// ------+---------+---------+---------+---------+---------+---------+---------+---------|
    /// PORTB |   RB7   |   RB6   |   RB5   |   RB4   |   RB3   |   RB2   |   RB1   | RB0/INT |
    /// TRISB | TRISB7  | TRISB6  | TRISB5  | TRISB4  | TRISB3  | TRISB2  | TRISB1  | TRISB0  |
    /// ------+---------+---------+---------+---------+---------+---------+---------+---------|
    /// 
    /// OnChange Interrupt:
    /// RB7, RB6, RB5, RB4
    /// </summary>
    /// 

    class IOAdapter : DataAdapter<byte>
    {
        private DataAdapter<byte>.OnDataChanged trisChangeListener;

        protected byte tris;
        private short portAddress;

        public override byte Value
        {
            get
            {
                return _Data;
            }
            set
            {
                // xxxx xxxx AND ~(iiii oooo) = 0000 xxxx
                _Data = (byte)(value & ~tris);
                onDataChanged(_Data, this);
            }
        }

        public byte Input
        {
            set
            {
                _Data = (byte)(value & tris);
                onDataChanged(_Data, this);
            }
        }

        /// <summary>
        /// New IOAdapter for an I/O-Port
        /// </summary>
        /// <param name="_address">Port address</param>
        public IOAdapter(RegisterFileMap _registerFileMap, short _address)
        {
            this.portAddress = _address;

            this.trisChangeListener = new DataAdapter<byte>.OnDataChanged(onTrisChange);
            _registerFileMap.registerDataListener(trisChangeListener, (short)(portAddress + 0x80));
        }

        /// <summary>
        /// Get Tris Value
        /// </summary>
        private void onTrisChange(byte Value, object Sender)
        {
            this.tris = Value;
        }

        /// <summary>
        /// unregister the tris listener
        /// </summary>
        public void dispose(RegisterFileMap _registerFileMap)
        {
            _registerFileMap.unregisterDataListener(trisChangeListener, (short)(portAddress + 0x80));
        }

        /// <summary>
        /// Sets the specified bits.
        /// </summary>
        /// <param name="_bitMask">Bitmask, selected bits == 1</param>
        public void setBit(byte _bitMask)
        {
            _Data = (byte)(_Data | _bitMask);
            onDataChanged(_Data, this);
        }

        /// <summary>
        /// Clears the specified bits.
        /// </summary>
        /// <param name="_bitMask">Bitmask, selected bits == 1</param>
        public void clearBit(byte _bitMask)
        {
            _Data = (byte)(_Data & ~_bitMask); 
            onDataChanged(_Data, this);
        }
    }
}
