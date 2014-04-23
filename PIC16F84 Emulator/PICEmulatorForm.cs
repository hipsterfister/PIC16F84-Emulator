using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PIC16F84_Emulator
{
    public partial class PICEmulatorForm : Form
    {
        public PICEmulatorForm()
        {
            InitializeComponent();
        }

        private void showOpenFileDialog(object sender, EventArgs e)
        {
            System.Console.WriteLine("klick");
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.Activate();
            string file = openFileDialog1.FileName;

            // Initialize the programMemory
            // TODO: PIC ??!
            PIC.Data.ProgamMemory programMemory = new PIC.Data.ProgamMemory();
            programMemory.readFile(file);

            // Create a new Listing Form and show it!
            ListingForm newListingForm = new ListingForm(file);
            newListingForm.MdiParent = this;
            newListingForm.Show();
        }
    }
}
