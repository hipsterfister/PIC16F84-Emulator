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
        protected PIC.Data.ProgamMemory programMemory;

        protected static System.Drawing.Color breakpointColor = System.Drawing.Color.Orange;
        protected static System.Drawing.Color defaultColor = System.Drawing.Color.White;
        protected static System.Drawing.Color defaultSelectionColor = System.Drawing.SystemColors.Highlight;

        protected int numberOfLinesDisplayed;

        public ListingForm(string _pathToFile, PIC.PIC _pic)
        {
            InitializeComponent();

            programView = new GUI.ProgramView(_pathToFile);

            foreach(string item in programView.source) {
                dataGridView1.Rows.Add(item);
            }

            _pic.nextInstructionEvent += onNextInstructionExecution;
            Disposed += delegate { _pic.nextInstructionEvent -= onNextInstructionExecution;  };

            programMemory = _pic.getProgramMemory();

            numberOfLinesDisplayed = dataGridView1.Height / dataGridView1.RowTemplate.Height;
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
            dataGridView1.Rows[line].Selected = true;
            // is the current line not visible?
            if (dataGridView1.FirstDisplayedScrollingRowIndex < line - numberOfLinesDisplayed || dataGridView1.FirstDisplayedScrollingRowIndex > line + numberOfLinesDisplayed)
            dataGridView1.FirstDisplayedScrollingRowIndex = line - 5;
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

        private void listingBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            toggleBreakpoint();
        }

        private void toggleBreakpoint()
        {
            int index = dataGridView1.SelectedRows[0].Index;
            short address = programView.getAddressByLine(index);
            
            if (address == ProgramView.NO_ADDRESS_VALUE)
            {
                // not an instruction
                return;
            }
            bool isSet = programMemory.toggleBreakpoint(address);

            if (isSet)
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    item.DefaultCellStyle.BackColor = breakpointColor;
                    item.DefaultCellStyle.SelectionBackColor = breakpointColor;
                }
            }
            else
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    item.DefaultCellStyle.BackColor = defaultColor;
                    item.DefaultCellStyle.SelectionBackColor = defaultSelectionColor;
                }
            }

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            toggleBreakpoint();
        }

    }
}
