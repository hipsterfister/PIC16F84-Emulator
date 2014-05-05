using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    public class DataAdapter<T>
    {
        public delegate void OnDataChanged(T Value, object Sender);
        public event OnDataChanged DataChanged;

        protected T _Data;

        public virtual T Value
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
                onDataChanged(value, this);
            }
        }

        protected void onDataChanged(T _value, object _sender)
        {
            if (DataChanged != null)
                DataChanged(_value, this);
        }
    }
}
