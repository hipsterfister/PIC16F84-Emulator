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
    public partial class PICEmulatorForm : Form
    {
        protected PIC.PIC pic;

        public PICEmulatorForm()
        {
            InitializeComponent();
        }

        private void showOpenFileDialog(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            closeAllOpenWindows();

            if (pic != null)
            {
                pic.dispose();
            }
            
            this.Activate();
            string file = openFileDialog1.FileName;
             
            // Initialize PIC
            this.pic = new PIC.PIC();
            pic.initProgramMemory(file);

            // Create a new Listing Form and show it!
            ListingForm newListingForm = new ListingForm(file, pic);
            newListingForm.MdiParent = this;
            newListingForm.Show();
            
        }

        private void showControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new Control Form and show it!
            ControlForm newControlForm = new ControlForm(pic);
            newControlForm.MdiParent = this;
            newControlForm.Show();
        }

        private void showRegisterMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new RegisterMapForm and show it!
            RegisterMapForm newRegisterMapForm = new RegisterMapForm(pic.getRegisterFileMap());
            newRegisterMapForm.MdiParent = this;
            newRegisterMapForm.Show();
        }

        private void closeAllOpenWindows()
        {
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }
        }
    }
}
