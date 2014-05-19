using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC
{
    public class PIC
    {
        // ######################################################
        // Members
        // ######################################################
        #region member declarations
        private short MS_FACTOR = 1000;

        private short interval = 4; // clock interval [ms]

        protected Data.ProgamMemory programMemory = new Data.ProgamMemory();
        protected Register.RegisterFileMap registerMap = new Register.RegisterFileMap();
        protected Data.EEPROMMemory eeprom = new Data.EEPROMMemory();
        protected Handler.EEPROMHandler eepromHandler;
        protected Data.OperationStack operationStack = new Data.OperationStack();
        protected Register.ProgramCounter programCounter;
        protected Handler.InterruptHandler interruptHandler;
        protected short cyclesLeftToExecute = 1;
        protected Clock clock;
        protected Parser.Parser parser;
        protected Timer0.Timer0 timer0;
        protected WatchDog.WDT wdt;
        internal Ports.PortSerialization portSerializer;

        protected bool isReady = true;
        protected Object isReadyLock = new Object();

        protected bool interruptIsNext = false;
        protected bool resumeAfterBreakpoint = false;
        protected bool serialPortIsOpen = false;

        protected Data.DataAdapter<int> executedCycles = new Data.DataAdapter<int>();
        protected Data.DataAdapter<float> frequencyValue = new Data.DataAdapter<float>();
        protected Data.DataAdapter<FrequencyUnit> frequencyUnit = new Data.DataAdapter<FrequencyUnit>();
        protected Data.DataAdapter<PicExecutionState> executionStatus = new Data.DataAdapter<PicExecutionState>();

        public delegate void OnCycleEnd();
        public event OnCycleEnd cycleEnded;
        public delegate void OnExecutionOfNextInstruction(short address);
        public event OnExecutionOfNextInstruction nextInstructionEvent;
        #endregion

        // ######################################################
        // Constructor & Internals
        // ######################################################
        #region Constructor & Internals
        public PIC()
        {
            programCounter      = new Register.ProgramCounter(registerMap);
            clock               = new Clock(this, interval);
            interruptHandler    = new Handler.InterruptHandler(this, registerMap);
            parser              = new Parser.Parser(this);
            timer0              = new Timer0.Timer0(registerMap, this);
            eepromHandler       = new Handler.EEPROMHandler(registerMap, eeprom);
            wdt                 = new WatchDog.WDT(this);

            executionStatus.Value   = PicExecutionState.STOPPED;
            executedCycles.Value    = 0;

            frequencyValue.Value    = 4;
            frequencyUnit.Value     = FrequencyUnit.MEGA_HZ;

            frequencyUnit.DataChanged   += onFrequencyUnitChange;
            frequencyValue.DataChanged  += onFrequencyValueChange;
        }

        /// <summary>
        /// This needs to be called to unsubscribe events.
        /// </summary>
        public void dispose()
        {
            endContinuousSerialization();
            endSerialization();

            frequencyUnit.DataChanged   -= onFrequencyUnitChange;
            frequencyValue.DataChanged  -= onFrequencyValueChange;

            clock.dispose();
            timer0.dispose();
            interruptHandler.dispose();
            eepromHandler.dispose();
            programCounter.dispose();
            registerMap.dispose();
            wdt.dispose();
        }

        /// <summary>
        /// Loads the program into the PIC's program memory
        /// </summary>
        /// <param name="_filePath"></param>
        public void initProgramMemory(string _filePath)
        {
            programMemory.readFile(_filePath);
        }
        #endregion

        // ######################################################
        // PIC API
        // ######################################################
        #region PIC API
        /// <summary>
        /// On call the PIC will perform a RESET
        /// </summary>
        public void resetPIC()
        {
            lock (isReadyLock)
            {
                if (isReady)
                {
                    isReady = false;
                    stopExecution();

                    endContinuousSerialization();
                    endSerialization();

                    registerMap.initializeValues();
                    programCounter.initializeValue();
                    eeprom.initializeValues();
                    operationStack.initializeValues();
                    cyclesLeftToExecute = 1;
                    executedCycles.Value = 0;
                    interruptIsNext = false;
                    isReady = true;
                }
            }
        }
        /// <summary>
        /// Starts the program execution
        /// </summary>
        public void beginExecution()
        {
            executionStatus.Value = PicExecutionState.RUNNING;
            resumeAfterBreakpoint = true;
            clock.enableClock();
            wdt.start();
        }
        /// <summary>
        /// Halts the program execution
        /// </summary>
        public void stopExecution()
        {
            executionStatus.Value = PicExecutionState.STOPPED;
            clock.disableClock();
            wdt.stop();
        }
        /// <summary>
        /// Executes the next cycle only if the execution is not running
        /// </summary>
        public void executeSingleOperation()
        {
            if (!clock.isEnabled())
            {
                resumeAfterBreakpoint = true;
                onCycleEnd();
            }
        }
        #endregion

        // ######################################################
        // Cycle Execution
        // ######################################################
        #region Cycle Execution
        /// <summary>
        /// Executes the next (means: the operation referenced in programmCounter) operation
        /// Performs Breakpoint and Interrupt checks
        /// 
        /// Not thread safe!
        /// </summary>
        protected bool executeNextOperation()
        {
            
            if (interruptIsNext)
            {
                // This approach was chosen to prevent bugs from modifying the programmCounter simultaneously (e.g. executing CallOperation & onInterrupt-Event)
                interruptHandler.triggerInterrupt(operationStack, programCounter);
            }

            if (programMemory.isBreakpoint(programCounter.value) && !resumeAfterBreakpoint)
            {
                stopExecution();
                return true;
            }
            


            execute();
            
            return true;
        }

        /// <summary>
        /// Executes the next operation without any checks
        /// 
        /// Fires the nextInstructionEvent
        /// </summary>
        protected void execute()
        {
            Operations.BaseOperation operation = parser.getNextOperation(programCounter.value);
            operation.execute();
            programCounter.increment();

            cyclesLeftToExecute = operation.cycles;
            executedCycles.Value += cyclesLeftToExecute;

            if (nextInstructionEvent != null)
                nextInstructionEvent(programCounter.value);
        }

        /// <summary>
        /// To be called after a cycle completed successfully. 
        /// Calls the next operation and therefore starts a new cycle.
        /// 
        /// A cycle completed successfully after
        ///     a) the operation was executed AND the cycle time elapsed (for 2-cycle-instructions: 2 x cycle time)
        ///     b) the operation was executed AND the next cycle was started by the debuger
        /// 
        /// Fires the cycleEnded Event
        /// 
        /// Thread safe!
        /// </summary>
        internal void onCycleEnd()
        {
            lock (isReadyLock)
            {
                cyclesLeftToExecute--;

                if (cycleEnded != null)
                    cycleEnded();

                if (cyclesLeftToExecute <= 0 && isReady)
                {
                    isReady = false;
                    isReady = executeNextOperation();
                }
            }
            resumeAfterBreakpoint = false;
        }

        /// <summary>
        /// Sets/Clears the interruptIsNext-Flag
        /// </summary>
        /// <param name="_value"></param>
        internal void setInterruptIsNext(bool _value)
        {
            interruptIsNext = _value;
        }
        #endregion

        // ######################################################
        // Getter Section
        // ######################################################
        #region Getters
        /// <summary>
        /// Returns the PIC's registerFileMap
        /// </summary>
        /// <returns>RegisterFileMap</returns>
        public Register.RegisterFileMap getRegisterFileMap()
        {
            return registerMap;
        }

        /// <summary>
        /// Returns the PIC's programMemory
        /// </summary>
        /// <returns>ProgamMemory</returns>
        public Data.ProgamMemory getProgramMemory()
        {
            return programMemory;
        }

        /// <summary>
        /// Returns the PIC's programCounter (PC)
        /// </summary>
        /// <returns>ProgramCounter</returns>
        public Register.ProgramCounter getProgramCounter()
        {
            return programCounter;
        }

        /// <summary>
        /// Returns the PIC's operationStack (Stack)
        /// </summary>
        /// <returns>OperationStack</returns>
        public Data.OperationStack getOperationStack()
        {
            return operationStack;
        }

        #endregion

        // ######################################################
        // Serialization
        // ######################################################
        #region Serialization
        /// <summary>
        /// Opens the serial port. Call this before starting continuous or manual serialization.
        /// When the port is no longer needed close it with endSerialization.
        /// 
        /// Note: Nothing happens when COM3 is not available on the user's machine.
        /// </summary>
        public void startSerialization()
        {
            if (this.portSerializer == null)
            {
                portSerializer = new Ports.PortSerialization(registerMap);
            }

            serialPortIsOpen = portSerializer.openPort();
        }

        /// <summary>
        /// Closes the serial port.
        /// </summary>
        public void endSerialization()
        {
            if (this.portSerializer != null)
            {
                portSerializer.closePort();
            }
        }

        /// <summary>
        /// Writes the current Port and Tris values on COM3
        /// </summary>
        public void serialize()
        {
            portSerializer.send();
        }

        /// <summary>
        /// Writes the current Port and Tris values to COM3 whenever one of these values was changed.
        /// </summary>
        public void beginContinuousSerialization()
        {
            if (serialPortIsOpen)
            {
                portSerializer.startSerialization();
            }
            else
            {
                startSerialization();
                portSerializer.startSerialization();
            }
        }

        /// <summary>
        /// Stops the continuous serialization
        /// </summary>
        public void endContinuousSerialization()
        {
            if (serialPortIsOpen)
            {
                portSerializer.endSerialization();
            }
        }
#endregion

        // ######################################################
        // Wake Up & Watch Dog
        // ######################################################
        #region WakeUp & WatchDog
        /// <summary>
        /// Wakes the PIC and restarts execution
        /// </summary>
        public void wakeUpFromSleep()
        {
            if ((registerMap.Get(Register.RegisterConstants.STATUS_ADDRESS) & 0x18) == 0x10)
            {
                // Device was sleeping
                registerMap.setBit(Register.RegisterConstants.STATUS_ADDRESS, 0x18);
                // The operation following SLEEP is prefetched during SLEEP execution.
                // => the next operation will be executed immediately on wake up.
                execute();
                beginExecution();
            }
        }

        /// <summary>
        /// Resets the WDT
        /// </summary>
        public void resetWDT()
        {
            this.wdt.reset();
        }
        #endregion
      
        // ######################################################
        // Listener API
        // ######################################################
        #region ListenerAPI

        // Listener API for the executed cycles counter.

        public void registerExecutedCyclesListener(Data.DataAdapter<int>.OnDataChanged listener)
        {
            executedCycles.DataChanged += listener;
        }

        public void unregisterExecutedCyclesListener(Data.DataAdapter<int>.OnDataChanged listener)
        {
            executedCycles.DataChanged -= listener;
        }

        // Listener API for Execution State

        public void registerExecutionStateListener(Data.DataAdapter<PicExecutionState>.OnDataChanged listener)
        {
            executionStatus.DataChanged += listener;
        }

        public void unregisterExecutionStateListener(Data.DataAdapter<PicExecutionState>.OnDataChanged listener)
        {
            executionStatus.DataChanged -= listener;
        }

        #endregion 

        // ######################################################
        // Frequency & Cycle Duration
        // ######################################################
        #region Frequency & Cycle Duration
        public void setFrequencyValue(float value)
        {
            frequencyValue.Value = value;
        }

        public void setFrequencyUnit(FrequencyUnit unit)
        {
            frequencyUnit.Value = unit;
        }

        private void onFrequencyValueChange(float value, object sender)
        {
            updateFrequency();
        }

        private void onFrequencyUnitChange(FrequencyUnit value, object sender)
        {
            updateFrequency();
        }


        /// <summary>
        /// Updates the PIC's frequency. Please note that the real Frequency is divided by 1000. This results in simulating 1µs with 1ms real time.
        /// </summary>
        private void updateFrequency()
        {
            if (frequencyValue.Value <= 0)
            {
                return;
            }

            double newInterval = 0;
            double power = 0;

            switch (frequencyUnit.Value)
            {
                case FrequencyUnit.HZ:
                    power = 0.001;
                    break;
                case FrequencyUnit.KILO_HZ:
                    power = 1;
                    break;
                case FrequencyUnit.MEGA_HZ:
                    power = 1000;
                    break;
                default:
                    // impossible
                    break;
            }

            newInterval = 4 / (frequencyValue.Value * power);
            newInterval *= MS_FACTOR;
            if (newInterval < 1)
            {
                newInterval = 1;
            }

            interval = (short) newInterval;
            clock.changeInterval((short) newInterval);
        }

        /// <summary>
        /// Returns the PIC's instruction cycle duration
        /// Note: The value does not factor in user's execution speed, so this value could appear lower then the actual needed execution time.
        /// </summary>
        /// <returns>value in ms</returns>
        public short getCycleDuration()
        {
            return interval;
        }

        #endregion

        // ######################################################
        // Enumerations & Dummies
        // ######################################################
        #region Enumerations & Dummies
        public enum PicExecutionState
        {
            RUNNING,
            STOPPED
        }

        public enum FrequencyUnit
        {
            HZ,
            KILO_HZ,
            MEGA_HZ
        }

        private static void endOfCylceDummy()
        {

        }

        private static void nextInstructionDummy(short a)
        {

        }
        #endregion

    }
}