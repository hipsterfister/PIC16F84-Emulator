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
        protected ListingForm listingForm;
        protected ControlForm controlForm;
        protected RegisterMapForm registerMapForm;
        protected IOForm ioForm;
        protected string file;

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
            freeResources();
            
            this.Activate();
            file = openFileDialog1.FileName;
             
            initNewPIC(file);
            enableAnsichtMenu();
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

            arrangeFormsToDefaultView();
        }

        private void arrangeFormsToDefaultView()
        {
            try
            {
                controlForm.defaultView();
                ioForm.defaultView(controlForm.Height);
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

        private void enableAnsichtMenu()
        {
            ansichtToolStripMenuItem.Enabled = true;
        }

        private void disableAnsichtMenu()
        {
            ansichtToolStripMenuItem.Enabled = false;
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
            disableAnsichtMenu();
            disableDateiSchließen();
            closeAllOpenWindows();
            freeResources();
        }

        private void iOControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleIOForm();
        }

    }
}
