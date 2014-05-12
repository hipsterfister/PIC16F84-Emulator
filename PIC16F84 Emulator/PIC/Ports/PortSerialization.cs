using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Ports
{
    internal class PortSerialization
    {
        private const byte CODE_SYMBOL = 0x30;
        private const byte CODE_CARRIAGE_RETURN = 0x0D;

        private const int BAUD_RATE = 4800;
        private const int DATA_BITS = 8;
        private const String PORT_NAME = "COM3";
        private const System.IO.Ports.StopBits STOP_BITS = System.IO.Ports.StopBits.One;
        private const System.IO.Ports.Parity PARITY = System.IO.Ports.Parity.None;

        private Data.DataAdapter<byte> portA;
        private Data.DataAdapter<byte> portB;

        private Data.DataAdapter<byte> trisA;
        private Data.DataAdapter<byte> trisB;

        private bool serializationIsActive = false;

        private System.IO.Ports.SerialPort comPort = new System.IO.Ports.SerialPort();

        internal PortSerialization(Register.RegisterFileMap registerFileMap)
        {
            this.portA = registerFileMap.getAdapter(Register.RegisterConstants.PORTA_ADDRESS);
            this.portB = registerFileMap.getAdapter(Register.RegisterConstants.PORTB_ADDRESS);

            this.trisA = registerFileMap.getAdapter(Register.RegisterConstants.TRISA_BANK1_ADDRESS);
            this.trisB = registerFileMap.getAdapter(Register.RegisterConstants.TRISB_BANK1_ADDRESS);

            comPort.DataReceived += comPort_DataReceived;
        }

        internal bool openPort() {
            try
            {
                if (comPort.IsOpen == true) 
                    comPort.Close();

                comPort.BaudRate = BAUD_RATE;
                comPort.DataBits = DATA_BITS;
                comPort.StopBits = STOP_BITS;
                comPort.Parity   = PARITY;
                comPort.PortName = PORT_NAME;

                comPort.Open();

                return true;
            }
            catch (Exception)
            {
                // TODO: notify user
                return false;
            }
        }

        internal bool closePort()
        {
            if (comPort.IsOpen == true)
                comPort.Close();
            
            return true;
        }

        internal void send()
        {
            this.writeData();
        }

        internal void startSerialization()
        {
            if (!serializationIsActive) 
            {
                portA.DataChanged += onValueChangeListener;
                portB.DataChanged += onValueChangeListener;

                trisA.DataChanged += onValueChangeListener;
                trisB.DataChanged += onValueChangeListener;

                serializationIsActive = true;
            }
        }

        internal void endSerialization()
        {
            if (serializationIsActive)
            {
                portA.DataChanged -= onValueChangeListener;
                portB.DataChanged -= onValueChangeListener;

                trisA.DataChanged -= onValueChangeListener;
                trisB.DataChanged -= onValueChangeListener;

                serializationIsActive = false;
            }
        }

        private void onValueChangeListener(byte value, object sender)
        {
            send();
        }

        private void comPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int bytes = comPort.BytesToRead;
            byte[] comBuffer = new byte[bytes];

            comPort.Read(comBuffer, 0, bytes);

            if (bytes == 5 && comBuffer[4] == CODE_CARRIAGE_RETURN)
            {
                portA.Value = decodeByte(comBuffer[0], comBuffer[1]);
                portB.Value = decodeByte(comBuffer[2], comBuffer[3]);
            }
            System.Console.WriteLine(comBuffer);
        }

        private byte[] serializeCurrentValues()
        {
            byte[] sequence = new byte[9];

            sequence[0] = encodeLowerNibble(trisA.Value);
            sequence[1] = encodeHigherNibble(trisA.Value);

            sequence[2] = encodeLowerNibble(portA.Value);
            sequence[3] = encodeHigherNibble(portA.Value);

            sequence[4] = encodeLowerNibble(trisB.Value);
            sequence[5] = encodeHigherNibble(trisB.Value);

            sequence[6] = encodeLowerNibble(portB.Value);
            sequence[7] = encodeHigherNibble(portB.Value);

            sequence[8] = CODE_CARRIAGE_RETURN;

            return sequence;
        }

        private byte encodeLowerNibble(byte value)
        {
            byte result = (byte)(value & 0xF);
            result += CODE_SYMBOL;
            return result;
        }

        private byte encodeHigherNibble(byte value)
        {
            byte result = (byte)(value >> 4);
            result += CODE_SYMBOL;
            return result;
        }

        private byte decodeByte(byte lowNibble, byte highNibble)
        {
            byte lowResult = (byte)(lowNibble & 0xF);
            byte highResult = (byte)(highNibble & 0xF);
            highResult = (byte)(highResult << 4);
            return (byte)(lowResult + highResult);
        }

        private void writeData()
        {
            byte[] msg = serializeCurrentValues();

            if (!(comPort.IsOpen == true))
                comPort.Open();
            comPort.Write(msg, 0, msg.Length);

            System.Console.WriteLine(msg);
        }



    }

}
