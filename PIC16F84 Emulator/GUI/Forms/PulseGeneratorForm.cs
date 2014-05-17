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
    public partial class PulseGeneratorForm : Form
    {
        private const int DEFAULT_INTERVAL = 1000;
        private const String BUTTON_ENABLE_TEXT = "Activate";
        private const String BUTTON_DISABLE_TEXT = "Deactivate";

        private const short CHECKBOX_X_OFFSET = 10;
        private const short CHECKBOX_WIDTH = 15;
        private const short CHECKBOX_SPACING = 6;
        private const short CHECKBOX_PORTA_Y_OFFSET = 53;
        private const short CHECKBOX_PORTB_Y_OFFSET = 143;

        private const short LABEL_PORTA_Y_OFFSET = CHECKBOX_PORTA_Y_OFFSET + 15;
        private const short LABEL_PORTB_Y_OFFSET = CHECKBOX_PORTB_Y_OFFSET + 15;

        private CheckBox[] portACheckBoxes = new CheckBox[8];
        private CheckBox[] portBCheckBoxes = new CheckBox[8];

        private System.Timers.Timer portATimer = new System.Timers.Timer(DEFAULT_INTERVAL);
        private System.Timers.Timer portBTimer = new System.Timers.Timer(DEFAULT_INTERVAL);

        private byte portABitmask = 0x00;
        private byte portBBitmask = 0x00;

        private PIC.Ports.IOAdapter portAAdapter;
        private PIC.Ports.IOAdapter portBAdapter;

        private bool portALastValue = false;
        private bool portBLastValue = false;


        public PulseGeneratorForm(PIC.PIC pic)
        {
            InitializeComponent();

            portAAdapter = (PIC.Ports.IOAdapter) pic.getRegisterFileMap().getAdapter(PIC.Register.RegisterConstants.PORTA_ADDRESS);
            portBAdapter = (PIC.Ports.IOAdapter) pic.getRegisterFileMap().getAdapter(PIC.Register.RegisterConstants.PORTB_ADDRESS);

            intervalABox.Text = DEFAULT_INTERVAL.ToString();
            intervalBBox.Text = DEFAULT_INTERVAL.ToString();

            portATimer.Elapsed += portATimer_Elapsed;
            portBTimer.Elapsed += portBTimer_Elapsed;

            int i = 0;
            Label label;
            // PORT-Checkboxes dynamisch erstellen
            for (i = 0; i < 8; i++)
            {
                portACheckBoxes[7-i] = new CheckBox();
                portACheckBoxes[7-i].SetBounds(CHECKBOX_X_OFFSET + i * (CHECKBOX_WIDTH + CHECKBOX_SPACING), CHECKBOX_PORTA_Y_OFFSET, CHECKBOX_WIDTH, CHECKBOX_WIDTH);
                portACheckBoxes[7-i].Parent = this;
                portACheckBoxes[7-i].MouseClick += new MouseEventHandler(portACheckboxChanged);

                label = new Label();
                label.Text = (7 - i).ToString();
                label.SetBounds(CHECKBOX_X_OFFSET + i * (CHECKBOX_WIDTH + CHECKBOX_SPACING), LABEL_PORTA_Y_OFFSET, CHECKBOX_WIDTH, CHECKBOX_WIDTH);
                label.Parent = this;
                label.Show();
                label.BringToFront();

                portBCheckBoxes[7-i] = new CheckBox();
                portBCheckBoxes[7-i].SetBounds(CHECKBOX_X_OFFSET + i * (CHECKBOX_WIDTH + CHECKBOX_SPACING), CHECKBOX_PORTB_Y_OFFSET, CHECKBOX_WIDTH, CHECKBOX_WIDTH);
                portBCheckBoxes[7-i].Parent = this;
                portBCheckBoxes[7-i].MouseClick += new MouseEventHandler(portBCheckboxChanged);

                label = new Label();
                label.Text = (7 - i).ToString();
                label.SetBounds(CHECKBOX_X_OFFSET + i * (CHECKBOX_WIDTH + CHECKBOX_SPACING), LABEL_PORTB_Y_OFFSET, CHECKBOX_WIDTH, CHECKBOX_WIDTH);
                label.Parent = this;
                label.Show();
                label.BringToFront();
            }
        }

        private void portACheckboxChanged(object sender, MouseEventArgs e)
        {
            portABitmask = parseCheckboxes(portACheckBoxes);
        }

        private void portBCheckboxChanged(object sender, MouseEventArgs e)
        {
            portBBitmask = parseCheckboxes(portBCheckBoxes);
        }

        private byte parseCheckboxes(CheckBox[] checkBoxes)
        {
            byte result = 0x00;
            result += (byte) (checkBoxes[0].Checked ? 0x01 :0);
            result += (byte) (checkBoxes[1].Checked ? 0x02 :0);
            result += (byte) (checkBoxes[2].Checked ? 0x04 :0);
            result += (byte) (checkBoxes[3].Checked ? 0x08 :0);

            result += (byte) (checkBoxes[4].Checked ? 0x10 :0);
            result += (byte) (checkBoxes[5].Checked ? 0x20 :0);
            result += (byte) (checkBoxes[6].Checked ? 0x40 :0);
            result += (byte) (checkBoxes[7].Checked ? 0x80 :0);
            return result;
        }

        private void intervalABox_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Return)
            {
                int newInterval = 0;
                int.TryParse(intervalABox.Text, out newInterval);
                if(newInterval > 1) {
                    portATimer.Interval = newInterval;                     
                }
            }
        }

        private void intervalBBox_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Return)
            {
                int newInterval = 0;
                int.TryParse(intervalABox.Text, out newInterval);
                if (newInterval > 1)
                {
                    portBTimer.Interval = newInterval;
                }
            }
        }

        void portATimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (portALastValue)
            {
                clearSelectedBits(portAAdapter, portABitmask);
                portALastValue = false;
            }
            else
            {
                setSelectedBits(portAAdapter, portABitmask);
                portALastValue = true;
            }
        }

        void portBTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (portBLastValue)
            {
                clearSelectedBits(portBAdapter, portBBitmask);
                portBLastValue = false;
            }
            else
            {
                setSelectedBits(portBAdapter, portBBitmask);
                portBLastValue = true;
            }
        }

        private void setSelectedBits(PIC.Ports.IOAdapter portAdapter, byte portBitmask)
        {
            portAdapter.Input = (byte)(portAdapter.Value | portBitmask);
        }

        private void clearSelectedBits(PIC.Ports.IOAdapter portAdapter, byte portBitmask)
        {
            portAdapter.Input = (byte)(portAdapter.Value & (~portBitmask));
        }

        private void togglePortAButton_Click(object sender, EventArgs e)
        {
            if (portATimer.Enabled)
            {
                portATimer.Stop();
                togglePortAButton.Text = BUTTON_ENABLE_TEXT;
            }
            else
            {
                portATimer.Start();
                togglePortAButton.Text = BUTTON_DISABLE_TEXT;
            }
        }

        private void togglePortBButton_Click(object sender, EventArgs e)
        {
            if (portBTimer.Enabled)
            {
                portBTimer.Stop();
                togglePortBButton.Text = BUTTON_ENABLE_TEXT;
            }
            else
            {
                portBTimer.Start();
                togglePortBButton.Text = BUTTON_DISABLE_TEXT;
            }
        }


        internal void defaultView(int _top)
        {
            this.Location = new Point(MdiParent.ClientRectangle.Right - (this.Width + 5), _top);
        }
    }
}
