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
        protected DataAdapter<byte> dataAdapter;
        protected DataAdapter<byte>.OnDataChanged dataChangeListener;

        public void initRegisterItem(DataAdapter<byte> _dataAdapter, int _positionX, int _positionY, System.Windows.Forms.Control _parent)
        {
            Parent = _parent;
            dataAdapter = _dataAdapter;
            this.Text = dataAdapter.Value.ToString("X2");
            // onChange listener
            dataChangeListener = new DataAdapter<byte>.OnDataChanged(updateValue);
            dataAdapter.DataChanged += dataChangeListener;
            // set default values
            this.SetBounds(_positionX, _positionY, WIDTH, HEIGHT);
            this.ReadOnly = true;
            this.MouseHover += showTooltip;
            this.Show();
        }

        public void updateValue(byte value, object sender)
        {
            this.Text = dataAdapter.Value.ToString("X2");
        }

        protected void showTooltip(object sender, System.EventArgs e)
        {
            ToolTip tt = new ToolTip();
            string ttText = "decimal value: " + this.dataAdapter.Value.ToString();
            tt.Show(ttText, this, 0, 18, 4000);
        } 
    }
}
