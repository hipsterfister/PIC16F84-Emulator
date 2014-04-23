using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Handler
{
    public class InterruptHandler
    {
        private PIC pic;
        private Register.RegisterFileMap registerFileMap;
        private Data.DataAdapter<byte>.OnDataChanged valueChangeListener;

        public InterruptHandler(PIC _pic, Register.RegisterFileMap _registerFileMap)
        {
            this.pic = _pic;
            this.registerFileMap = _registerFileMap;
            this.valueChangeListener = new Data.DataAdapter<byte>.OnDataChanged(onValueChange);
            registerSelfWithRegisterFileMap();
        }

        ~InterruptHandler()
        {
            registerFileMap.unregisterDataListener(valueChangeListener, Register.RegisterConstants.INTCON_ADDRESS);
            registerFileMap.unregisterDataListener(valueChangeListener, Register.RegisterConstants.EECON1_BANK1_ADDRESS);
        }

        private void onValueChange(byte value, object sender)
        {
            if (checkInterruptCondition())
            {
                pic.setInterruptIsNext(true); 
            } 
        }

        private bool checkInterruptCondition()
        {
            short intconRegister = registerFileMap.Get(Register.RegisterConstants.INTCON_ADDRESS);
            // enable Flags
            bool gieFlag = intconRegister > 0x7F;
            bool rbieFlag = (intconRegister & 0x08) != 0;
            bool inteFlag = (intconRegister & 0x10) != 0;
            bool t0eFlag = (intconRegister & 0x20) != 0;
            bool eeieFlag = (intconRegister & 0x40) != 0;
            // interrupt Flags
            bool rbifFlag = (intconRegister & 0x01) != 0;
            bool intfFlag = (intconRegister & 0x02) != 0;
            bool t0ifFlag = (intconRegister & 0x04) != 0;
            bool eeifFlag = (registerFileMap.Get(Register.RegisterConstants.EECON1_BANK1_ADDRESS) & 0x10) != 0;

            if (gieFlag)
            {
                //      PORTB INTERRUPT  ||     TMR0 INTERRUPT  || DATA EEPROM INTERRUPT
                if (rbieFlag && rbifFlag || t0eFlag && t0ifFlag || eeieFlag && eeifFlag) {
                    return true;
                }
                // INT INTERRUPT
                if(inteFlag && intfFlag) {
                    // Check if sleeping
                    if ((registerFileMap.Get(Register.RegisterConstants.STATUS_ADDRESS) & 0x18) == 0x10)
                    {
                        // TODO: Wake Up auslagern!
                        // -> Wake up!
                        registerFileMap.setBit(Register.RegisterConstants.STATUS_ADDRESS, 0x18);
                        pic.beginExecution();
                    }

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Pushes the current value of our programmCounter to the operationStack and sets programmCounter to INTERRUPT_VECTOR_ADDRESS
        /// </summary>
        public void triggerInterrupt(Data.OperationStack operationStack, Register.ProgramCounter programCounter)
        {
            operationStack.push(programCounter.value);
            programCounter.value = Register.RegisterConstants.INTERRUPT_VECTOR_ADDRESS;
            pic.setInterruptIsNext(false);
        }

        private void registerSelfWithRegisterFileMap()
        {
            registerFileMap.registerDataListener(valueChangeListener, Register.RegisterConstants.INTCON_ADDRESS);
            registerFileMap.registerDataListener(valueChangeListener, Register.RegisterConstants.EECON1_BANK1_ADDRESS);
        }
    }
}
