using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.GUI.Helpers
{
    public class UpdateTimer
    {
        private System.Timers.Timer timer;

        public UpdateTimer(int _interval, System.Timers.ElapsedEventHandler handler)
        {
            timer = new System.Timers.Timer(_interval);
            timer.Elapsed += handler;
            timer.AutoReset = false;
        }

        public void resetTimer()
        {
            timer.Stop();
            timer.Start();
        }

        public void startTimer()
        {
            timer.Start();
        }

    }
}
