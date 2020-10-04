using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;
using BE;
using System.Xml.Serialization;
using System.IO;
using System.Net;


namespace DAL
{


    public class DAL_XML_imp : Idal
    {

        string UnitsRootPath = @"units.xml"; 

        string RequestsRootPath = @"requests.xml";

        XElement ConfigRoot;
        string ConfigRootPath = @"Config.xml";

        XElement OrdersRoot;
        string OrdersRootPath = @"orders.xml";

        XElement banksRoot;
        string banksPath = @"atm.xml";
        public DAL_XML_imp()
        {

            if (!File.Exists(banksPath))
            {
                
                try
                {
                    downloadAtm();
                    new Thread(downloadAtm).Start();
                    while (!File.Exists(banksPath))
                    {
                        Thread.Sleep(1000 * 10);
                    }
                    banksRoot = XElement.Load(banksPath);
                }
                catch
                {
                    throw new FileLoadException("file upload problem");
                }
            }
            else
            {
                banksRoot = XElement.Load(banksPath);
            }

            if (!File.Exists(UnitsRootPath))
            {

                FileStream f = new FileStream(UnitsRootPath,FileMode.Create);
                f.Close();

                
            }
            if (!File.Exists(banksPath))
            {
                FileStream f = new FileStream(banksPath, FileMode.Create);
                f.Close();

            }
            if (!File.Exists(OrdersRootPath))
            {
                OrdersRoot = new XElement("orders");
                OrdersRoot.Save(OrdersRootPath);
            }
            else
            {
                try { OrdersRoot = XElement.Load(OrdersRootPath); }
                catch { throw new KeyNotFoundException("File upload problem"); }
            }
            if (!File.Exists(ConfigRootPath))
            {
                ConfigRoot = new XElement("Config");
                ConfigRoot.Add(new XElement("guestRequestKey", 10000000));
                ConfigRoot.Add(new XElement("hostingUnitKey", 10000000));
                ConfigRoot.Add(new XElement("orderKey", 10000000));
                ConfigRoot.Add(new XElement("managerPassword", 1234));
                ConfigRoot.Add(new XElement("fee", 10));
                ConfigRoot.Add(new XElement("expireRequestDays", 30));
                ConfigRoot.Save(ConfigRootPath);
            }
            else
            {
                try { ConfigRoot = XElement.Load(ConfigRootPath); }
                catch { throw new KeyNotFoundException("File upload problem"); }
            }
            if (!File.Exists(RequestsRootPath))
            {
                FileStream f = new FileStream(RequestsRootPath, FileMode.Create);
                f.Close();
            }

           
        }
       
        public static void saveListToXML<T>(List<T> list, string path)
        {
            XmlSerializer x = new XmlSerializer(list.GetType());
            FileStream fs = new FileStream(path, FileMode.Create);
            x.Serialize(fs, list);
            fs.Close();
        }

