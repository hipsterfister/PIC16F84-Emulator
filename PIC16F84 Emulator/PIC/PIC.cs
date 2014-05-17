using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC
{
    public class PIC
    {
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

        public PIC()
        {
            programCounter = new Register.ProgramCounter(registerMap);
            clock = new Clock(this, interval);
            interruptHandler = new Handler.InterruptHandler(this, registerMap);
            parser = new Parser.Parser(this);
            timer0 = new Timer0.Timer0(registerMap, this);
            eepromHandler = new Handler.EEPROMHandler(registerMap, eeprom);
            wdt = new WatchDog.WDT(this);

            executionStatus.Value = PicExecutionState.STOPPED;
            executedCycles.Value = 0;

            frequencyValue.Value = 4;
            frequencyUnit.Value = FrequencyUnit.MEGA_HZ;

            frequencyUnit.DataChanged += onFrequencyUnitChange;
            frequencyValue.DataChanged += onFrequencyValueChange;
        }

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
        /// This needs to be called to unsubscribe events.
        /// </summary>
        public void dispose()
        {
            endContinuousSerialization();
            endSerialization();

            frequencyUnit.DataChanged -= onFrequencyUnitChange;
            frequencyValue.DataChanged -= onFrequencyValueChange;

            clock.dispose();
            timer0.dispose();
            interruptHandler.dispose();
            eepromHandler.dispose();
            programCounter.dispose();
            registerMap.dispose();
            wdt.dispose();
        }

        public void beginExecution()
        {
            executionStatus.Value = PicExecutionState.RUNNING;
            resumeAfterBreakpoint = true;
            clock.enableClock();
            wdt.start();
        }


        public void stopExecution()
        {
            executionStatus.Value = PicExecutionState.STOPPED;
            clock.disableClock();
            wdt.stop();
        }

        public void executeSingleOperation()
        {
            if (!clock.isEnabled())
            {
                resumeAfterBreakpoint = true;
                onCycleEnd();
            }
        }

        public void initProgramMemory(string _filePath)
        {
            programMemory.readFile(_filePath);
        }

        /// <summary>
        /// Executes the next (means: the operation referenced in programmCounter) operation
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
        /// Thread safe!
        /// </summary>
        public void onCycleEnd()
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
        public void setInterruptIsNext(bool _value)
        {
            interruptIsNext = _value;
        }

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

        /// <summary>
        /// Returns the PIC's instruction cycle duration
        /// Note: The value does not factor in user's execution speed, so this value could appear lower then the actual needed execution time.
        /// </summary>
        /// <returns>value in ms</returns>
        public short getCycleDuration()
        {
            return interval;
        }


        // TODO: there must be a better way... come on.
        private static void endOfCylceDummy()
        {

        } 

        private static void nextInstructionDummy(short a) 
        {

        }

        public void registerExecutionStateListener(Data.DataAdapter<PicExecutionState>.OnDataChanged listener)
        {
            executionStatus.DataChanged += listener;
        }

        public void unregisterExecutionStateListener(Data.DataAdapter<PicExecutionState>.OnDataChanged listener)
        {
            executionStatus.DataChanged -= listener;
        }

        public void resetWDT()
        {
            this.wdt.reset();
        }

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

        public void setFrequencyValue(float value)
        {
            frequencyValue.Value = value;
        }

        public void setFrequencyUnit(FrequencyUnit unit)
        {
            frequencyUnit.Value = unit;
        }

        public void registerExecutedCyclesListener(Data.DataAdapter<int>.OnDataChanged listener)
        {
            executedCycles.DataChanged += listener;
        }

        public void unregisterExecutedCyclesListener(Data.DataAdapter<int>.OnDataChanged listener)
        {
            executedCycles.DataChanged -= listener;
        }

        private void onFrequencyValueChange(float value, object sender)
        {
            updateFrequency();
        }

        private void onFrequencyUnitChange(FrequencyUnit value, object sender)
        {
            updateFrequency();
        }

        private void updateFrequency()
        {
            if (frequencyValue.Value <= 0)
            {
                return;
            }

            double newInterval = 0;
            int power = 0;

            switch (frequencyUnit.Value)
            {
                case FrequencyUnit.HZ:
                    power = 1;
                    break;
                case FrequencyUnit.KILO_HZ:
                    power = 1000;
                    break;
                case FrequencyUnit.MEGA_HZ:
                    power = 1000000;
                    break;
                default:
                    // impossible
                    break;
            }

            newInterval = 1 / (frequencyValue.Value * power * 4);
            newInterval *= MS_FACTOR;
            if (newInterval < 1)
            {
                newInterval = 1;
            }

            interval = (short) newInterval;
            clock.changeInterval((short) newInterval);
        }

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
    }
}