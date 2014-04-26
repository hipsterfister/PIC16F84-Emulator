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
    public partial class ControlForm : Form
    {
        protected PIC.PIC pic;

        public ControlForm(PIC.PIC _pic)
        {
            InitializeComponent();

            this.pic = _pic;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            pic.beginExecution();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            pic.stopExecution();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            pic.onCycleEnd();
        }
    }
}
