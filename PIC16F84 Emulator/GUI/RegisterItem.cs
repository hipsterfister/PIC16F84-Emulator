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
        protected const short ACTIVE_DURATION = 1500; //[ms]
        protected byte value;
        protected Helpers.UpdateTimer updateTimer;

        protected static System.Drawing.Color passiveColor = System.Drawing.SystemColors.Control;
        protected static System.Drawing.Color activeColor = System.Drawing.Color.DeepSkyBlue;

        public void initRegisterItem(DataAdapter<byte> _dataAdapter, int _positionX, int _positionY, System.Windows.Forms.Control _parent)
        {
            Parent = _parent;
            this.Text = _dataAdapter.Value.ToString("X2");
            // onChange listener
            _dataAdapter.DataChanged += onValueChange;
            Disposed += delegate { _dataAdapter.DataChanged -= onValueChange; };
            // set default values
            this.SetBounds(_positionX, _positionY, WIDTH, HEIGHT);
            this.ReadOnly = true;
            this.MouseHover += showTooltip;
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
                    this.Invoke(mi);
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
            value = _value;
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
                    this.Invoke(mi);
                }
                catch (ObjectDisposedException)
                {
                    // Just ignore it. The UI Element is allready disposed, nothing left to update.
                }
            }
        }


        protected void showTooltip(object sender, System.EventArgs e)
        {
            ToolTip tt = new ToolTip();
            string ttText = "decimal value: " + this.value.ToString();
            tt.Show(ttText, this, 0, 18, 4000);
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
