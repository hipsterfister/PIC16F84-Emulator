using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PIC16F84_Emulator.GUI.Forms
{
    public partial class IOForm : Form
    {
        public IOForm()
        {

            InitializeComponent();
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

    }
}
