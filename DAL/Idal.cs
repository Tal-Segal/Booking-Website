using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    /// <summary>
    /// This is the interface that contains all the public functions of the DAL
    /// </summary>
    public interface Idal
    {

        //-----------------------------------------------------------------

        /// <summary>
        /// Add a new guest request to the Data Source
        /// </summary>
        /// <param name="gr"></param>
        void addClientRequest(GuestRequest gr);

        //-----------------------------------------------------------------

        /// <summary>
        /// Update details of an existing guest request
        /// </summary>
        /// <param name="gr"></param>
        void updateClientRequestStatus(GuestRequest gr);

        //-----------------------------------------------------------------

        /// <summary>
        /// Add a new hosting unit to the Data Source
        /// </summary>
        /// <param name="hu"></param>
        void addHostingUnit(HostingUnit hu);

        //-----------------------------------------------------------------

        /// <summary>
        /// Delete a hosting unit from the Data Source
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
        /// Add a new order to the Data Source
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

        List<HostingUnit> getHostingUnits();
        List<GuestRequest> getGuestRequests();
        List<Order> getOrders();
        List<string> getAllBankBranches();
        HostingUnit FindUnit(int num);
        GuestRequest FindRequest(int num);
        int getUnitsConfig();
        int getRequestsConfig();
        int getOrdersConfig();
        List<BankBranch> getBankBranches();




    }
}
