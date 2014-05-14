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
        private Data.DataAdapter<byte>.OnDataChanged portBValueChangeListener;

        private byte oldPortBValue;
        private byte oldIntconValue;
        private byte oldEecon1Value;

        public InterruptHandler(PIC _pic, Register.RegisterFileMap _registerFileMap)
        {
            this.pic = _pic;
            this.registerFileMap = _registerFileMap;

            this.oldPortBValue = registerFileMap.Get(Register.RegisterConstants.PORTB_ADDRESS);
            this.oldIntconValue = registerFileMap.Get(Register.RegisterConstants.INTCON_ADDRESS);
            this.oldEecon1Value = registerFileMap.Get(Register.RegisterConstants.EECON1_BANK1_ADDRESS);

            this.valueChangeListener = new Data.DataAdapter<byte>.OnDataChanged(onValueChange);
            this.portBValueChangeListener = new Data.DataAdapter<byte>.OnDataChanged(onPortBValueChange);
            registerSelfWithRegisterFileMap();
        }

        public void dispose()
        {
            unregisterSelfWithRegisterFileMap();
        }

        private void onValueChange(byte value, object sender)
        {
            if (checkInterruptCondition())
            {
                pic.setInterruptIsNext(true); 
            } 
        }

        private void onPortBValueChange(byte value, object sender)
        {
            byte oldPortBBits4to7 = (byte)(oldPortBValue & 0xF0);
            byte newPortBBits4to7 = (byte)(value & 0xF0);
            bool oldRB0 = (oldPortBValue & 0x1) != 0;
            bool newRB0 = (value & 0x1) != 0;
            oldPortBValue = value;

            if ((oldPortBBits4to7 ^ (byte)(value & 0xF0)) != 0)
            {
                // At least one bit in PortB<4:7> changed
                registerFileMap.setBit(Register.RegisterConstants.INTCON_ADDRESS, Register.RegisterConstants.INTCON_RBIF_MASK);
            }
            else if (oldRB0 != newRB0)
            {
                // RB0/INT changed
                bool fallingEdgeActive = (registerFileMap.Get(Register.RegisterConstants.OPTION_REG_BANK1_ADDRESS) & Register.RegisterConstants.OPTION_INTEDG_MASK) == 0;
                if (newRB0 && !fallingEdgeActive)
                {
                    // Rising edge
                    registerFileMap.setBit(Register.RegisterConstants.INTCON_ADDRESS, Register.RegisterConstants.INTCON_INTF_MASK);
                }
                else if (!newRB0 && fallingEdgeActive)
                {
                    // Falling edge
                    registerFileMap.setBit(Register.RegisterConstants.INTCON_ADDRESS, Register.RegisterConstants.INTCON_INTF_MASK);
                }
            }
        }

        private bool checkInterruptCondition()
        {
            byte intconRegister = registerFileMap.Get(Register.RegisterConstants.INTCON_ADDRESS);
            // enable Flags
            bool gieFlag = intconRegister > 0x7F;
            bool rbieFlag = (intconRegister & Register.RegisterConstants.INTCON_RBIE_MASK) != 0; 
            bool inteFlag = (intconRegister & Register.RegisterConstants.INTCON_INTE_MASK) != 0;
            bool t0eFlag = (intconRegister & Register.RegisterConstants.INTCON_T0IE_MASK) != 0;
            bool eeieFlag = (intconRegister & Register.RegisterConstants.INTCON_EEIE_MASK) != 0;
            // interrupt Flags
            bool rbifFlag = (intconRegister & Register.RegisterConstants.INTCON_RBIF_MASK) != 0;
            bool intfFlag = (intconRegister & Register.RegisterConstants.INTCON_INTF_MASK) != 0;
            bool t0ifFlag = (intconRegister & Register.RegisterConstants.INTCON_T0IF_MASK) != 0;
            bool eeifFlag = (registerFileMap.Get(Register.RegisterConstants.EECON1_BANK1_ADDRESS) & 0x10) != 0;
            // oldInterruptFlags
            bool oldRbifFlag = (oldIntconValue & Register.RegisterConstants.INTCON_RBIF_MASK) != 0;
            bool oldIntfFlag = (oldIntconValue & Register.RegisterConstants.INTCON_INTF_MASK) != 0;
            bool oldT0ifFlag = (oldIntconValue & Register.RegisterConstants.INTCON_T0IF_MASK) != 0;
            bool oldEeifFlag = (oldEecon1Value & 0x10) != 0;

            oldEecon1Value = registerFileMap.Get(Register.RegisterConstants.EECON1_BANK1_ADDRESS);
            oldIntconValue = intconRegister;

            if (gieFlag)
            {
                //      PORTB INTERRUPT                  ||     TMR0 INTERRUPT                  ||    DATA EEPROM INTERRUPT
                if (rbieFlag && rbifFlag && !oldRbifFlag || t0eFlag && t0ifFlag && !oldT0ifFlag || eeieFlag && eeifFlag && !oldEeifFlag) {
                    return true;
                }
                // INT INTERRUPT
                if(inteFlag && intfFlag && !oldIntfFlag) {
                    // Check if sleeping
                    if ((registerFileMap.Get(Register.RegisterConstants.STATUS_ADDRESS) & 0x18) == 0x10)
                    {
                        invokeWakeUp();
                    }

                    return true;
                }
            }
            if (rbieFlag && rbifFlag && !oldRbifFlag || eeieFlag && eeifFlag && !oldEeifFlag || inteFlag && intfFlag && !oldIntfFlag)
            {
                invokeWakeUp();
            }
            return false;
        }

        protected void invokeWakeUp()
        {
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.DoWork += delegate {pic.wakeUpFromSleep();};
            worker.RunWorkerAsync();
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
            registerFileMap.registerDataListener(portBValueChangeListener, Register.RegisterConstants.PORTB_ADDRESS);
        }

        private void unregisterSelfWithRegisterFileMap()
        {
            registerFileMap.unregisterDataListener(valueChangeListener, Register.RegisterConstants.INTCON_ADDRESS);
            registerFileMap.unregisterDataListener(valueChangeListener, Register.RegisterConstants.EECON1_BANK1_ADDRESS);
            registerFileMap.unregisterDataListener(portBValueChangeListener, Register.RegisterConstants.PORTB_ADDRESS);
        }
    }
}
