using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC
{
    public class PIC
    {
        private const short INTERVAL = 1000; // clock interval [ms]

        protected Register.RegisterFileMap registerMap = new Register.RegisterFileMap();
        protected Data.OperationStack operationStack = new Data.OperationStack();
        protected Register.ProgramCounter programCounter;
        protected short cyclesLeftToExecute = 1;
        protected Clock clock;

        private bool isReady = true;
        private Object isReadyLock = new Object();

        public PIC()
        {
            programCounter = new Register.ProgramCounter(registerMap);
            clock = new Clock(this, INTERVAL);
        }

        public void beginExecution()
        {
            clock.enableClock();
        }

        public void stopExecution()
        {
            clock.disableClock();
        }

        /// <summary>
        /// Executes the next (means: the operation referenced in programmCounter) operation
        /// </summary>
        protected bool executeNextOperation()
        {
            Operations.BaseOperation operation = new Operations.NopOperation(registerMap, programCounter.value); // to be replaced by parser call (fetchOperation(programCounter.value))
            cyclesLeftToExecute = operation.cycles;
            operation.execute();
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
            cyclesLeftToExecute--;

            lock (isReadyLock)
            {
                if (cyclesLeftToExecute <= 0 && isReady)
                {
                    isReady = false;
                    isReady = executeNextOperation();
                }
            }
        }
    }
}
