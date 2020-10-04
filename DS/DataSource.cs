using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DS
{
    public class DataSource
    {

        public static List<GuestRequest> guestRequestList = new List<GuestRequest>()
        {
            new GuestRequest()
            {
              GuestRequestKey=Configuration.guestRequestKey++,
              PrivateName="Izdarechet",
              FamilyName="Shlomzion",
              MailAddress="IS@gmail.com",
              Status=RequestStatus.Open,
              RegistrationDate=DateTime.Now,
              EntryDate=new DateTime(2021,2,3),
              ReleaseDate=new DateTime(2021,2,10),
              Area=area.Jerusalem,
              Type=type.Zimmer,
              Adults=2,
              Children=2,
              Pool=Preference.Must,
              Jacuzzi=Preference.Must,
              Garden=Preference.Unwanted,
              Beach=false,
              ChildrenAttractions=Preference.Unwanted,
              minPrice=200,
              maxPrice=400,
    }
        };
        public static List<Order> orderList = new List<Order>()
        {
            //new Order()
            //{
            //    HostingUnitKey=123,
            //    GuestRequestKey=123,
            //    OrderKey=Configuration.orderKey++,
            //    Fee=40,
            //    Status=RequestStatus.Approved,
            //}
        };
        public static List<BankBranch> banksList = new List<BankBranch>();
        public static List<HostingUnit> hostingUnitList = new List<HostingUnit>()
        {
        new HostingUnit()
        {
            Area = area.Jerusalem,
            HostingUnitKey = Configuration.hostingUnitKey++,
            HostingUnitName = "river",
            price = 2000,
            Diary = new bool[13, 31],
            Owner = new Host()
            {
                ID = 12345789,
                PrivateName = "Talya",
                FamilyName = "Yazdi",
                PhoneNumber = 0543725708,
                BankAccountNumber = 71,
                CollectionClearance = true,
                MailAddress = "t@jct.ac.il",

                password = "A"
            }
        },
        new HostingUnit()
        {
            Area = area.North,
            HostingUnitKey = Configuration.hostingUnitKey++,
            HostingUnitName = "blue",
            price = 1800,
            Diary = new bool[13, 31],
            Owner = new Host()
            {
                ID = 987654321,
                PrivateName = "Talya",
                FamilyName = "Yazdi",
                PhoneNumber = 0543725708,
                BankAccountNumber = 71,

                CollectionClearance = true,
                MailAddress = "t@jct.ac.il",
                password = "B"
            }
        }
        };
    

    
    }





}