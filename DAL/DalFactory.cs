using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DalFactory
    {
        public static Idal instance;

        protected DalFactory() { instance = null; }
        public static Idal getDal()
        {
            if (instance==null)
                instance = new DAL_XML_imp();
            return instance;
        }
    }
}
