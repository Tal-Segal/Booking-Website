using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    /// <summary>
    /// This is the interface that contains all the public functions of the BL
    /// </summary>
    public interface IBL
    {

        //**Basic Functions**

        //-----------------------------------------------------------------

        /// <summary>
        /// Send a guest request to DAL to add to the Data Source ONLY if he fits the conditions
        /// </summary>
        /// <param name="gr"></param>
        void addClientRequest(GuestRequest gr);

        //-----------------------------------------------------------------

        /// <summary>
        /// Update details of an existing request 
        /// </summary>
        /// <param name="gr"></param>
        void updateClientRequestStatus(GuestRequest gr);

        //-----------------------------------------------------------------

        /// <summary>
        ///  Send a hosting unit to DAL to add to the Data Source ONLY if he fits the conditions
        /// </summary>
        /// <param name="hu"></param>
        void addHostingUnit(HostingUnit hu);

        //-----------------------------------------------------------------

        /// <summary>
        /// Delete an existing hosting unit
        /// </summary>
        /// <param name="hu"></param>
        void deleteHostingUnit(HostingUnit hu);

        //-----------------------------------------------------------------

        /// <summary>
        /// Update details of an existing hosting unit
        /// </summary>
        /// <param name="hu"></param>
        void updateHostingUnit(HostingUnit hu);

        //-----------------------------------------------------------------

        /// <summary>
        /// Send a order to DAL to add to the Data Source ONLY if he fits the conditions
        /// </summary>
        /// <param name="o"></param>
        void addOrder(Order o);

        //-----------------------------------------------------------------

        /// <summary>
        /// Update details of an existing order
        /// </summary>
        /// <param name="o"></param>
        void updateOrder(Order o);

        //-----------------------------------------------------------------

        //**Essential Functions**

        List<HostingUnit> emptyHostingUnits(DateTime date, int days);
        int howLong(DateTime date1, DateTime date2);
        int howLong(DateTime date1);
        bool orderIsClosed(Order o);
        List<Order> daysSinceCreation(int days);
        int numOfOrdersToRequest(GuestRequest gr);
        int numOfOrdersToUnit(HostingUnit hu);
        List<HostingUnit> getHostingUnits();
        List<GuestRequest> getGuestRequests();
        List<Order> getOrders();
        List<string> getAllBankBranches();
        List<GuestRequest> getNewList(Predicate<GuestRequest> cond);
        List<HostingUnit> unitAreaGrouping(area Area);
        //List<HostingUnit> hostGrouping(int num);
        List<GuestRequest> numOfVisitorsGrouping(int num);
        List<GuestRequest> areaGrouping(area Area);
        int getUnitsConfig();
        int getRequestsConfig();
        int getOrdersConfig();
        List<BankBranch> getBankBranches();
        void MonthThread();


        //-----------------------------------------------------------------

        //**Additional Functions**

        int AnnnualOccupancyToHostingUnit(HostingUnit hu);
        string AnnualOccupancyPercent(HostingUnit hu);
        area mostRequestedArea();
        bool checkHostID(Host h);
        type mostRequestedTypeOfUnit();
        int numOfApprovedOrders();
        HostingUnit CheapestHostingUnit();
        HostingUnit FindUnit(int num);
        int FindHost(string password);
        GuestRequest FindRequest(int num);

        //-----------------------------------------------------------------

        void sendMail(string src, string dst);
    }
}

