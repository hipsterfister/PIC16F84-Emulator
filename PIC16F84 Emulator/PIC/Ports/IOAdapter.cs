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
        public delegate void OnTrisChanged(T Value, object Sender);
        public event OnTrisChanged TrisChanged;

        protected T tris;
        private short portAddress;
        private short trisAddress;

        public T Tris
        {
            get
            {
                return tris;
            }
            set
            {
                tris = value;
                if (TrisChanged != null)
                    TrisChanged(value, this);
            }
        }

        /// <summary>
        /// New IOAdapter for an I/O-Port
        /// </summary>
        /// <param name="_portAddress">Port Address on Bank 0</param>
        public IOAdapter(short _portAddress)
        {
            this.portAddress = _portAddress;
            this.trisAddress = (short)(_portAddress + 0x80);
        }
    }
}
