using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace PIC16F84_Emulator.PIC
{
    public class Clock
    {
        /// <summary>
        /// This class reasembles the clock of the PIC. After the specified interval elapsed, the onCycleEnd() method is called.
        /// </summary>
        private static System.Timers.Timer clock;
        private PIC pic;

        public Clock(PIC _pic, short _interval)
        {
            clock = new System.Timers.Timer(_interval);
            clock.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            this.pic = _pic;
        }

        public void enableClock() {
            clock.Enabled = true;
        }

        public void disableClock() {
            clock.Enabled = false;
        }

        public void changeInterval(short _interval)
        {
            clock.Interval = _interval;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            pic.onCycleEnd();
        }
    }
}
