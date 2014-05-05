using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Register;
using PIC16F84_Emulator.PIC.Data;
using PIC16F84_Emulator.PIC.Ports;

namespace PIC16F84_Emulator.GUI.Forms
{
    public partial class IOForm : Form
    {
        DataAdapter<byte>.OnDataChanged portAListener;
        DataAdapter<byte>.OnDataChanged portBListener;
        DataAdapter<byte>.OnDataChanged trisAListener;
        DataAdapter<byte>.OnDataChanged trisBListener;

        RegisterFileMap registerFileMap;

        public IOForm(RegisterFileMap _registerFileMap)
        {
            InitializeComponent();

            this.registerFileMap = _registerFileMap;

            this.portAListener = new DataAdapter<byte>.OnDataChanged(onPortAChange);
            this.portBListener = new DataAdapter<byte>.OnDataChanged(onPortBChange);
            this.trisAListener = new DataAdapter<byte>.OnDataChanged(onTrisAChange);
            this.trisBListener = new DataAdapter<byte>.OnDataChanged(onTrisBChange);

            registerFileMap.registerDataListener(portAListener, RegisterConstants.PORTA_ADDRESS);
            registerFileMap.registerDataListener(portBListener, RegisterConstants.PORTB_ADDRESS);
            registerFileMap.registerDataListener(trisAListener, RegisterConstants.TRISA_BANK1_ADDRESS);
            registerFileMap.registerDataListener(trisBListener, RegisterConstants.TRISB_BANK1_ADDRESS);

            Disposed += delegate { registerFileMap.unregisterDataListener(portAListener, RegisterConstants.PORTA_ADDRESS); };
            Disposed += delegate { registerFileMap.unregisterDataListener(portBListener, RegisterConstants.PORTB_ADDRESS); };
            Disposed += delegate { registerFileMap.unregisterDataListener(trisAListener, RegisterConstants.TRISA_BANK1_ADDRESS); };
            Disposed += delegate { registerFileMap.unregisterDataListener(trisBListener, RegisterConstants.TRISB_BANK1_ADDRESS); };

            updateTrisA(registerFileMap.Get(RegisterConstants.TRISA_BANK1_ADDRESS));
            updateTrisB(registerFileMap.Get(RegisterConstants.TRISB_BANK1_ADDRESS));

        }

        public void onPortAChange(byte Value, object Sender)
        {
            MethodInvoker mi = delegate { updatePortA(Value); };
            if (InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(mi); // Async to prohibit deadlock
                }
                catch (ObjectDisposedException)
                {
                    // nothing to do here.
                }
            }
            else
            {
                updatePortA(Value);
            }
        }

