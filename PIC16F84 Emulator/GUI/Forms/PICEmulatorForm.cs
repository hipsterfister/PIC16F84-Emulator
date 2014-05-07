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
            createNewControlForm(true);
            createNewIOForm(true);
            createNewRegisterMapForm(true);
            createNewListingForm(true);
        }

        private void rearrangeFormsToDefaultView()
        {
            controlForm.defaultView();
            ioForm.defaultView(controlForm.Height);
            registerMapForm.defaultView(controlForm.Bounds.Left);
            listingForm.defaultView(registerMapForm.Bounds.Left);
        }

        private void PICEmulatorForm_onClientSizeChanged(object sender, EventArgs e)
        {
            rearrangeFormsToDefaultView();
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

        private void createNewListingForm(bool _defaultView)
        {
            listingForm = new ListingForm(file, pic);
            listingForm.MdiParent = this;
            listingForm.Show();
            if (_defaultView)
                listingForm.defaultView(registerMapForm.Bounds.Left);
        }

        private void createNewControlForm(bool _defaultView) 
        {
            controlForm = new ControlForm(pic);
            controlForm.MdiParent = this;
            controlForm.Show();
            if (_defaultView)
                controlForm.defaultView();
        }
        private void createNewRegisterMapForm(bool _defaultView) 
        {
            registerMapForm = new RegisterMapForm(pic.getRegisterFileMap());
            registerMapForm.MdiParent = this;
            registerMapForm.Show();
            if (_defaultView)
                registerMapForm.defaultView(controlForm.Bounds.Left);
        }
        private void createNewIOForm(bool _defaultView)
        {
            ioForm = new IOForm(pic.getRegisterFileMap());
            ioForm.MdiParent = this;
            ioForm.Show();
            if (_defaultView)
                ioForm.defaultView(controlForm.Height);
        }

        private void toggleListingForm()
        {
            if (listingForm == null || listingForm.Visible == false)
            {
                createNewListingForm(false);
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
                createNewControlForm(false);
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
                createNewRegisterMapForm(false);
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
                createNewIOForm(false);
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
