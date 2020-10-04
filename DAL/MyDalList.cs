using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    /// <summary>
    /// This class does only the simple things, just like adds, updates and deletings
    /// It also checks if something exists and changes configs in all running programs at once
    /// </summary>
    class MyDalList 
    {
        //-----------------------------------------------------------------

        public void addClientRequest(GuestRequest gr)
        {

            if (DataSource.guestRequestList.Exists(item => item.GuestRequestKey == gr.GuestRequestKey) == true)
                throw new ArgumentException("This request already exists");
            else
            {
                DataSource.guestRequestList.Add(gr.Clone());
                Configuration.guestRequestKey++;
            }
        }

        //-----------------------------------------------------------------

        public void updateHostingUnit(HostingUnit hu)
        {
            int x = DataSource.hostingUnitList.FindIndex(item => item.HostingUnitKey == hu.HostingUnitKey);
            if (x == -1)//אין כזה הוסטינגיוניטקי ברשימה
            {
                throw new KeyNotFoundException("There is no such a hosting unit");
            }
            DataSource.hostingUnitList[x] = hu;
        }

        //-----------------------------------------------------------------

        public void updateOrder(Order o)
        {
            int index = DataSource.orderList.FindIndex(item => item.OrderKey == o.OrderKey);
            if (index == -1)
            {
                throw new KeyNotFoundException("There is no such an order");
            }
            o.Status =DataSource.orderList[index].Status ;
        }

        //-----------------------------------------------------------------

        public void updateClientRequestStatus(GuestRequest gr)
        {
            int x = DataSource.guestRequestList.FindIndex(item => item.GuestRequestKey == gr.GuestRequestKey);
            if ( x!= -1)
            {
                gr.Status = DataSource.guestRequestList[x].Status;
                return;
            }
            throw new KeyNotFoundException("There is no such a request");
        }

        //-----------------------------------------------------------------

        public void addHostingUnit(HostingUnit hu)
        {
            if (DataSource.hostingUnitList.Exists(item => item.HostingUnitKey == hu.HostingUnitKey) == true)
                throw new ArgumentException("This hosting unit already exists");
            else
            {
                DataSource.hostingUnitList.Add(hu.Clone());
                Configuration.hostingUnitKey++;
            }
        }

        //-----------------------------------------------------------------

        public void deleteHostingUnit(HostingUnit hu)
        {
            if (DataSource.hostingUnitList.Exists(item => item.HostingUnitKey == hu.HostingUnitKey) == true)
            {
                DataSource.hostingUnitList.Remove(hu);
                hu.HostingUnitKey = 0;
                return;
            }
            throw new KeyNotFoundException("There is no such a hosting unit");
        }

        
        //-----------------------------------------------------------------

        public void addOrder(Order o)
        {
            if (DataSource.orderList.Exists(item => item.OrderKey == o.OrderKey) == true)
                throw new ArgumentException("This order already exists");
            else
            {
                DataSource.orderList.Add(o.Clone());
                Configuration.orderKey++;
            }
        }

        //-----------------------------------------------------------------

        public List<HostingUnit> getHostingUnits()
        {
            return DataSource.hostingUnitList;
        }

        //-----------------------------------------------------------------

        public List<GuestRequest> getGuestRequests()
        {
            return DataSource.guestRequestList;
        }

        //-----------------------------------------------------------------

        public List<Order> getOrders()
        {
            return DataSource.orderList;
        }

        //-----------------------------------------------------------------

        public List<BankBranch> GetBankBranches() 
        {
            IEnumerable<BankBranch> arr = new BankBranch[5];
            List<BankBranch> bankBranchesList = new List<BankBranch>();
            foreach (var item in Enumerable.Repeat(arr, 5)); //create 5 bankBranches in the array
            foreach (var item in arr) bankBranchesList.Add(item.Clone());
            return bankBranchesList;
        }

        //-----------------------------------------------------------------
        public HostingUnit FindUnit(int num)
        {
            int index = DataSource.hostingUnitList.FindIndex(item => item.HostingUnitKey == num);
            if (index == -1)
            {
                throw new ArgumentException("There is no such a hosting unit");//
            }
            return getHostingUnits()[index];
        }
        public GuestRequest FindRequest (int num)
        {
            int index = DataSource.hostingUnitList.FindIndex(item => item.HostingUnitKey == num);
            if (index == -1)
            {
                throw new ArgumentException("There is no such a guest request");//
            }
            return getGuestRequests()[index];
        }

        //----------------------------------------------------------------

        
    }
}
