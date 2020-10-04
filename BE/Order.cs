using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        #region Properties
        public int HostingUnitKey { get; set; }
        public int GuestRequestKey { get; set; }
        public int OrderKey { get; set; }
        public int Fee { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
    
    #endregion

    #region ToString Function
    public override string ToString()
        {
            return
                "Hosting Unit Key: " + HostingUnitKey.ToString() + "\n"
                + "Guest Request Key: " + GuestRequestKey.ToString() + "\n"
                + "Order Key: " + OrderKey.ToString() + "\n"
                + "Fee: " + Fee.ToString() + "\n"
                + "Status: " + Status.ToString() + "\n"
                + "Create Date: " + CreateDate.ToString() + "\n";
                //+ "Order Date: " + OrderDate.ToString() + "\n";
        }
        #endregion
    }
}
