using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.WatchDog
{

    public class WDT
    {
        private System.Timers.Timer wdtTimer;
        private PIC pic;
        private Data.DataAdapter<byte> optionRegister;
        private const short WDT_MINIMAL_INTERVAL = 18; // [ms]
        private const double PIC_REAL_CYCLE_DURATION = 0.2; // [µs]
        

        public WDT(PIC _pic)
        {
            this.pic = _pic;

            optionRegister = pic.getRegisterFileMap().getAdapter(Register.RegisterConstants.OPTION_REG_BANK1_ADDRESS);
            optionRegister.DataChanged += onOptionRegisterChange;

            wdtTimer = new System.Timers.Timer();
            wdtTimer.Interval = calculateWdtInterval();
            wdtTimer.Elapsed += onWdtTimerElapsed;
            wdtTimer.AutoReset = false;
        }

        void onOptionRegisterChange(byte Value, object Sender)
        {
            wdtTimer.Interval = calculateWdtInterval();
        }

        /// <summary>
        /// Starts the WDT. (resumes after it was stopped)
        /// </summary>
        public void start()
        {
            wdtTimer.Start();
        }

        /// <summary>
        /// Stops the WDT without resetting it.
        /// </summary>
        public void stop()
        {
            wdtTimer.Stop();
        }

        /// <summary>
        /// Called whenever a WDT timeout occurs.
        /// Calls PIC's reset method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onWdtTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            pic.resetPIC();
            pic.beginExecution();
        }

        /// <summary>
        /// Checks whether the Postscaler is assigned (PSA-bit == 1)
        /// </summary>
        /// <returns></returns>
        private bool postScalerIsAssigned()
        {
            return ((optionRegister.Value & Register.RegisterConstants.OPTION_PSA_MASK) != 0);
        }

        /// <summary>
        /// Returns the Postscalers current value
        /// </summary>
        /// <returns></returns>
        private short getPostScalerValue()
        {
            int power = (optionRegister.Value & 0x07);
            return (short)(Math.Pow(2, power));
        }

        /// <summary>
        /// Returns the WDT's timeout interval length [ms]
        /// </summary>
        /// <returns>int [ms]</returns>
        private double calculateWdtInterval()
        {
            // instructionCycle = 200ns = 0,2 µs = A0
            // simulatedInstructionCycle = 1ms = A1
            // baselineTimeout after 18ms 
            //                       18ms / 0,2µs = 90.000 cycles
            // simulatedBaseTO after 18ms * (A1 / A0)

            // TODO: get actual values
            double picSimulatedCycleDuration = pic.getCycleDuration() * 1000; // [µs]
            double instructionDurationRatio = picSimulatedCycleDuration / PIC_REAL_CYCLE_DURATION;

            double interval = WDT_MINIMAL_INTERVAL * instructionDurationRatio; // [ms]

            if (postScalerIsAssigned())
            {
                interval *= getPostScalerValue();
            }

            return interval;
        }
    }
}
