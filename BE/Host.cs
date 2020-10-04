using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Host
    {
        #region Properties
        public int ID { get; set; }
        public string password { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public int PhoneNumber { get; set; }
        public string MailAddress { get; set; }
        public BankBranch BankBranchDetails { get; set; }
        public int BankAccountNumber { get; set; }
        public bool CollectionClearance { get; set; }
        public int HostKey { get; set; }
        #endregion

        #region ToString Function
        public override string ToString()
        {
            return "ID: " + ID.ToString() + "\n"
                + "Password: " + password.ToString() + "\n"
                + "Private Name: " + PrivateName.ToString() + "\n"
                + "Family Name: " + FamilyName.ToString() + "\n"
                + "Phone Number: " + PhoneNumber.ToString() + "\n"
                + "Mail Address: " + MailAddress.ToString() + "\n"
                + "Bank Branch Details: " + BankBranchDetails.ToString() + "\n"
                + "Bank Account Number: " + BankAccountNumber.ToString() + "\n"
                + "Collection Clearance: " + CollectionClearance.ToString() + "\n"
                + "Host Key: " + HostKey.ToString() + "\n";
        }
        #endregion
    }
}
