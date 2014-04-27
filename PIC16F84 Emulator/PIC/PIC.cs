using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC
{
    public class PIC
    {
        private const short INTERVAL = 50; // clock interval [ms]

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

        private bool isReady = true;
        private Object isReadyLock = new Object();

        private bool interruptIsNext = false;

        public delegate void OnCycleEnd();
        public event OnCycleEnd cycleEnded;
        public delegate void OnExecutionOfNextInstruction(short address);
        public event OnExecutionOfNextInstruction nextInstructionEvent;

        public PIC()
        {
            programCounter = new Register.ProgramCounter(registerMap);
            clock = new Clock(this, INTERVAL);
            interruptHandler = new Handler.InterruptHandler(this, registerMap);
            parser = new Parser.Parser(this);
            timer0 = new Timer0.Timer0(registerMap, this);
            eepromHandler = new Handler.EEPROMHandler(registerMap, eeprom);
        }

        public void resetPIC()
        {
            lock (isReadyLock)
            {
                isReady = false;
                clock.disableClock();
                registerMap.initializeValues();
                programCounter.initializeValue();
                eeprom.initializeValues();
                operationStack.initializeValues();
                cyclesLeftToExecute = 1;
                interruptIsNext = false;
                isReady = true;
            }
        }

        /// <summary>
        /// This needs to be called to unsubscribe events.
        /// </summary>
        public void dispose()
        {
            clock.dispose();
            timer0.dispose();
            interruptHandler.dispose();
            eepromHandler.dispose();
        }

    /*    ~PIC()
        {
            while (cycleEnded != null)
            {
                cycleEnded -= new OnCycleEnd(endOfCylceDummy);
            }
            while (nextInstructionEvent != null)
            {
                nextInstructionEvent -= new OnExecutionOfNextInstruction(nextInstructionDummy);
            }
        } */

        public void beginExecution()
        {
            clock.enableClock();
        }

        public void stopExecution()
        {
            clock.disableClock();
        }

        public void executeSingleOperation()
        {
            if (!clock.isEnabled())
            {
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
            if (nextInstructionEvent != null)
                nextInstructionEvent(programCounter.value);
            if (interruptIsNext)
            {
                // This approach was chosen to prevent bugs from modifying the programmCounter simultaneously (e.g. executing CallOperation & onInterrupt-Event)
                interruptHandler.triggerInterrupt(operationStack, programCounter);
            }
            Operations.BaseOperation operation = parser.getNextOperation(programCounter.value);
            Console.WriteLine(operation.GetType());
            programCounter.value += 1;
            operation.execute();
            cyclesLeftToExecute = operation.cycles;
            return true;
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

        // TODO: there must be a better way... come on.
        private static void endOfCylceDummy()
        {

        } 

        private static void nextInstructionDummy(short a) 
        {

        }
    }
}
