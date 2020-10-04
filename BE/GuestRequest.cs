using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GuestRequest

    {
        #region Properties
        public int GuestRequestKey { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public area Area { get; set; }
        public type Type { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public Preference Pool { get; set; }
        public Preference Jacuzzi { get; set; }
        public Preference Garden { get; set; }
        public bool Beach { get; set; }
        public Preference ChildrenAttractions { get; set; }
        public int minPrice { get; set; }
        public int maxPrice { get; set; }
        #endregion

        #region ToString Function
        public override string ToString()
        {
            return "Guest Request Key: " + GuestRequestKey.ToString() + "\n"
                + "Private Name: " + PrivateName.ToString() + "\n"
                + "Family Name: " + FamilyName.ToString() + "\n"
                + "Mail Address: " + MailAddress.ToString() + "\n"
                + "Request's status: " + Status.ToString() + "\n"
                + "Registration Date: " + RegistrationDate.ToString() + "\n"
                + "Entry Date: " + EntryDate.ToString() + "\n"
                + "Release Date: " + ReleaseDate.ToString() + "\n"
                + "Request Area: " + Area.ToString() + "\n"
                + "Request Type: " + Type.ToString() + "\n"
                + "Adults: " + Adults.ToString() + "\n"
                + "Children: " + Children.ToString() + "\n"
                + "Pool: " + Pool.ToString() + "\n"
                + "Jacuzzi " + Jacuzzi.ToString() + "\n"
                + "Garden: " + Garden.ToString() + "\n"
                + "Beach: " + Beach.ToString() + "\n"
                + "Attractions: " + ChildrenAttractions.ToString() + "\n"
                + "Minimum price: " + minPrice.ToString() + "\n"
                + "Maximum price: " + maxPrice.ToString() + "\n";
            
        }
        #endregion

    }
}
