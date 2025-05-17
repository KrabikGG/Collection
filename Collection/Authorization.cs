using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection
{
    public class Authorization
    {
        public static int logUser
        {
            get;
            set;
        }
        public int LogCheck(string logText, string pswText)
        {
            logUser = 0;
            if ((logText == "Ivan") && (pswText == "krytoi"))
            {
                logUser = 2;
            }
            return logUser;
        }
    }
}