        public void onPortBChange(byte Value, object Sender)
        {
            MethodInvoker mi = delegate { updatePortB(Value); };
            if (InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(mi); // Async to prohibit deadlock
                }
                catch (ObjectDisposedException)
                {
                    // nothing to do here.
                }
            }
            else
            {
                updatePortB(Value);
            }
        }

        public void onTrisAChange(byte Value, object Sender)
        {
            MethodInvoker mi = delegate { updateTrisA(Value); };
            if (InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(mi); // Async to prohibit deadlock
                }
                catch (ObjectDisposedException)
                {
                    // nothing to do here.
                }
            }
            else
            {
                updateTrisA(Value);
            }
        }

        public void onTrisBChange(byte Value, object Sender)
        {
            MethodInvoker mi = delegate { updateTrisB(Value); };
            if (InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(mi); // Async to prohibit deadlock
                }
                catch (ObjectDisposedException)
                {
                    // nothing to do here.
                }
            }
            else
            {
                updateTrisB(Value);
            }
        }

        private void updatePortA(byte _value)
        {
            // xxxx xxxx & 0x01 = 0000 000x => checkbox = x
            this.checkBoxBitA0.Checked = (_value & 0x01) != 0 ? true : false;
            this.checkBoxBitA1.Checked = (_value & 0x02) != 0 ? true : false;
            this.checkBoxBitA2.Checked = (_value & 0x04) != 0 ? true : false;
            this.checkBoxBitA3.Checked = (_value & 0x08) != 0 ? true : false;
            this.checkBoxBitA4.Checked = (_value & 0x10) != 0 ? true : false;
            this.checkBoxBitA5.Checked = (_value & 0x20) != 0 ? true : false;
            this.checkBoxBitA6.Checked = (_value & 0x40) != 0 ? true : false;
            this.checkBoxBitA7.Checked = (_value & 0x80) != 0 ? true : false;
        }

        private void updatePortB(byte _value)
        {
            // xxxx xxxx & 0x01 = 0000 000x => checkbox = x
            this.checkBoxBitB0.Checked = (_value & 0x01) != 0 ? true : false;
            this.checkBoxBitB1.Checked = (_value & 0x02) != 0 ? true : false;
            this.checkBoxBitB2.Checked = (_value & 0x04) != 0 ? true : false;
            this.checkBoxBitB3.Checked = (_value & 0x08) != 0 ? true : false;
            this.checkBoxBitB4.Checked = (_value & 0x10) != 0 ? true : false;
            this.checkBoxBitB5.Checked = (_value & 0x20) != 0 ? true : false;
            this.checkBoxBitB6.Checked = (_value & 0x40) != 0 ? true : false;
            this.checkBoxBitB7.Checked = (_value & 0x80) != 0 ? true : false;
        }
        private void updateTrisA(byte _value)
        {
            this.labelTrisA0.Text = (_value & 0x01) != 0 ? "i" : "o";
            this.labelTrisA1.Text = (_value & 0x02) != 0 ? "i" : "o";
            this.labelTrisA2.Text = (_value & 0x04) != 0 ? "i" : "o";
            this.labelTrisA3.Text = (_value & 0x08) != 0 ? "i" : "o";
            this.labelTrisA4.Text = (_value & 0x10) != 0 ? "i" : "o";
            this.labelTrisA5.Text = (_value & 0x20) != 0 ? "i" : "o";
            this.labelTrisA6.Text = (_value & 0x40) != 0 ? "i" : "o";
            this.labelTrisA7.Text = (_value & 0x80) != 0 ? "i" : "o";
        }
        private void updateTrisB(byte _value)
        {
            this.labelTrisB0.Text = (_value & 0x01) != 0 ? "i" : "o";
            this.labelTrisB1.Text = (_value & 0x02) != 0 ? "i" : "o";
            this.labelTrisB2.Text = (_value & 0x04) != 0 ? "i" : "o";
            this.labelTrisB3.Text = (_value & 0x08) != 0 ? "i" : "o";
            this.labelTrisB4.Text = (_value & 0x10) != 0 ? "i" : "o";
            this.labelTrisB5.Text = (_value & 0x20) != 0 ? "i" : "o";
            this.labelTrisB6.Text = (_value & 0x40) != 0 ? "i" : "o";
            this.labelTrisB7.Text = (_value & 0x80) != 0 ? "i" : "o";
        }

        private void checkBoxBitA0_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitA0.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).setBit(0x01);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).clearBit(0x01);
        }

        private void checkBoxBitA1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitA1.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).setBit(0x02);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).clearBit(0x02);

        }

        private void checkBoxBitA2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitA2.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).setBit(0x04);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).clearBit(0x04);

        }

        private void checkBoxBitA3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitA3.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).setBit(0x08);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).clearBit(0x08);

        }

        private void checkBoxBitA4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitA4.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).setBit(0x10);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).clearBit(0x10);

        }

        private void checkBoxBitA5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitA5.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).setBit(0x20);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).clearBit(0x20);

        }

        private void checkBoxBitA6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitA6.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).setBit(0x40);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).clearBit(0x40);

        }

        private void checkBoxBitA7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitA7.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).setBit(0x80);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTA_ADDRESS))).clearBit(0x80);

        }

        private void checkBoxBitB0_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitB0.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).setBit(0x01);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).clearBit(0x01);
        }

        private void checkBoxBitB1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitB1.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).setBit(0x02);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).clearBit(0x02);

        }

        private void checkBoxBitB2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitB2.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).setBit(0x04);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).clearBit(0x04);

        }

        private void checkBoxBitB3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitB3.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).setBit(0x08);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).clearBit(0x08);

        }

        private void checkBoxBitB4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitB4.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).setBit(0x10);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).clearBit(0x10);
        }

        private void checkBoxBitB5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitB5.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).setBit(0x20);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).clearBit(0x20);
        }

        private void checkBoxBitB6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitB6.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).setBit(0x40);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).clearBit(0x40);
        }

        private void checkBoxBitB7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBitB7.Checked)
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).setBit(0x80);
            else
                ((IOAdapter)(registerFileMap.getAdapter(RegisterConstants.PORTB_ADDRESS))).clearBit(0x80);
        }
    }
}