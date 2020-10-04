using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public static class Configuration
    {
        public static int expireRequestDays = 30;
        public static int fee = 10;
        public static int guestRequestKey = 10000000;
        public static int orderKey = 10000000;
        public static int hostingUnitKey = 10000000;
        public static string managerPassword = "1234";
    }
}
