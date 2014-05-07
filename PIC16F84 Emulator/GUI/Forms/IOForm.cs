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

        CheckBox[] PortA = new CheckBox[8];
        CheckBox[] PortB = new CheckBox[8];

        public IOForm(RegisterFileMap _registerFileMap)
        {

            this.registerFileMap = _registerFileMap;

            // Listener fuer GUI-Aktualisierung
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

            // PORT-Checkboxes dynamisch erstellen
            for (int i = 0; i < 8; i++)
            {
                PortA[i] = new CheckBox();
                PortA[i].Location = new Point(58 + i * 21, 96);
                PortA[i].MouseClick += new MouseEventHandler(CheckboxChanged);
                PortA[i].Name = "checkBoxBitA" + (7 - i).ToString();
                PortA[i].Width = 15;
                PortA[i].Height = 15;

                PortB[i] = new CheckBox();
                PortB[i].Location = new Point(58 + i * 21, 50);
                PortB[i].MouseClick += new MouseEventHandler(CheckboxChanged);
                PortB[i].Name = "checkBoxBitB" + (7 - i).ToString();
                PortB[i].Width = 15;
                PortB[i].Height = 15;
            }

            this.Controls.AddRange(PortA);
            this.Controls.AddRange(PortB);

            InitializeComponent();

            updateTrisA(registerFileMap.Get(RegisterConstants.TRISA_BANK1_ADDRESS));
            updateTrisB(registerFileMap.Get(RegisterConstants.TRISB_BANK1_ADDRESS));
            updatePortA(registerFileMap.Get(RegisterConstants.PORTA_ADDRESS));
            updatePortB(registerFileMap.Get(RegisterConstants.PORTB_ADDRESS));

        }

        /// <summary>
        /// default form position
        /// </summary>
        /// <param name="_top">control-form height</param>
        public void defaultView(int _top)
        {
            this.Location = new Point(MdiParent.ClientRectangle.Right - (this.Width + 5), _top);
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
            // xxxx xxxx & 0x01 = 0000 000x => checkBoxBitA0 = x etc...
            for (int i = 0; i < PortA.Length; i++)
                PortA[(7-i)].Checked = (_value & (0x01 << i)) != 0 ? true : false;
        }
        private void updatePortB(byte _value)
        {
            // xxxx xxxx & 0x01 = 0000 000x => checkBoxBitB0 = x etc...
            for (int i = 0; i < PortB.Length; i++)
                PortB[(7 - i)].Checked = (_value & (0x01 << i)) != 0 ? true : false;
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

        private void CheckboxChanged(object sender, EventArgs e)
        {
            CheckBox bit;
            byte byteMask = 0x00;
            int bitNumber = 0;
            short portAddress = 0;

            try
            {
                bit = (CheckBox)sender;
                // bit.Name = "checkBoxBit" + [A|B] + x(=bitNumber)
                bitNumber = Int32.Parse(bit.Name.Substring(bit.Name.Length - 1));
                portAddress = bit.Name.Substring(bit.Name.Length - 2, 1) == "A" ? RegisterConstants.PORTA_ADDRESS : RegisterConstants.PORTB_ADDRESS;
                byteMask = (byte)(0x01 << bitNumber);
                
                if (bit.Checked)
                    ((IOAdapter)(registerFileMap.getAdapter(portAddress))).setBit(byteMask);
                else
                    ((IOAdapter)(registerFileMap.getAdapter(portAddress))).clearBit(byteMask);
            }
            catch
            {
                throw new Exception("failed to handle user input");
            }
        }
    }
}