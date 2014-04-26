using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Handler;

namespace PIC16F84_Emulator.PIC.Timer0
{
    public class Timer0
    {
        /// <summary>
        /// Represents and encapsulates TMR0
        /// Responsible for:
        ///     > providing a comfortable interface
        ///     > prescaler calculation
        ///     > triggering TMR0 interrupts
        ///     > general functionality
        /// Timer0Handler is responsible for:
        ///     > triggering "ticks"
        ///     > any event handling
        ///     (Note that Timer0Handler is a member of Timer0)
        /// </summary>
         
        // TODO: When the prescaler is assigned to the TMR0 writing to the TMR0 register will clear the prescaler.

        protected Data.DataAdapter<byte> tmr0Register;
        protected Data.DataAdapter<byte> optionRegister;
        protected Register.RegisterFileMap registerFileMap;
        protected short internalCounter;
        protected Handler.Timer0Handler timerHandler;

        public Timer0(Register.RegisterFileMap _registerFileMap, PIC _pic)
        {
            tmr0Register = _registerFileMap.getAdapter(Register.RegisterConstants.TMR0_ADDRESS);
            optionRegister = _registerFileMap.getAdapter(Register.RegisterConstants.OPTION_REG_BANK1_ADDRESS);
            registerFileMap = _registerFileMap;
            internalCounter = 0;
            timerHandler = new Timer0Handler(_registerFileMap, this, _pic);
        }

        /// <summary>
        /// Encapsulates the TMR0's state. Use this to get / set the state using an enum.
        /// Option-Register's value is synchronized.
        /// </summary>
        public Timer0State timerState
        {
            get
            {
                if((optionRegister.Value & Register.RegisterConstants.OPTION_T0CS_MASK) == 0)
                    return Timer0State.TIMER;
                return Timer0State.COUNTER;
            }
            set
            {
                if (value == Timer0State.TIMER)
                {
                    registerFileMap.clearBit(Register.RegisterConstants.OPTION_REG_BANK1_ADDRESS, Register.RegisterConstants.OPTION_T0CS_MASK);
                }
                else
                {
                    registerFileMap.setBit(Register.RegisterConstants.OPTION_REG_BANK1_ADDRESS, Register.RegisterConstants.OPTION_T0CS_MASK);
                }
            }
        }

        /// <summary>
        /// Encapsulates the TMR0's value.
        /// TMR0-Register's value is synchronized.
        /// Do NOT use this to increment the timers value, as the prescaler is not beeing factored in.
        /// </summary>
        public byte tmr0Value
        {
            get
            {
                return tmr0Register.Value;
            }
            set
            {
                tmr0Register.Value = value;
            }
        }

        /// <summary>
        /// Encapsulates the prescaler Assignment
        /// Option-Register's value is synchronized.
        /// </summary>
        public bool prescalerIsAssigned
        {
            get
            {
                return ((optionRegister.Value & Register.RegisterConstants.OPTION_PSA_MASK) == 0); // assigned to TMR0 when Bit is cleared
            }
            set
            {
                if (value)
                {
                    // assign to WDT (set bit -> assign to wdt)
                    registerFileMap.setBit(Register.RegisterConstants.OPTION_REG_BANK1_ADDRESS, Register.RegisterConstants.OPTION_PSA_MASK);
                }
                else
                {
                    // assign to TMR0 (clear bit -> assign to tmr0)
                    registerFileMap.clearBit(Register.RegisterConstants.OPTION_REG_BANK1_ADDRESS, Register.RegisterConstants.OPTION_PSA_MASK);
                }
            }
        }

