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
        protected PlayButtonState playButtonState = PlayButtonState.PLAY;
        protected System.ComponentModel.ComponentResourceManager resources = new ComponentResourceManager(typeof(ControlForm));
        private delegate void picReset();

        public ControlForm(PIC.PIC _pic)
        {
            InitializeComponent();

            this.pic = _pic;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            switch (playButtonState)
            {
                case PlayButtonState.PLAY:
                    pic.beginExecution();
                    changeState(PlayButtonState.PAUSE);
                    break;
                case PlayButtonState.PAUSE:
                    pic.stopExecution();
                    changeState(PlayButtonState.PLAY);
                    break;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(new picReset(pic.resetPIC));
            changeState(PlayButtonState.PLAY);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            pic.onCycleEnd();
        }

        private void changeState(PlayButtonState newState)
        {
            switch(newState) {
              case PlayButtonState.PLAY:
                    playButtonState = PlayButtonState.PLAY;
                    PlayButton.Image = (System.Drawing.Bitmap)(resources.GetObject("PlayButton.Image"));
                    break;
                case PlayButtonState.PAUSE:
                    playButtonState = PlayButtonState.PAUSE;
                    PlayButton.Image = (System.Drawing.Bitmap)(resources.GetObject("PauseButton.Image"));
                    break;
            }
        }

        protected enum PlayButtonState
        {
            PLAY,
            PAUSE
        }
    }

}
