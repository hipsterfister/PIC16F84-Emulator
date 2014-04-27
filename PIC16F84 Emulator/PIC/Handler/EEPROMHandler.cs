using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Handler
{
    public class EEPROMHandler
    {
        private Register.RegisterFileMap registerFileMap;
        private Data.EEPROMMemory eepromMemory;
        private Data.DataAdapter<byte>.OnDataChanged valueChangeListener;

        /// <summary>
        /// Crates a new EEPROM Handler to enable EEPROM functionality.
        /// IMPORTANT: call dispose(); when it's no longer needed.
        /// </summary>
        /// <param name="_registerFileMap"></param>
        /// <param name="_eepromMemory"></param>
        public EEPROMHandler(Register.RegisterFileMap _registerFileMap, Data.EEPROMMemory _eepromMemory)
        {
            this.registerFileMap = _registerFileMap;
            this.eepromMemory = _eepromMemory;
            this.valueChangeListener = new Data.DataAdapter<byte>.OnDataChanged(onValueChange);
            registerSelfWithRegisterFileMap();
        }

        public void dispose()
        {
            unregisterSelfWithRegisterFileMap();
        }

        public void onValueChange(byte value, object sender)
        {
            if (checkReadControlBit(value))
            {
                executeEEPROMRead();
            }
            if (checkWriteControlBit(value)) {
                executeEEPROMWrite();
            }
        }

        private bool checkReadControlBit(byte value)
        {
            if (value % 2 == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkWriteControlBit(byte value)
        {
            if ((value & 2) == 1 && (value & 4) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void executeEEPROMRead()
        {
            // TODO: Ensure that this operation is completed until the next cycle begins....
            short address = registerFileMap.Get(Register.RegisterConstants.EEADR_ADDRESS);
            byte value = eepromMemory[address];
            // Store EEPROM Value in Register
            registerFileMap.Set(value, Register.RegisterConstants.EEDATA_ADDRESS);
            // Clear Read Control Bit
            registerFileMap.clearBit(Register.RegisterConstants.EECON1_BANK1_ADDRESS, 1);
        }

        private void executeEEPROMWrite()
        {
            short address = registerFileMap.Get(Register.RegisterConstants.EEADR_ADDRESS);
            byte value = registerFileMap.Get(Register.RegisterConstants.EEDATA_ADDRESS);
            // update EEPROM
            eepromMemory[address] = value;
            // Clear Write Control Bit
            registerFileMap.clearBit(Register.RegisterConstants.EECON1_BANK1_ADDRESS, 2);
        }

        private void registerSelfWithRegisterFileMap()
        {
            registerFileMap.registerDataListener(valueChangeListener, Register.RegisterConstants.EECON1_BANK1_ADDRESS);
        }

        private void unregisterSelfWithRegisterFileMap()
        {
            registerFileMap.unregisterDataListener(valueChangeListener, Register.RegisterConstants.EECON1_BANK1_ADDRESS);
        }
    }
}
