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
        private const String COM3_LABEL_AKTIVIEREN_TEXT = "COM3 Ausgabe aktivieren";
        private const String COM3_LABEL_DEAKTIVIEREN_TEXT = "COM3 Ausgabe deaktivieren";

        protected PIC.PIC pic;
        protected ListingForm listingForm;
        protected ControlForm controlForm;
        protected RegisterMapForm registerMapForm;
        protected IOForm ioForm;
        protected SpecialValueForm specialForm;
        protected PulseGeneratorForm pulseGeneratorForm;
        protected HelpForm helpForm;
        protected string file;

        public PICEmulatorForm()
        {
            InitializeComponent();

            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType() == typeof(MdiClient))
                {
                    ctl.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
                }
            }
            this.SizeChanged += delegate { this.Refresh(); };
        }

        private void showOpenFileDialog(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            freeResources();
            
            this.Activate();
            file = openFileDialog1.FileName;
             
            initNewPIC(file);
            enableMenuItems();
            enableDateiSchließen();
            enableDefaultView();
        }

        /// <summary>
        /// Instances a new PIC
        /// </summary>
        /// <param name="file"></param>
        private void initNewPIC(string file)
        {
            // Initialize PIC
            this.pic = new PIC.PIC();
            pic.initProgramMemory(file);
        }

        /// <summary>
        /// opens the child-forms in a default view
        /// </summary>
        private void enableDefaultView()
        {
            createNewControlForm();
            createNewIOForm();
            createNewRegisterMapForm();
            createNewListingForm();
            createNewSpecialValueForm();
            createNewPulseGeneratorForm();
            arrangeFormsToDefaultView();
        }

        private void arrangeFormsToDefaultView()
        {
            try
            {
                controlForm.defaultView();
                ioForm.defaultView(controlForm.Bounds.Bottom);
                specialForm.defaultView(ioForm.Bounds.Bottom);
                pulseGeneratorForm.defaultView(specialForm.Bounds.Bottom);
                registerMapForm.defaultView(controlForm.Bounds.Left < ioForm.Bounds.Left ? controlForm.Bounds.Left : ioForm.Bounds.Left);
                listingForm.defaultView(registerMapForm.Bounds.Left);
            }
            catch
            {
                return;
            }
        }

        private void PICEmulatorForm_onClientSizeChanged(object sender, EventArgs e)
        {
            arrangeFormsToDefaultView();
        }

        private void enableMenuItems()
        {
            ansichtToolStripMenuItem.Enabled = true;
            extrasToolStripMenuItem.Enabled = true;
        }

        private void disableMenuItems()
        {
            ansichtToolStripMenuItem.Enabled = false;
            extrasToolStripMenuItem.Enabled = false;
        }

        private void enableDateiSchließen()
        {
            dateiSchließenToolStripMenuItem.Enabled = true;
        }

        private void disableDateiSchließen()
        {
            dateiSchließenToolStripMenuItem.Enabled = false;
        }

        private void createNewListingForm()
        {
            listingForm = new ListingForm(file, pic);
            listingForm.MdiParent = this;
            listingForm.Show();
        }

        private void createNewControlForm() 
        {
            controlForm = new ControlForm(pic);
            controlForm.MdiParent = this;
            controlForm.Show();
        }

        private void createNewRegisterMapForm() 
        {
            registerMapForm = new RegisterMapForm(pic.getRegisterFileMap());
            registerMapForm.MdiParent = this;
            registerMapForm.Show();
        }

        private void createNewIOForm()
        {
            ioForm = new IOForm(pic.getRegisterFileMap());
            ioForm.MdiParent = this;
            ioForm.Show();
        }

        private void createNewSpecialValueForm()
        {
            specialForm = new SpecialValueForm(pic);
            specialForm.MdiParent = this;
            specialForm.Show();
        }

        private void createNewPulseGeneratorForm()
        {
            pulseGeneratorForm = new PulseGeneratorForm(pic);
            pulseGeneratorForm.MdiParent = this;
            pulseGeneratorForm.Show();
        }

        private void openHelpForm()
        {
            helpForm = new HelpForm();
            helpForm.Show();
        }

        private void toggleListingForm()
        {
            if (listingForm == null || listingForm.Visible == false)
            {
                createNewListingForm();
            }
            else
            {
                listingForm.Close();
            }
        }

        private void toggleControlForm()
        {
            if (controlForm == null || controlForm.Visible == false)
            {
                createNewControlForm();
            }
            else
            {
                controlForm.Close();
            }
        }

        private void toggleRegisterMapForm()
        {
            if (registerMapForm == null || registerMapForm.Visible == false)
            {
                createNewRegisterMapForm();
            }
            else
            {
                registerMapForm.Close();
            }
        }

        private void toggleIOForm()
        {
            if (ioForm == null || ioForm.Visible == false)
            {
                createNewIOForm();
            }
            else
            {
                ioForm.Close();
            }
        }

        private void toggleSpecialValueForm()
        {
            if (specialForm == null || specialForm.Visible == false)
            {
                createNewSpecialValueForm();
            }
            else
            {
                specialForm.Close();
            }
        }

        private void togglePulseGeneratorForm()
        {
            if (pulseGeneratorForm == null || pulseGeneratorForm.Visible == false)
            {
                createNewPulseGeneratorForm();
            }
            else
            {
                pulseGeneratorForm.Close();
            }
        }

        private void toggleHelpForm()
        {
            if (helpForm == null)
            {
                openHelpForm();
            }
            else
            {
                helpForm.BringToFront();
            }
        }

        private void showControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleControlForm();
        }

        private void showRegisterMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleRegisterMapForm();
        }

        private void listingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleListingForm();
        }

        private void closeAllOpenWindows()
        {
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }
        }

        /// <summary>
        /// Frees resources and closes all windows.
        /// </summary>
        private void freeResources() 
        {
            closeAllOpenWindows();
            if (pic != null)
            {
                pic.dispose();
            }
        }

        private void dateiSchließenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableMenuItems();
            disableDateiSchließen();
            closeAllOpenWindows();
            freeResources();
        }

        private void iOControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleIOForm();
        }

        private void specialValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleSpecialValueForm();
        }

        private void pulseGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            togglePulseGeneratorForm();
        }

        private void cOM3AusgabeAktivierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleCom3Label();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleHelpForm();
        }

        private void toggleCom3Label()
        {
            if (cOM3AusgabeAktivierenToolStripMenuItem.Text == COM3_LABEL_AKTIVIEREN_TEXT)
            {
                pic.startSerialization();

                cOM3AusgabeAktivierenToolStripMenuItem.Text = COM3_LABEL_DEAKTIVIEREN_TEXT;
                pic.beginContinuousSerialization();
            }
            else
            {
                cOM3AusgabeAktivierenToolStripMenuItem.Text = COM3_LABEL_AKTIVIEREN_TEXT;
                pic.endContinuousSerialization();
            }
        }
    }
}
