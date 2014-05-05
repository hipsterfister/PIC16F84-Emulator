using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Handler
{
    public class Timer0Handler
    {
        private Register.RegisterFileMap registerFileMap;
        private Timer0.Timer0 timer0;
        private byte lastPortAValue;
        private bool tmr0RegisterChangeEventIsSuppressed = false;
        private PIC pic;

        // Listeners
        private Data.DataAdapter<byte>.OnDataChanged portAValueChangeListener;
        private Data.DataAdapter<byte>.OnDataChanged tmr0RegisterChangeListener;
        private PIC.OnCycleEnd cycleEndListener;


        public Timer0Handler(Register.RegisterFileMap _registerFileMap, Timer0.Timer0 _timer0, PIC _pic)
        {
            pic = _pic;
            this.registerFileMap = _registerFileMap;
            this.timer0 = _timer0;
            lastPortAValue = _registerFileMap.Get(Register.RegisterConstants.PORTA_ADDRESS);

            // instance delegates
            this.portAValueChangeListener = new Data.DataAdapter<byte>.OnDataChanged(onPortAValueChange);
            this.tmr0RegisterChangeListener = new Data.DataAdapter<byte>.OnDataChanged(onTMR0RegisterChanged);
            this.cycleEndListener = new PIC.OnCycleEnd(onCycleEnd);

            registerDelegates(_pic);
        }

        public void dispose()
        {
            unregisterDelegates();
        }

        public void onPortAValueChange(byte value, object sender)
        {
            if (((value ^ lastPortAValue) & 0x10) != 0) // xxxx.xxxx XOR yyyy.yyyy = cccc.cccc; cccc.cccc & 0001.0000 = 000c.0000 (c read as: bit changed)
            {
                tmr0RegisterChangeEventIsSuppressed = true;
                if ((value & 0x10) == 0) // RA4/T0CKI 1 -> 0 => falling edge
                {
                    timer0.triggerCounterTick(Timer0.Timer0SelectedEdge.FALLING);
                }
                else
                {
                    timer0.triggerCounterTick(Timer0.Timer0SelectedEdge.FALLING);
                }
                tmr0RegisterChangeEventIsSuppressed = false;
            }
        }

        public void onCycleEnd()
        {
            tmr0RegisterChangeEventIsSuppressed = true;
            timer0.triggerTimerTick();
            tmr0RegisterChangeEventIsSuppressed = false;
        }

        /// <summary>
        /// Triggers the neccessary Timer0 methods on a change of TMR0 register's value.
        /// This event is suppressed when a regular write to TMR0 register occurs.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sender"></param>
        public void onTMR0RegisterChanged(byte value, object sender)
        {
            if (!tmr0RegisterChangeEventIsSuppressed)
            {
                // These methods will only have effect if their conditions are met.
                timer0.inhibitTwoCycles();
                timer0.resetPrescaler();
            }
        }


        /// <summary>
        /// Registers the Listeners with the corresponding Observables
        /// </summary>
        private void registerDelegates()
        {
            registerFileMap.registerDataListener(tmr0RegisterChangeListener, Register.RegisterConstants.TMR0_ADDRESS);
            registerFileMap.registerDataListener(portAValueChangeListener, Register.RegisterConstants.PORTA_ADDRESS);
            pic.cycleEnded += onCycleEnd;
        }
        
        private void unregisterDelegates() 
        {
            registerFileMap.unregisterDataListener(tmr0RegisterChangeListener, Register.RegisterConstants.TMR0_ADDRESS);
            registerFileMap.unregisterDataListener(onPortAValueChange, Register.RegisterConstants.PORTA_ADDRESS);
            pic.cycleEnded -= onCycleEnd;
        }
    }
}
