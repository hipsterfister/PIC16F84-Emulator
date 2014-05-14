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
        protected const short ELEMENT_HEIGHT = 20;
        protected const short ELEMENT_WIDTH = 21;
        protected const short ELEMENT_MARING = 4;
        protected const short MAP_LINE_HEIGHT = 2;
        protected const short MAP_X_OFFSET = 25;
        protected const short MAP_Y_OFFSET = 60;
        protected const short TEXT_BOX_INCREASED_WIDTH = 1;
        protected const short TEXT_BOX_INCREASED_HEIGHT = 3;

        public RegisterMapForm(PIC.Register.RegisterFileMap _registerFileMap)
        {
            InitializeComponent();
            registerFileMap = _registerFileMap;
            createMap();
            createSpecialValueView();
        }

        /// <summary>
        /// default form position
        /// </summary>
        /// <param name="_right">control-form left position</param>
        public void defaultView(int _right)
        {
            this.Location = new Point(_right - this.Width, 0);
            this.Height = MdiParent.ClientRectangle.Height - 30;
        }

        private void createSpecialValueView()
        {
            createNewLabel(5, 5 + TEXT_BOX_INCREASED_HEIGHT, 65, 20, "W-Register:");
            RegisterItem newRegisterItem = new RegisterItem(registerFileMap.getAdapter(PIC.Register.RegisterConstants.WORKING_REGISTER_ADDRESS), 75, 5, this);
        }

        private void createMap()
        {
            RegisterItem newRegisterItem;
            for (int y = 0; y < NUMBER_OF_ELEMENTS; y += 0x10)
            {
                createNewLabel(MAP_X_OFFSET - ELEMENT_WIDTH, y * 2 + MAP_Y_OFFSET + TEXT_BOX_INCREASED_HEIGHT, ELEMENT_WIDTH, ELEMENT_HEIGHT, y.ToString("X2"));
                for (int x = 0; x < 16; x++)
                {
                    newRegisterItem = new RegisterItem(registerFileMap.getAdapter(x + y), x * (ELEMENT_WIDTH + ELEMENT_MARING) + MAP_X_OFFSET, y * MAP_LINE_HEIGHT + MAP_Y_OFFSET, this);
                }
            }
            for (int x = 0; x < 16; x++)
            {
                createNewLabel(x * (ELEMENT_WIDTH + ELEMENT_MARING) + TEXT_BOX_INCREASED_WIDTH + MAP_X_OFFSET, MAP_Y_OFFSET - ELEMENT_HEIGHT, ELEMENT_WIDTH, ELEMENT_HEIGHT, x.ToString("X2"));
            }

        }

        private void createNewLabel(int x, int y, int width, int height, string text)
        {
            Label newLabel = new Label();
            newLabel.Parent = this;
            newLabel.Text = text;
            newLabel.SetBounds(x, y, width, height);
            newLabel.Show();
        }


    }
}