        public static List<T> loadListFromXML<T>(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(List<T>));
                List<T> R = (List<T>)x.Deserialize(fs);
                fs.Close();
                return R;
            }
            catch
            {
                fs.Close();
                return new List<T>();
            }
            
        }

        public void addClientRequest(GuestRequest gr)
        {
            List<GuestRequest> lst = loadListFromXML<GuestRequest>(RequestsRootPath);
            if (lst.Exists(item => item.GuestRequestKey == gr.GuestRequestKey) == true)
                throw new ArgumentException("This request already exists");
            else
            {
                
                int config = Convert.ToInt32(ConfigRoot.Element("guestRequestKey").Value);
                config++;
                ConfigRoot.Element("guestRequestKey").Value = Convert.ToString(config);
                ConfigRoot.Save(ConfigRootPath);
                lst.Add(gr.Clone());
            }
            saveListToXML<GuestRequest>(lst, RequestsRootPath);
        }

        public void updateClientRequestStatus(GuestRequest gr)
        {
            List<GuestRequest> lst = loadListFromXML<GuestRequest>(RequestsRootPath);
            int x = lst.FindIndex(item => item.GuestRequestKey == gr.GuestRequestKey);
            if (x != -1)
            {
                lst[x].Status= gr.Status;
                saveListToXML<GuestRequest>(lst, RequestsRootPath);
                return;
            }
            throw new KeyNotFoundException("There is no such a request");            
        }

        public void addHostingUnit(HostingUnit hu)
        {
            List<HostingUnit> lst = loadListFromXML<HostingUnit>(UnitsRootPath);
            if (lst.Exists(item => item.HostingUnitKey == hu.HostingUnitKey) == true)
                throw new ArgumentException("This hosting unit already exists");
            else
            {
                bool [,] Diary = new bool[13,31];
                for (int i=0;i<13;i++)
                    for (int j  = 0; j < 31; j++)
                    {
                        Diary[i, j] = false;
                    }
                hu.Diary = Diary;
                lst.Add(hu.Clone());
                int config = Convert.ToInt32(ConfigRoot.Element("hostingUnitKey").Value);
                config++;
                ConfigRoot.Element("hostingUnitKey").Value = Convert.ToString(config);
                ConfigRoot.Save(ConfigRootPath);
                saveListToXML<HostingUnit>(lst, UnitsRootPath);
            }

        }

        public void deleteHostingUnit(HostingUnit hu)
        {
            List<HostingUnit> lst = loadListFromXML<HostingUnit>(UnitsRootPath);
            if (lst.Exists(item => item.HostingUnitKey == hu.HostingUnitKey) == true)
            {
                lst.Remove(hu);
                hu.HostingUnitKey = 0;
                saveListToXML<HostingUnit>(lst, UnitsRootPath);
                return;
            }
            throw new KeyNotFoundException("There is no such a hosting unit");
        }

        public void updateHostingUnit(HostingUnit hu)
        {
            List<HostingUnit> lst = loadListFromXML<HostingUnit>(UnitsRootPath);
            int x =lst.FindIndex(item => item.HostingUnitKey == hu.HostingUnitKey);
            if (x == -1)
            {
                throw new KeyNotFoundException("There is no such a hosting unit");
            }
            lst[x] = hu;
            saveListToXML<HostingUnit>(lst, UnitsRootPath);
        }

        public void addOrder(Order or)
        {
            try
            {
                LoadOrder();
                LoadConfiguration();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            or.OrderKey = getOrdersConfig();
            XElement HostingUnitKey = new XElement("HostingUnitKey", or.HostingUnitKey);
            XElement Fee = new XElement("Fee", or.Fee);
            XElement GuestRequestKey = new XElement("GuestRequestKey", or.GuestRequestKey);
            XElement OrderKey = new XElement("OrderKey", or.OrderKey);
            XElement Status = new XElement("Status", or.Status);
            XElement CreateDate = new XElement("CreateDate", or.CreateDate);
            XElement OrderDate = new XElement("OrderDate", or.OrderDate);
            XElement Order = new XElement("Order", HostingUnitKey,Fee, GuestRequestKey,OrderKey,  Status, CreateDate, OrderDate);
            OrdersRoot.Add(Order);
            ConfigRoot.Save(ConfigRootPath);                          
            OrdersRoot.Save(OrdersRootPath);
        }
        private void LoadOrder()
        {
            try
            {
                OrdersRoot = XElement.Load(OrdersRootPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }

        }
        private void LoadConfiguration()
        {
            try
            {
                ConfigRoot = XElement.Load(ConfigRootPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }

        }


        public void updateOrder(Order or)
        {
            XElement orderElement = (from item in OrdersRoot.Elements()
                                     where or.OrderKey == Convert.ToInt32(item.Element("OrderKey").Value)
                                     select item).FirstOrDefault();
            if (orderElement != null)
            {
                if (or.Status.ToString() == orderElement.Element("Status").Value)
                    throw new Exception("The status of order is already initialized to what you have requested ");

                else
                {
                    orderElement.Element("Status").Value = or.Status.ToString();
                    OrdersRoot.Save(OrdersRootPath);
                }
            }
            else { throw new Exception("Order doesn't exist"); }
        }
        //-----------------------------------------------------------------

        public List<HostingUnit> getHostingUnits()
        {
            return loadListFromXML<HostingUnit>(UnitsRootPath);
        }

        //-----------------------------------------------------------------

        public List<GuestRequest> getGuestRequests()
        {
            return loadListFromXML<GuestRequest>(RequestsRootPath);
        }

        //-----------------------------------------------------------------

        public List<Order> getOrders()
        {

            LoadOrder();
            List<Order> orders;
            try
            {
                orders = (from p in OrdersRoot.Elements()
                          select new BE.Order()
                          {
                              HostingUnitKey = int.Parse(p.Element("HostingUnitKey").Value),
                              GuestRequestKey = int.Parse(p.Element("GuestRequestKey").Value),
                              CreateDate = DateTime.Parse(p.Element("CreateDate").Value),
                              OrderDate = DateTime.Parse(p.Element("OrderDate").Value),
                              OrderKey = int.Parse(p.Element("OrderKey").Value),
                              Fee = int.Parse(p.Element("Fee").Value),
                              Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), p.Element("Status").Value),
                          }).ToList();
                int config = int.Parse(ConfigRoot.Element("orderKey").Value);
                config++;
                ConfigRoot.Element("orderKey").Value = Convert.ToString(config);
                ConfigRoot.Save(ConfigRootPath);
            }
            catch
            {
                orders = null;
            }
            return orders;
            //return loadListFromXML<BE.Order>(OrdersRootPath);
        }



        //-----------------------------------------------------------------

        public HostingUnit FindUnit(int num)
        {
            List<HostingUnit> lst= loadListFromXML<HostingUnit>(UnitsRootPath);
            int index = lst.FindIndex(item => item.HostingUnitKey == num);
            if (index == -1)
            {
                throw new ArgumentException("There is no such a hosting unit");//
            }
            return getHostingUnits()[index];
        }

        //-----------------------------------------------------------------

        public GuestRequest FindRequest(int num)
        {
            List<GuestRequest> lst = loadListFromXML<GuestRequest>(RequestsRootPath);
            int index = lst.FindIndex(item => item.GuestRequestKey == num);
            if (index == -1)
            {
                throw new ArgumentException("There is no such a guest request");
            }
            return getGuestRequests()[index];
        }
        //---------------------------------------------------------------------------------------
        public int getUnitsConfig()
        {

            string ConfigRootPath = @"Config.xml";
            try
            {
                XElement ConfigRoot = XElement.Load(ConfigRootPath);
                int config = int.Parse(ConfigRoot.Element("hostingUnitKey").Value);
                config++;
                ConfigRoot.Element("hostingUnitKey").Value = Convert.ToString(config);
                ConfigRoot.Save(ConfigRootPath);
                return config;
            }
            catch { throw new KeyNotFoundException("File upload problem"); }

        }
        public int getOrdersConfig()
        {
            string ConfigRootPath = @"Config.xml";
            try
            {
                XElement ConfigRoot = XElement.Load(ConfigRootPath);
                int config = int.Parse(ConfigRoot.Element("orderKey").Value);
                config++;
                ConfigRoot.Element("orderKey").Value = Convert.ToString(config);
                ConfigRoot.Save(ConfigRootPath);
                return config;
            }
            catch { throw new KeyNotFoundException("File upload problem"); }
        }
        public int getRequestsConfig()
        {
            string ConfigRootPath = @"Config.xml";
            try
            {
                XElement ConfigRoot = XElement.Load(ConfigRootPath);
                int config = int.Parse(ConfigRoot.Element("guestRequestKey").Value);
                config++;
                ConfigRoot.Element("guestRequestKey").Value = Convert.ToString(config);
                ConfigRoot.Save(ConfigRootPath);
                return config;
            }
            catch { throw new KeyNotFoundException("File upload problem"); }
        }

        private BankBranch ConvertBankBranch(XElement element)
        {
            return new BankBranch()
            {
                BankName = element.Element("שם_בנק").Value,
                BankNumber = int.Parse(element.Element("קוד_בנק").Value),       
                BranchNumber = int.Parse(element.Element("קוד_סניף").Value),
                BranchAddress = element.Element("כתובת_ה-ATM").Value,
                BranchCity = element.Element("ישוב").Value
            };
        }

        public List<string> getAllBankBranches()
        {

           var v=from item in banksRoot.Elements()
                    let a = ConvertBankBranch(item)
                    select a;

            List<string> BankNames = new List<string>();
            foreach (var item in v)
            {
                BankNames.Add(item.BankName + " " + item.BranchNumber);
            }
            List<string> result = new List<string>();
            result = BankNames.Distinct().ToList();
            result = result.Where(item => item.StartsWith("\n") == false).Select(item => item).ToList();
            return result;
        }

        public List<BankBranch> getBankBranches()
        {

            var v = from item in banksRoot.Elements()
                    let a = ConvertBankBranch(item)
                    select a;
            return v.ToList();
        }

        void downloadAtm()
        {
            XElement atm;
            try
            {
                atm = new XElement("atm");
                atm = XElement.Load("https://drive.google.com/u/0/uc?id=1FpcqslnRD6naLHOjrCvKArCg3Ihkb9hR&export=download");
                atm.Save(banksPath);
            }
            catch (Exception)
            {
                atm = new XElement("atm");
                atm = XElement.Load("http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml");
                atm.Save(banksPath);
            }
        }

    }
}
