using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Threading;
using System.Xml;
using BE;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using DAL;

namespace BL
{
    /// <summary>
    /// This class connects between the UI and Dal and DataSource layers
    /// In every action it checks if it fits the configurations and other lofics that are required
    /// </summary>
    public class MyBlList : IBL
    {
        //--------------------------------------------------------------------------------------------
        Idal dal = DalFactory.getDal();

        //-----------------------------------------------------------------

        /// <summary>
        /// This functions checks if there is more than one day
        /// between entry and release dates
        /// </summary>
        /// <param name="gr"></param>
        private bool checkDaysBetweenDates(GuestRequest gr)
        {
            if (gr.EntryDate < gr.ReleaseDate)
                return true;
            return false;
        }
        //-----------------------------------------------------------------

        public void addClientRequest(GuestRequest gr)
        {
            if (checkDaysBetweenDates(gr))
            {
                try
                {
                    dal.addClientRequest(gr.Clone());
                }

                catch (Exception s)
                {
                    throw s;
                }
            }
            else
                throw new ArgumentException("Less than a day between entry and release");
        }
        //-----------------------------------------------------------------

        public void updateOrder(Order o)
        {
            if (isNotTaken(o))
            {
                if (o.Status == RequestStatus.MailWasSent)
                {
                    if (!checkCollectionClearance(o))
                        throw new ArgumentException("No Collection Clearance");
                    o.OrderDate = DateTime.Now;
                    Console.WriteLine("sending an email");
                }
                if (o.Status == RequestStatus.Approved)
                {
                    approveOrder(o);
                    o.Fee = fee(o);
                }
                try
                {
                    dal.updateOrder(o);
                }
                catch (Exception s)
                {
                    throw s;
                }
            }
            else
                throw new ArgumentException("This dates are already taken");
        }
        //-----------------------------------------------------------------

        public void updateClientRequestStatus(GuestRequest gr)
        {
            try
            {
                dal.updateClientRequestStatus(gr);
            }
            catch (Exception s)
            {
                throw s;
            }
        }
        //-----------------------------------------------------------------

