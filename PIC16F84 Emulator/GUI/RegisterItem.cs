using PIC16F84_Emulator.PIC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Register;

namespace PIC16F84_Emulator.GUI
{
    public class RegisterItem : System.Windows.Forms.TextBox
    {
        protected const short WIDTH = 20;
        protected const short HEIGHT = 40;
        protected const short ACTIVE_DURATION = 1500; //[ms]
        protected byte value;
        protected Helpers.UpdateTimer updateTimer;

        protected static System.Drawing.Color passiveColor = System.Drawing.SystemColors.Control;
      //  protected static System.Drawing.Color activeColor = System.Drawing.Color.DeepSkyBlue;
        protected static System.Drawing.Color activeColor = System.Drawing.Color.OrangeRed;

        DataAdapter<byte> dataAdapter;

        ToolTip tt;
        TextBox hexBox;

        public void initRegisterItem(DataAdapter<byte> _dataAdapter, int _positionX, int _positionY, System.Windows.Forms.Control _parent)
        {
            Parent = _parent;
            // this.BackColor = backColor;
            // this.ForeColor = foreColor;
            
            this.Text = _dataAdapter.Value.ToString("X2");
            this.dataAdapter = _dataAdapter;
            this.value = dataAdapter.Value;
            // onChange listener
            dataAdapter.DataChanged += onValueChange;
            Disposed += delegate { dataAdapter.DataChanged -= onValueChange; };
            // set default values
            this.SetBounds(_positionX, _positionY, WIDTH, HEIGHT);
            this.ReadOnly = true;
            this.MouseEnter += showTooltip;
            this.MouseLeave += hideTooltip;
            this.DoubleClick += createNewHexBox;
            this.Show();
            this.updateTimer = new Helpers.UpdateTimer(ACTIVE_DURATION, new System.Timers.ElapsedEventHandler(onTimerExpiredHandler));
        }

        public void onValueChange(byte _value, object sender)
        {
            MethodInvoker mi = delegate { updateValue(_value); };
            if (InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(mi); // Async to prohibit deadlock
                }
                catch (ObjectDisposedException)
                {
                    // nothing to do here.
                }
            }
            else
            {
                updateValue(_value);
            }
            
        }

        private void updateValue(byte _value)
        {
            this.value = _value;
            this.Text = value.ToString("X2");
            makeThisActive();
        }

        private void makeThisActive()
        {
            this.BackColor = activeColor;
            this.updateTimer.resetTimer();
        }

        private void makeThisPassive()
        {
            this.BackColor = passiveColor;
        }

        public void onTimerExpiredHandler(object o, System.Timers.ElapsedEventArgs e)
        {
            MethodInvoker mi = delegate { makeThisPassive(); };
            if (InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(mi); // Async to prohibit deadlock
                }
                catch (ObjectDisposedException)
                {
                    // Just ignore it. The UI Element is allready disposed, nothing left to update.
                }
            }
        }


        protected void showTooltip(object sender, System.EventArgs e)
        {
            this.tt = new ToolTip();
            string ttText = "decimal value: " + this.value.ToString();
            tt.Show(ttText, this, 0, 18);
        }

        protected void hideTooltip(object sender, System.EventArgs e)
        {
            tt.Dispose();
        }

        protected void createNewHexBox(object sender, System.EventArgs e)
        {
            hexBox = new TextBox();
            hexBox.Text = value.ToString("X2");
            hexBox.MaxLength = 2;
            hexBox.Parent = this;
            hexBox.KeyDown += updateHexValue;
            hexBox.Show();
            Parent.MouseClick += clickBesideHexBox;
        }

        private void updateHexValue(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    this.dataAdapter.Value = checked((byte)(Convert.ToInt16(hexBox.Text, 16)));
                }
                catch
                {
                    hexBox.Clear();
                    return;
                }
                disposeHexBox();
            }
        }

        private void disposeHexBox()
        {
            hexBox.Dispose();
        }

        private void clickBesideHexBox(object sender, MouseEventArgs e)
        {
            disposeHexBox();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RegisterItem
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ResumeLayout(false);

        } 
    }
}
