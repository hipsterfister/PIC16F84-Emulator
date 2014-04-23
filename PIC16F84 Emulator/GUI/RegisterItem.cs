using PIC16F84_Emulator.PIC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PIC16F84_Emulator.GUI
{
    public class RegisterItem : System.Windows.Forms.TextBox
    {
        protected const short WIDTH = 20;
        protected const short HEIGHT = 40;
        protected byte value;

        public void initRegisterItem(DataAdapter<byte> _dataAdapter, int _positionX, int _positionY, System.Windows.Forms.Control _parent)
        {
            Parent = _parent;
            this.Text = _dataAdapter.Value.ToString("X2");
            // onChange listener
            _dataAdapter.DataChanged += onValueChange;
            // set default values
            this.SetBounds(_positionX, _positionY, WIDTH, HEIGHT);
            this.ReadOnly = true;
            this.MouseHover += showTooltip;
            this.Show();
        }

        public void onValueChange(byte _value, object sender)
        {
            MethodInvoker mi = delegate { updateValue(_value); };
            if (InvokeRequired)
            {
                this.Invoke(mi);
            }
            
        }

        private void updateValue(byte _value)
        {
            value = _value;
            this.Text = value.ToString("X2");
        }

        protected void showTooltip(object sender, System.EventArgs e)
        {
            ToolTip tt = new ToolTip();
            string ttText = "decimal value: " + this.value.ToString();
            tt.Show(ttText, this, 0, 18, 4000);
        } 
    }
}
