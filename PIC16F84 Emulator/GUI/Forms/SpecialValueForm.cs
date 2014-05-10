﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PIC16F84_Emulator.GUI.Forms
{
    public partial class SpecialValueForm : Form
    {
        private PIC.PIC pic;
        private const int TEXT_BOX_Y_OFFSET = 2;

        private TextBox[] stackBoxes;

        public SpecialValueForm(PIC.PIC _pic)
        {
            InitializeComponent();

            pic = _pic;
            stackBoxes = new TextBox[PIC.Data.OperationStack.STACK_SIZE];
            pic.getOperationStack().registerStackUpdateListener(onStackUpdate);

            Disposed += delegate { pic.getOperationStack().unregisterStackUpdateListener(onStackUpdate); };

            createValueDisplays();
        }

        private void createValueDisplays()
        {
            createWRegisterDisplay();
            createProgramCounterDisplay();
            createTimer0Display();
            createStackDisplay();
        }

        private void createWRegisterDisplay()
        {
            PIC.Data.DataAdapter<byte> wAdapter = pic.getRegisterFileMap().getAdapter(PIC.Register.RegisterConstants.WORKING_REGISTER_ADDRESS);
            RegisterItem item = new RegisterItem();
            item.initRegisterItem(wAdapter, 27, wRegisterLabel.Bounds.Y - TEXT_BOX_Y_OFFSET, this);
        }

        private void createProgramCounterDisplay()
        {
            PIC.Data.DataAdapter<byte> pcHighByteAdapter = pic.getProgramCounter().getHighByteAdapter();
            PIC.Data.DataAdapter<byte> pcLowByteAdapter = pic.getRegisterFileMap().getAdapter(PIC.Register.RegisterConstants.PCL_ADDRESS);
            
            // High Byte
            RegisterItem item = new RegisterItem();
            item.initRegisterItem(pcHighByteAdapter, 5, programCounterLabel.Bounds.Y - TEXT_BOX_Y_OFFSET, this);
            // Low Byte
            item = new RegisterItem();
            item.initRegisterItem(pcLowByteAdapter, 27 , programCounterLabel.Bounds.Y - TEXT_BOX_Y_OFFSET, this);
        }

        private void createTimer0Display()
        {
            PIC.Data.DataAdapter<byte> tmr0Adapter = pic.getRegisterFileMap().getAdapter(PIC.Register.RegisterConstants.TMR0_ADDRESS);
            RegisterItem item = new RegisterItem();
            item.initRegisterItem(tmr0Adapter, 27, timer0Label.Bounds.Y - TEXT_BOX_Y_OFFSET, this);
        }


        private void createStackDisplay()
        {
            TextBox box;

            for (int i = 0; i < PIC.Data.OperationStack.STACK_SIZE; i++)
            {
                box = new TextBox();
                box.SetBounds(5, stackIndex0Label.Bounds.Y - TEXT_BOX_Y_OFFSET + i * 20, 25, 25);
                box.Parent = this;
                box.ReadOnly = true;
                box.Show();
                stackBoxes[i] = box;
            }

            updateStackDisplay();
            //throw new NotImplementedException();
        }

        private void updateStackDisplay()
        {
            short[] stackArray = pic.getOperationStack().getStackAsSortedArray();
            TextBox box;

            for (int i = 0; i < PIC.Data.OperationStack.STACK_SIZE; i++)
            {
                box = stackBoxes[i];
                box.Text = stackArray[i].ToString("X3");
            }

        }

        private void onStackUpdate()
        {
            MethodInvoker mi = delegate { updateStackDisplay(); };
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
                updateStackDisplay();
            }
        }
    }
}
