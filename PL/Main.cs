using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BE;

//Avital Prives 322435165
//Tal Segal 323013888

namespace PL
{
    public class PL
    {
        static void Main(string[] args)
        {
            IBL bl = BLFactory.getBL();

            #region newGuestRequest
            GuestRequest gr = new GuestRequest();
            gr.GuestRequestKey = Configuration.guestRequestKey++;
            gr.PrivateName = "Tal";
            gr.FamilyName = "Segal";
            gr.MailAddress = "talsegal@gmail.com";
            gr.Status = RequestStatus.Open;
            gr.RegistrationDate = new DateTime(2019, 03, 02);
            gr.EntryDate = new DateTime(2019, 03, 12);
            gr.ReleaseDate = new DateTime(2019, 03, 15);
            gr.Area = area.Center;
            gr.Type = type.Igloo;
            gr.Adults = 2;
            gr.Children = 3;
            gr.Pool = Preference.Possible;
            gr.Garden = Preference.Must;
            gr.Jacuzzi = Preference.Possible;
            gr.Beach = false;
            gr.minPrice = 1000;
            gr.maxPrice = 2200;
            gr.ChildrenAttractions = Preference.Unwanted;
            #endregion

            #region addGuestRequest
            GuestRequest g1 = new GuestRequest();
            try
            {
                bl.addClientRequest(gr);
                g1 = bl.getGuestRequests()[bl.getGuestRequests().Count() - 1];
                Console.WriteLine("\nGuest request add succesfully:\n");
                Console.WriteLine(g1.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
            #endregion

            #region newHost
            Host h = new Host();
            h.ID = 315428976;
            h.PrivateName = "Avital";
            h.FamilyName = "Prives";
            h.PhoneNumber = 0505678432;
            h.MailAddress = "prives@gmail.com";
            h.BankBranchDetails = new BankBranch();
            h.BankBranchDetails.BankName = "Hapoalim";
            h.BankBranchDetails.BankNumber = 4;
            h.BankBranchDetails.BranchAddress = "Ben Yehuda";
            h.BankBranchDetails.BranchCity = "Tel Aviv";
            h.BankBranchDetails.BranchNumber = 45;
            h.BankAccountNumber = 54;
            h.CollectionClearance = true;
            #endregion

            #region newHostingUnit
            HostingUnit hu = new HostingUnit();
            hu.Owner = h;
            hu.price = 1500;
            hu.Area = area.North;
            hu.Type = type.Igloo;
            hu.Adults = 2;
            hu.Children = 3;
            hu.Pool = true;
            hu.Jacuzzi = true;
            hu.Beach = false;
            hu.Garden = false;
            hu.ChildrenAttractions = false;
            hu.HostingUnitKey = Configuration.hostingUnitKey++;
            hu.HostingUnitName = "Frozen Igloo";
            hu.Diary = new bool[31, 13];
            for (int i = 1; i < 13; i++)
                for (int j = 1; j < 31; j++)
                    hu.Diary[j, i] = false;
            #endregion

            #region tryDeleteHostingUnit
            HostingUnit h1 = new HostingUnit();
            try
            {
                bl.deleteHostingUnit(hu);
                h1 = bl.getHostingUnits()[bl.getHostingUnits().Count() - 1];
                Console.WriteLine("\nDelete hosting unit succesfully:\n");
                Console.WriteLine(h1.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
            #endregion

            #region addHostingUnit
            HostingUnit h2 = new HostingUnit();
            try
            {
                bl.addHostingUnit(hu);
                h2 = bl.getHostingUnits()[bl.getHostingUnits().Count() - 1];
                Console.WriteLine("\nHosting unit add succesfully:\n");
                Console.WriteLine(h2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region updateHostingUnit
            h2.HostingUnitName = "Home Sweet Home";
            try
            {
                bl.updateHostingUnit(h2);
                Console.WriteLine("\nHosting unit update succesfully:\n");
                Console.WriteLine(h2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            Console.WriteLine("\nAfter edit\n");

            #region newOrder
            Order or = new Order();
            or.OrderKey = Configuration.orderKey++;
            or.HostingUnitKey = h2.HostingUnitKey;
            or.GuestRequestKey = g1.GuestRequestKey;
            or.CreateDate = new DateTime(2019, 01, 29);
            or.OrderDate = or.CreateDate;
            or.Status = RequestStatus.Open;
            #endregion

            #region addOrder
            Order o1 = new Order();
            try
            {
                bl.addOrder(or);
                o1 = bl.getOrders()[bl.getOrders().Count() - 1];
                Console.WriteLine("\nOrder added succcessfuly: \n");
                Console.WriteLine(o1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
            #endregion

            or.Status = RequestStatus.Approved;

            #region updateOrder
            try
            {
                bl.updateOrder(or);
                Console.WriteLine("\nOrder update succcessfuly:");
                Console.WriteLine("\n" + or);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
            #endregion

            Console.WriteLine("The annual occupancy to this unit is: "+bl.AnnnualOccupancyToHostingUnit(hu));
            bl.AnnualOccupancyPercent(hu);

            Console.WriteLine("\nNumber of orders to the first request: " + bl.numOfOrdersToRequest(gr));

            #region tryDeleteHostingUnit
            try
            {
                bl.deleteHostingUnit(h2);
                Console.WriteLine("\nhosting unit deletion succcessfuly:\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
            #endregion

            Console.WriteLine("\nThe most requested area by guests is: " + bl.mostRequestedArea());

            #region newHost2
            Host ho = new Host();
            ho.ID = 315428976;
            ho.PrivateName = "Talya";
            ho.FamilyName = "Gil";
            ho.PhoneNumber = 0501234532;
            ho.MailAddress = "gil@gmail.com";
            ho.BankBranchDetails = new BankBranch();
            ho.BankBranchDetails.BankName = "Hapoalim";
            ho.BankBranchDetails.BankNumber = 4;
            ho.BankBranchDetails.BranchAddress = "Ben Yehuda";
            ho.BankBranchDetails.BranchCity = "Tel Aviv";
            ho.BankBranchDetails.BranchNumber = 45;
            ho.BankAccountNumber = 54;
            ho.CollectionClearance = true;
            #endregion

            #region newHostingUnit2
            HostingUnit hu2 = new HostingUnit();
            hu2.Owner = ho;
            hu2.price = 1000;
            hu2.Area = area.North;
            hu2.Type = type.Zimmer;
            hu2.Adults = 2;
            hu2.Children = 5;
            hu2.Pool = true;
            hu2.Jacuzzi = true;
            hu2.Beach = true;
            hu2.Garden = false;
            hu2.ChildrenAttractions = false;
            hu2.HostingUnitKey = Configuration.hostingUnitKey++;
            hu2.HostingUnitName = "Hot Zimmer";
            hu2.Diary = new bool[31, 13];
            for (int i = 1; i < 13; i++)
                for (int j = 1; j < 31; j++)
                    hu2.Diary[j, i] = false;
            #endregion

            #region addHostingUnit2
            HostingUnit hu3 = new HostingUnit();
            try
            {
                bl.addHostingUnit(hu2);
                hu3 = bl.getHostingUnits()[bl.getHostingUnits().Count() - 1];
                Console.WriteLine("\nHosting unit add succesfully:\n");
                Console.WriteLine(hu3);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion

            Console.WriteLine("Number of orders to this unit: " + bl.numOfOrdersToUnit(hu3));
            Console.WriteLine("\n\nAll hosts' ID's are correct: " + bl.checkHostID(h));
            Console.WriteLine("\nMost requested type of hosting unit: " + bl.mostRequestedTypeOfUnit());
            Console.WriteLine("\nNumber of Approved Orders: " + bl.numOfApprovedOrders() + "\n");
            Console.WriteLine("\nCheapest hosting unit:\n\n" + bl.CheapestHostingUnit());

            #region groupingChecking
            bl.unitAreaGrouping(area.Jerusalem);
            bl.numOfVisitorsGrouping(3);
           // bl.hostGrouping(1);
            bl.areaGrouping(area.Center);
            #endregion

            Console.ReadLine();

        }
    }
}
/*
 
 *****RUNNING EXAMPLE:*****


Guest request add succesfully:

Guest Request Key: 10000000
Private Name: Tal
Family Name: Segal
Mail Address: talsegal@gmail.com
Request's status: Open
Registration Date: 02/03/2019 00:00:00
Entry Date: 12/03/2019 00:00:00
Release Date: 15/03/2019 00:00:00
Request Area: Center
Request Type: Igloo
Adults: 2
Children: 3
Pool: Possible
Jacuzzi Possible
Garden: Must
Beach: False
Attractions: Unwanted
Minimum price: 1000
Maximum price: 2200


There is no such a hosting unit

Hosting unit add succesfully:

Hosting Unit Key: 10000000
Hosting Unit Name: Frozen Igloo
Price: 1500
Area: North
Type: Igloo
Adults: 2
Children: 3
Pool: True
Jacuzzi True
Garden: False
Beach: False
Attractions: False


Hosting unit update succesfully:

Hosting Unit Key: 10000000
Hosting Unit Name: Home Sweet Home
Price: 1500
Area: North
Type: Igloo
Adults: 2
Children: 3
Pool: True
Jacuzzi True
Garden: False
Beach: False
Attractions: False


After edit


Order added succcessfuly:

Hosting Unit Key: 10000000
Guest Request Key: 10000000
Order Key: 10000000
Fee: 0
Status: Open
Create Date: 29/01/2019 00:00:00
Order Date: 29/01/2019 00:00:00


Order update succcessfuly:

Hosting Unit Key: 10000000
Guest Request Key: 10000000
Order Key: 10000000
Fee: 40
Status: Approved
Create Date: 29/01/2019 00:00:00
Order Date: 29/01/2019 00:00:00

The annual occupancy to this unit is: 4
Annual occupancy percent for this unit is: 1%

Number of orders to the first request: 1

There are still open orders connected to this hosting unit

The most requested area by guests is: Center

Hosting unit add succesfully:

Hosting Unit Key: 10000001
Hosting Unit Name: Hot Zimmer
Price: 1000
Area: North
Type: Zimmer
Adults: 2
Children: 5
Pool: True
Jacuzzi True
Garden: False
Beach: True
Attractions: False

Number of orders to this unit: 0


All hosts' ID's are correct: False

Most requested type of hosting unit: Igloo

Number of Approved Orders: 1


Cheapest hosting unit:

Hosting Unit Key: 10000001
Hosting Unit Name: Hot Zimmer
Price: 1000
Area: North
Type: Zimmer
Adults: 2
Children: 5
Pool: True
Jacuzzi True
Garden: False
Beach: True
Attractions: False

*/
