using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    public class EEPROMMemory
    {
        private DataAdapter<byte>[] eeprom;
        private const short EEPROM_MEMORY_SIZE = 64;

        public EEPROMMemory()
        {
            eeprom = new DataAdapter<byte>[EEPROM_MEMORY_SIZE];
            for (int i = 0; i < EEPROM_MEMORY_SIZE; i++)
            {
                eeprom[i] = new DataAdapter<byte>();
            }
        }

        public byte this[short address]
        {
            get
            {
                if (address >= EEPROM_MEMORY_SIZE || address < 0)
                {
                    return 0;
                }
                return eeprom[address].Value;
            }
            set
            {
                if (address >= EEPROM_MEMORY_SIZE || address < 0)
                {
                    return;
                }
                eeprom[address].Value = value;
            }
        }
    }
}
