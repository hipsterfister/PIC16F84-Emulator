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
    public partial class RegisterMapForm : Form
    {
        protected PIC.Register.RegisterFileMap registerFileMap;
        protected const short NUMBER_OF_ELEMENTS = 256;

        public RegisterMapForm(PIC.Register.RegisterFileMap _registerFileMap)
        {
            InitializeComponent();
            registerFileMap = _registerFileMap;
            //RegisterItem test = new RegisterItem();
            //test.initRegisterItem(tAdapter, 10, 10, this);
           // test.Show();
            createMap();
        }

        private void createMap()
        {
            int xOffset = 20;
            int yOffset = 20;
            RegisterItem newRegisterItem;
            Label newLabel;
            for (int y = 0; y < NUMBER_OF_ELEMENTS; y += 0x10)
            {
                newLabel = new Label();
                newLabel.Parent = this;
                newLabel.Text = y.ToString("X2");
                newLabel.SetBounds(0, y * 2 + yOffset + 3, 21, 20);
                newLabel.Show();
                for (int x = 0; x < 16; x++)
                {
                    newRegisterItem = new RegisterItem();
                    newRegisterItem.initRegisterItem(registerFileMap.getAdapter(x + y), x * 25 + xOffset, y * 2 + yOffset, this);
                }
            }
            for (int x = 0; x < 16; x++)
            {
                newLabel = new Label();
                newLabel.Parent = this;
                newLabel.Text = x.ToString("X2");
                newLabel.SetBounds(x*25 + xOffset + 1, 0, 21, 20);
                newLabel.Show();
            }

        }


    }
}
