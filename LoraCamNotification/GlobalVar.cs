using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoraCamNotification
{
    class GlobalVar
    {
        private static Serial_Data _Serial_Data;
        private static Boolean _OK = false;
        private static Boolean _DataOK = false;
        private static Controller _Control;

        public static Controller Control
        {
            get { return _Control; }
            set { _Control = value; }
        }

        public static Serial_Data Serial_Data
        {
            get { return _Serial_Data; }
            set { _Serial_Data = value; }
        }

        public static Boolean OK
        {
            get { return _OK; }
            set { _OK = value; }
        }

        public static Boolean DataOK
        {
            get { return _DataOK; }
            set { _DataOK = value; }
        }
    }
}
