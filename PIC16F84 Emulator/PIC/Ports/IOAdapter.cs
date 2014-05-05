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

    class IOAdapter<T> : DataAdapter<byte>
    {
        private DataAdapter<byte>.OnDataChanged trisChangeListener;

        protected byte tris;
        private short portAddress;

        public byte Tris
        {
            get
            {
                return tris;
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
    }
}
