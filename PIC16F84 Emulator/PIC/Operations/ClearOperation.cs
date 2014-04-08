﻿using PIC16F84_Emulator.PIC.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Operations
{
    public class ClearOperation : BaseOperation
    {
        /*
         *  This OperationClass covers the following instructions:
         *      > CLRF
         *      > CLRW
         *      > CLRWDT
         *   Simply create a new instance with the corresponding address and call execute();
         */

        private short targetAddress;
        private const short CYCLES = 1;
        // private const short WDT_ADDRESS = 0x00; TODO: implement WDT

        public ClearOperation(short _targetAddress, RegisterFileMap _registerFileMap, short _address) :
            base(_registerFileMap, CYCLES, _address)
        {
            this.targetAddress = _targetAddress;
        }

        public override void execute()
        {
            registerFileMap.Set(0x00, targetAddress);
            
            registerFileMap.setZeroFlag();

            // Note: WDT is not part of the SFRs and has yet to be implemented
           // if (targetAddress == WDT_ADDRESS)
          //  {
           //     registerFileMap.setBit(RegisterConstants.STATUS_ADDRESS, 0x18); // set PD, TO
           // }

        }
    }
}
