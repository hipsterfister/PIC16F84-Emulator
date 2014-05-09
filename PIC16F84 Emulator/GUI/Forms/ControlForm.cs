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
        protected PIC.PIC.PicExecutionState picExecutionState = PIC.PIC.PicExecutionState.STOPPED;
        protected System.ComponentModel.ComponentResourceManager resources = new ComponentResourceManager(typeof(ControlForm));
        private delegate void picReset();

        public ControlForm(PIC.PIC _pic)
        {
            InitializeComponent();
            this.pic = _pic;
            pic.registerExecutionStateListener(onPicExecutionChange);
            Disposed += delegate { pic.unregisterExecutionStateListener(onPicExecutionChange); };
        }

        /// <summary>
        /// default form position
        /// </summary>
        public void defaultView()
        {
            this.Location = new Point(MdiParent.ClientRectangle.Right - (this.Width + 5), 0);
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (picExecutionState == PIC.PIC.PicExecutionState.STOPPED)
            {
                pic.beginExecution();
            }
            else
            {
                pic.stopExecution();
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(new picReset(pic.resetPIC));
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            pic.executeSingleOperation();
        }

        private void onPicExecutionChange(PIC.PIC.PicExecutionState value, object sender)
        {
            MethodInvoker mi = delegate { changeState(value); };
            if (InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(mi); // Async to prohibit deadlock
                }
                catch (ObjectDisposedException)
                {
                    // ignore, the listener is not yet unregistered...
                }
            }
            else
            {
                changeState(value);
            }
        }


        private void changeState(PIC.PIC.PicExecutionState newState)
        {
            picExecutionState = newState;

            if (newState == PIC.PIC.PicExecutionState.STOPPED)
            {
                PlayButton.Image = (System.Drawing.Bitmap)(resources.GetObject("PlayButton.Image"));
            }
            else
            {
                PlayButton.Image = (System.Drawing.Bitmap)(resources.GetObject("PauseButton.Image"));
            }
        }

    }

}