        /// <summary>
        /// Encapsulates the Edge Selection Bit for Timer0.
        /// Option Register's Value is synchronized.
        /// </summary>
        public Timer0SelectedEdge edgeSelectionMode
        {
            get
            {
                if ((optionRegister.Value & Register.RegisterConstants.OPTION_T0SE_MASK) == 0)
                    return Timer0SelectedEdge.RISING; // cleared -> rising edge
                return Timer0SelectedEdge.FALLING;
            }
            set
            {
                if (value == Timer0SelectedEdge.FALLING)
                {
                    registerFileMap.setBit(Register.RegisterConstants.OPTION_REG_BANK1_ADDRESS, Register.RegisterConstants.OPTION_T0SE_MASK);
                }
                else // value == Timer0SelectedEdge.RISING
                {
                    registerFileMap.clearBit(Register.RegisterConstants.OPTION_REG_BANK1_ADDRESS, Register.RegisterConstants.OPTION_T0SE_MASK);
                }
            }
        }

        /// <summary>
        /// Encapsulates the prescalers Value. Does not account for the assignment of the prescaler!
        /// Option-Register's value is synchronized.
        /// </summary>
        public byte prescalerValue
        {
            get
            {
                return (byte) (optionRegister.Value & 0x07);
            }
            set
            {
                int arg1 = optionRegister.Value & 0xF8;         // xxxx.xxxx & 1111.1000 = xxxx.x000
                int arg2 = value & 0x07;                        // yyyy.yyyy & 0000.0111 = 0000.0yyy
                optionRegister.Value = (byte)(arg1 | arg2);     // xxxx.x000 | 0000.0yyy = xxxx.xyyy #profit

                internalCounter = 0;
            }
        }

        /// <summary>
        /// Triggers a TMR0 tick, therefor the timer0 value is incremented (with respect of the prescaler)
        /// </summary>
        private void tick()
        {
            internalCounter++;

            if (prescalerIsAssigned)
            {
                if (internalCounter >= prescalerValue)
                {
                    internalCounter = 0;
                    incrementTimer0();
                }
            }
            else
            {
                internalCounter = 0;
                incrementTimer0();
            }
        }

        /// <summary>
        /// Trigger a TMR0 tick, caused by a Counter-Mode event. (edge on RA4/T0CKI)
        /// Does nothing when TMR0 is in Timer-Mode.
        /// </summary>
        /// <param name="edge">rising / falling</param>
        public void triggerCounterTick(Timer0SelectedEdge edge)
        {
            if ((timerState == Timer0State.COUNTER) && (edge == edgeSelectionMode))
                tick();
        }

        /// <summary>
        /// Trigger a TMR0 tick, caused by a Timer-Mode event.
        /// Does nothing when TMR0 is in Counter-Mode.
        /// </summary>
        public void triggerTimerTick()
        {
            if (timerState == Timer0State.TIMER)
                tick();
        }

        /// <summary>
        /// Blocks the next two timer ticks IF (and only if) TMR0 is in Timer-Mode.
        /// </summary>
        public void inhibitTwoCycles()
        {
            if (timerState == Timer0State.TIMER)
                internalCounter -= 2;
        }

        /// <summary>
        /// Resets the prescaler IF (and only if) the prescaler is assigned to TMR0.
        /// </summary>
        public void resetPrescaler()
        {
            // TODO: evaluate: Muss der Prescaler oder der internal Counter resettet werden?
            if (prescalerIsAssigned)
            {
                prescalerValue = 0;
            }
        }


        /// <summary>
        /// Increments the Timer and triggers TMR0 Interrupts on Timer overflow
        /// </summary>
        private void incrementTimer0()
        {
            if (tmr0Value == 0xFF)
            {
                registerFileMap.setBit(Register.RegisterConstants.INTCON_ADDRESS, Register.RegisterConstants.INTCON_T0IF_MASK);
                tmr0Value = 0;
            }
            else
            {
                tmr0Value = (byte) ( tmr0Value + 1 );
            }
        }

    }

    public enum Timer0State
    {
        TIMER,
        COUNTER
    }

    public enum Timer0SelectedEdge
    {
        FALLING,
        RISING
    }
}
