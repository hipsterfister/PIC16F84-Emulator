using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Register
{
    public class ProgramCounter
    {
        /// <summary>
        /// This class encapsulates the program counter which is split into the registers PCL and PCLATH.
        /// </summary>
        private RegisterFileMap registerFileMap;
        private Data.DataAdapter<byte> upperBits;
        private bool isOwnChange = false;
        private bool wasModified = false;
        private Data.DataAdapter<byte>.OnDataChanged onPclChangeListener;

        public ProgramCounter(RegisterFileMap _registerFileMap)
        {
            this.registerFileMap = _registerFileMap;
            this.upperBits = new Data.DataAdapter<byte>();
            onPclChangeListener = new Data.DataAdapter<byte>.OnDataChanged(onPclChange);
            registerFileMap.registerDataListener(onPclChangeListener, RegisterConstants.PCL_ADDRESS);
            initializeValue();
        }

        public void initializeValue()
        {
            this.value = 0x00;
            this.upperBits.Value = 0x00;
            this.isOwnChange = false;
        }

        public void dispose()
        {
            registerFileMap.unregisterDataListener(onPclChangeListener, RegisterConstants.PCL_ADDRESS);
        }

        public short value
        {
            get
            {
                return (short)(upperBits.Value * 0x100 + registerFileMap.Get(RegisterConstants.PCL_ADDRESS));
            }
            set
            {
                isOwnChange = true;
                registerFileMap.Set((byte)(value % 0x100), RegisterConstants.PCL_ADDRESS);
                upperBits.Value = (byte)(value / 0x100);
            }
        }

        public void increment()
        {
            if (!wasModified)
            {
                this.value = (short) (value + 1);
            }
            wasModified = false;
        }

        private void onPclChange(byte v, object o)
        {
            if (isOwnChange)
            {
                // An dieser Stelle wird sichergestellt, dass der PC am Zyklusende nicht hochgezählt wird, wenn er durch GOTO o.Ä. modifiziert wurde.
                wasModified = true;
            }
            else
            {
                upperBits.Value = registerFileMap.Get(RegisterConstants.PCLATH_ADDRESS);
            }
            isOwnChange = false;
        }

        internal Data.DataAdapter<byte> getHighByteAdapter()
        {
            return upperBits;
        }
    }
}
