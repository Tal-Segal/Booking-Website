using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using BE;
using BL;
using System.Threading;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for OrderStatus.xaml
    /// </summary>
    public partial class OrderStatus : Window
    {
        IBL myBL;
        BE.Order o1;

      
        public OrderStatus(BE.Order o)
        {
            myBL = BL.BLFactory.getBL();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.status.ItemsSource = Enum.GetValues(typeof(BE.RequestStatus));
            
            o1 = o;
            grid.DataContext = o1;

        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.updateOrder(o1);
                GuestRequest g = myBL.FindRequest(o1.GuestRequestKey);
                g.Status = o1.Status;
                myBL.updateClientRequestStatus(g);
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += BackgroundWorker_DoWork;

                backgroundWorker.RunWorkerAsync();

                MessageBox.Show(o1.ToString(), "Order was saccessfully updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                this.Close();
            }

        }
        

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (o1.Status == RequestStatus.MailWasSent)
            {
                try
                {
                    myBL.sendMail(myBL.FindUnit(o1.HostingUnitKey).Owner.MailAddress, myBL.FindRequest(o1.GuestRequestKey).MailAddress);
                }
               catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                }
            }

        }
    }
}
