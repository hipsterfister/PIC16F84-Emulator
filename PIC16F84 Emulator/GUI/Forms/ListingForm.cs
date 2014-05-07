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
    public partial class ListingForm : Form
    {
        protected GUI.ProgramView programView;

        public ListingForm(string _pathToFile, PIC.PIC _pic)
        {
            InitializeComponent();

            programView = new GUI.ProgramView(_pathToFile);
            listingBox.DataSource = programView.source;
            _pic.nextInstructionEvent += onNextInstructionExecution;
            Disposed += delegate { _pic.nextInstructionEvent -= onNextInstructionExecution;  };
        }

        /// <summary>
        /// default form position
        /// </summary>
        /// <param name="_right">registermap-form left position</param>
        public void defaultView(int _right)
        {
            this.Location = new Point(0, 0);
            this.Height = MdiParent.ClientRectangle.Height - 30;
            this.Width = _right;
        }

        public void changeCursor(short _instructionAddress) {
            int line = programView.getLineByAddress(_instructionAddress);
            listingBox.SelectedIndex = line;
        }

        public void onNextInstructionExecution(short _instructionAddress)
        {
            MethodInvoker mi = delegate { changeCursor(_instructionAddress); };
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
                changeCursor(_instructionAddress);
            }
        }
    }
}
