using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BankBranch
    {
        
        public string BankName { get; set; }

        public int BankNumber { get; set; }
        public int BranchNumber { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }
        public override string ToString()
        {
            return
                "Bank Name: " + BankName.ToString() + "\n"
                + "Bank Number: " + BankNumber.ToString() + "\n"
                + "Branch Number: " + BranchNumber.ToString() + "\n"
                + "Branch Address: " + BranchAddress.ToString() + "\n"
                + "Branch City: " + BranchCity.ToString() + "\n";
        }

    }
}