        private void approveOrder(Order o)
        {
            if (isNotTaken(o))
            {
                var v1 = dal.getHostingUnits().Where(item => item.HostingUnitKey == o.HostingUnitKey);
                var v2 = dal.getGuestRequests().Where(item => item.GuestRequestKey == o.GuestRequestKey);
                var v3 = dal.getOrders().Where(item => item.OrderKey == o.OrderKey);
                DateTime day = v2.First().EntryDate;
                while (day <= v2.First().ReleaseDate)
                {
                    v1.First().Diary[day.Month, day.Day] = true;
                    day = day.AddDays(1);
                }
                v3.First().Status = RequestStatus.Approved;
                foreach (var item in dal.getOrders())
                {
                    if (item.GuestRequestKey == o.GuestRequestKey && item.OrderKey != o.OrderKey)
                        item.Status = RequestStatus.Unanswered;
                }
            }
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function calculates the fee by number of days of stay
        /// </summary>
        /// <param name="o"></param>
        private int fee(Order o)
        {
            var v = dal.getGuestRequests().Where(item => item.GuestRequestKey == o.GuestRequestKey);
            DateTime day = v.FirstOrDefault().EntryDate;
            int sum = 0;
            while (day <= v.FirstOrDefault().ReleaseDate)
            {
                sum += Configuration.fee;
                day = day.AddDays(1);
            }
            return sum;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function checks if an order has been approved or expired
        /// </summary>
        /// <param name="o"></param>
        public bool orderIsClosed(Order o)
        {
            var v = dal.getOrders().Where(item => item.OrderKey == o.OrderKey);
            if (v.First().Status == RequestStatus.Approved || v.First().Status == RequestStatus.Expired)
                return true;
            return false;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function checks if the owner of a given hosting unit has a collection clearance
        /// </summary>
        /// <param name="o"></param>
        private bool checkCollectionClearance(Order o)
        {
            var v = dal.getHostingUnits().Where(t => t.HostingUnitKey == o.HostingUnitKey);
            if (v.FirstOrDefault().Owner.CollectionClearance)
                return true;
            return false;
        }
        //-----------------------------------------------------------------

        public void addOrder(Order o)
        {
            if (dal.getHostingUnits().FindIndex(t => t.HostingUnitKey == o.HostingUnitKey) == -1)
                throw new KeyNotFoundException("there is not such hosting unit to this order");
            if (dal.getGuestRequests().FindIndex(t => t.GuestRequestKey == o.GuestRequestKey) == -1)
                throw new KeyNotFoundException("there is not such guest request to this order");
            if (isNotTaken(o))
            {
                try
                {
                    dal.addOrder(o.Clone());
                }
                catch (Exception s)
                {
                    throw s;
                }
            }
            else
                throw new ArgumentException("This dates are already taken");
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function checks if certain dates have not been taken yet
        /// </summary>
        /// <param name="o"></param>
        private bool isNotTaken(Order o)
        {
            var v1 = dal.getHostingUnits().Where(t => t.HostingUnitKey == o.HostingUnitKey);
            var v2 = dal.getGuestRequests().Where(t => t.GuestRequestKey == o.GuestRequestKey);
            DateTime d = v2.FirstOrDefault().EntryDate;
            DateTime d2 = v2.FirstOrDefault().ReleaseDate;
            while (d <= d2)
            {
                if (v1.FirstOrDefault().Diary[d.Month, d.Day]) //תפוס
                    return false;
                d = d.AddDays(1);
            }
            return true;
        }
        //-----------------------------------------------------------------

        public void addHostingUnit(HostingUnit hu)
        {
            try
            {
                dal.addHostingUnit(hu.Clone());
            }
            catch (Exception s)
            {
                throw s;
            }
        }
        //-----------------------------------------------------------------

        public void deleteHostingUnit(HostingUnit hu)
        {
            foreach (var item in dal.getOrders())
            {
                if (item.HostingUnitKey == hu.HostingUnitKey)
                    throw new ArgumentException("There are still open orders connected to this hosting unit");
            }
            try
            {
                dal.deleteHostingUnit(hu);
            }
            catch (Exception s)
            {
                throw s;
            }
        }
        //-----------------------------------------------------------------

        public void updateHostingUnit(HostingUnit hu)
        {
            if (hu.Owner.CollectionClearance == false)
            {
                int ID = hu.Owner.ID;
                foreach (var item in dal.getOrders())
                {
                    int x = item.HostingUnitKey;
                    int y = dal.getHostingUnits().FindIndex(otherItem => otherItem.HostingUnitKey == x);
                    if (dal.getHostingUnits()[x].Owner.ID == ID)
                        if (item.Status == RequestStatus.Open)
                            throw new ArgumentException("there are still open orders with this host");
                }
            }
            try
            {
                dal.updateHostingUnit(hu.Clone());
            }
            catch (Exception s)
            {
                throw s;
            }
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function checks which hosting units are available on certain dates
        /// </summary>
        /// <param name="date"></param>
        /// <param name="days"></param>
        public List<HostingUnit> emptyHostingUnits(DateTime date, int days)
        {
            var v = from item in dal.getHostingUnits()
                    where helpEmptyHostingUnits(date, days, item)
                    select item;
            return v.ToList();
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// Auxiliary function
        /// </summary>
        /// <param name="date"></param>
        /// <param name="days"></param>
        /// <param name="hu"></param>
        private bool helpEmptyHostingUnits(DateTime date, int days, HostingUnit hu)
        {
            int mone = 0;
            while (mone <= days)
            {
                if (hu.Diary[date.Month, date.Day] == true)
                    return false;
                mone++;
                date = date.AddDays(1);
            }
            return true;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function calculates how much time has passed between two dates
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        public int howLong(DateTime date1, DateTime date2)
        {
            int counter = 0;
            while (date1 <= date2)
            {
                counter++;
                date1 = date1.AddDays(1);
            }
            return counter;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function calculates how much time has passed from a certain date till today 
        /// </summary>
        /// <param name="date1"></param>
        public int howLong(DateTime date1)
        {
            DateTime d = DateTime.Now;
            return howLong(date1, d);
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function returns an order list that a certain number of days have passed
        /// since their creation
        /// </summary>
        /// <param name="days"></param>
        public List<Order> daysSinceCreation(int days)
        {
            var v = from item in dal.getOrders()
                    where helpDaysSinceCreation(item, days)
                    select item;
            return v.ToList();
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// Auxiliary function
        /// </summary>
        /// <param name="o"></param>
        /// <param name="days"></param>
        private bool helpDaysSinceCreation(Order o, int days)
        {
            if (o.Status == RequestStatus.MailWasSent)
                return (howLong(o.OrderDate) >= days);
            return (howLong(o.CreateDate) >= days);
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function returns a list of guest requests that meet a certain condition
        /// </summary>
        /// <param name="cond"></param>
        public List<GuestRequest> getNewList(Predicate<GuestRequest> cond)
        {
            var v = from item in dal.getGuestRequests()
                    where cond(item)
                    select (item);
            return v.ToList();
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function calculates how many orders have been sent to a certain request
        /// </summary>
        /// <param name="gr"></param>
        public int numOfOrdersToRequest(GuestRequest gr)
        {
            int counter = 0;
            foreach (var item in dal.getOrders())
            {
                if (item.GuestRequestKey == gr.GuestRequestKey)
                    counter++;
            }
            return counter;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function calculates how many orders have been applied to a certain hosting unit
        /// </summary>
        /// <param name="hu"></param>
        public int numOfOrdersToUnit(HostingUnit hu)
        {
            var v = from item in dal.getOrders()
                    where item.HostingUnitKey == hu.HostingUnitKey && item.Status != RequestStatus.Unanswered && item.Status != RequestStatus.Expired
                    select item;
            return (v.ToArray()).Length;
        }
        //-----------------------------------------------------------------

        public List<HostingUnit> getHostingUnits() { return dal.getHostingUnits(); }
        public List<GuestRequest> getGuestRequests() { return dal.getGuestRequests(); }
        public List<Order> getOrders() { return dal.getOrders(); }
        public List<string> getAllBankBranches()
        {
            return dal.getAllBankBranches();
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// Grouping guest requests by area
        /// </summary>
        /// <param name="Area"></param>
        public List<GuestRequest> areaGrouping(area Area)
        {
            IEnumerable<IGrouping<area, GuestRequest>> result = from n in dal.getGuestRequests()
                                                                group n by n.Area into gr
                                                                select gr;
            var sorted = result.Where(item => item.Key == Area).ToList();
            if (sorted.Count() == 0)
                return null;
            List<GuestRequest> result1 = new List<GuestRequest>();

            foreach (var item in sorted[0])
            {
                result1.Add(item);
            }
            return result1;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// Grouping guest requests by number of visitors
        /// </summary>
        /// <param name="num"></param>
        public List<GuestRequest> numOfVisitorsGrouping(int num)
        {

            IEnumerable<IGrouping<int, GuestRequest>> result = from n in dal.getGuestRequests()
                                                               group n by n.Adults + n.Children into gr
                                                               select gr;
            var sorted = result.Where(item => item.Key == num).ToList();
            if (sorted.Count() == 0)
                return null;
            List<GuestRequest> result1 = new List<GuestRequest>();

            foreach (var item in sorted[0])
            {
                result1.Add(item);
            }
            return result1;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// Grouping hosts by their amount of hosting units
        /// </summary>
        /// <param name="num"></param>
        //public List<HostingUnit> hostGrouping(int num)
        //{

        //    IEnumerable<IGrouping<Host, HostingUnit>> result = from n in dal.getHostingUnits()
        //                                                       group n by n.Owner into h
        //                                                       select h;
        //    IEnumerable<IGrouping<Host, HostingUnit>> resultB = from n in result
        //                                                        let numberOfUnits = numOfUnits(n)
        //                                                        where numberOfUnits == num
        //                                                        select n;
        //    var sorted = resultB.Where(item => item.Key == num).ToList();
        //    if (resultB.Count() == 0)
        //        return null;
        //    List<HostingUnit> result1 = new List<HostingUnit>();

        //    foreach (var item in sorted[0])
        //    {
        //        result1.Add(item);
        //    }
        //    return result1;
        //}

        //-----------------------------------------------------------------

        /// <summary>
        /// Auxiliary function - calculates for each host how many units he owns
        /// </summary>
        /// <param name="h"></param>
        private int numOfUnits(IGrouping<Host, HostingUnit> h)
        {
            return (from n in dal.getHostingUnits()
                    where n.Owner == h.Key
                    select n).ToArray<HostingUnit>().Length;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// Grouping hosting units by area
        /// </summary>
        /// <param name="Area"></param>
        public List<HostingUnit> unitAreaGrouping(area Area)
        {
            IEnumerable<IGrouping<area, HostingUnit>> result = from n in dal.getHostingUnits()
                                                               group n by n.Area into gr
                                                               select gr;
            var sorted = result.Where(item => item.Key == Area).ToList();
            if (sorted.Count() == 0)
                return null;
            List<HostingUnit> result1 = new List<HostingUnit>();

            foreach (var item in sorted[0])
            {
                result1.Add(item);
            }
            return result1;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function returns the annual occupancy to a certain hosting unit
        /// </summary>
        /// <param name="hu"></param>
        public int AnnnualOccupancyToHostingUnit(HostingUnit hu)
        {
            int sum = 0;
            for (int i = 1; i < 13; i++)
                for (int j = 1; j < 31; j++)
                {
                    if (hu.Diary[i, j])
                        sum++;
                }
            return sum;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function calculates the annual occupancy percent per year
        /// </summary>
        /// <param name="hu"></param>
        public string AnnualOccupancyPercent(HostingUnit hu)
        {
            int tmp = AnnnualOccupancyToHostingUnit(hu);
            return ("Annual occupancy percent for this unit is: " + (tmp * 100) / 365 + "%");
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function returns the most requests area by guests
        /// </summary>
        public area mostRequestedArea()
        {
            IEnumerable<IGrouping<area, GuestRequest>> result = from n in dal.getGuestRequests()
                                                                group n by n.Area into gr
                                                                select gr;
            int tmp = 0;
            area a = new area();
            foreach (var item in result)
            {
                if (item.ToArray().Length > tmp)
                {
                    tmp = item.ToArray().Length;
                    a = item.Key;
                }
            }
            return a;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function returns the cheapest hosting unit by its price
        /// </summary>
        public HostingUnit CheapestHostingUnit()
        {
            HostingUnit tmp = dal.getHostingUnits().First();
            foreach (var item in dal.getHostingUnits())
            {
                if (item.price < tmp.price)
                    tmp = item;
            }
            return tmp;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function returns the number of approved orders that have been closed 
        /// in our amazing website
        /// </summary>
        public int numOfApprovedOrders()
        {
            var v = from item in dal.getOrders()
                    where item.Status == RequestStatus.Approved
                    select item;
            return v.ToArray().Length;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function returns the most requested type of hosting unit
        /// </summary>
        public type mostRequestedTypeOfUnit()
        {
            IEnumerable<IGrouping<type, GuestRequest>> result = from n in dal.getGuestRequests()
                                                                group n by n.Type into gr
                                                                select gr;
            int tmp = 0;
            type a = new type();
            foreach (var item in result)
            {
                if (item.ToArray().Length > tmp)
                {
                    tmp = item.ToArray().Length;
                    a = item.Key;
                }
            }
            return a;
        }

        //-----------------------------------------------------------------

        /// <summary>
        /// This function checks that there are not two hosts with the same ID
        /// </summary>
        /// <param name="h"></param>
        public bool checkHostID(Host h)
        {
            IEnumerable<IGrouping<Host, HostingUnit>> result = from n in dal.getHostingUnits()
                                                               group n by n.Owner into gr
                                                               select gr;
            var v = from item in result
                    where item.Key.ID == h.ID && item.Key.MailAddress != h.MailAddress //same ID, different person
                    select item;
            if (v.ToArray().Length > 0)
                return false;
            return true;
        }

        //------------------------------------------------------------------------------------------

        public HostingUnit FindUnit(int num)
        {
            return dal.FindUnit(num);
        }
        public GuestRequest FindRequest(int num)
        {
            return dal.FindRequest(num);
        }

        //------------------------------------------------------------------------------------------
        public int FindHost(string password)
        {
            int index = dal.getHostingUnits().FindIndex(otherItem => otherItem.Owner.password == password);
            return index;
        }

        //------------------------------------------------------------------------------------------

        public void sendMail(string src, string dst)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(dst);
            mail.From = new MailAddress(src);
            mail.Subject = "Hosting offer from A&T Systems";
            mail.Body = "In a peaceful place we have found the perfect accommodation for you.<br>" +
                "We invite you to enjoy everything it has to offer.<br> Let us know. " + src;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;
            smtp.Credentials = new System.Net.NetworkCredential("atsystems4@gmail.com", "miniproject");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MoreThanMonth()
        {
            while (true)
            {
                foreach (Order order in getOrders())
                {
                    DateTime tmp = order.OrderDate;
                    if ((tmp.AddDays(30) <= DateTime.Now) && (order.Status == RequestStatus.MailWasSent))
                    {
                        order.Status = RequestStatus.Unanswered;
                        updateOrder(order);
                    }
                    Thread.Sleep(86400000);
                }
            }
        }

        public void MonthThread()
        {
            Thread t = new Thread(MoreThanMonth);
            t.IsBackground = true;
            t.Start();
        }
        //------------------------------------------------------------------------------------------

        public int getUnitsConfig()
        {
            return dal.getUnitsConfig();
        }
        public int getRequestsConfig()
        {
            return dal.getRequestsConfig();
        }
        public int getOrdersConfig()
        {
            return dal.getOrdersConfig();
        }
        public List<BankBranch> getBankBranches()
        {
            return dal.getBankBranches();
        }



    }
}



