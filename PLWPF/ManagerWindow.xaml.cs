using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        List<GuestRequest> lst = new List<GuestRequest>();
        List<HostingUnit> lst2=new List<HostingUnit>();
        List<BE.Order> lst3 = new List<BE.Order>();
        IBL myBL;
        
        public ManagerWindow()
        {

            myBL = BLFactory.getBL();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            lst = myBL.getGuestRequests();
            lst2 = myBL.getHostingUnits();
            lst3 = myBL.getOrders();
            guests.ItemsSource = lst;
            orders.ItemsSource = lst3;
            units.ItemsSource = lst2;

        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(myBL.mostRequestedArea().ToString(),"Information",MessageBoxButton.OK, MessageBoxImage.Information,MessageBoxResult.OK,MessageBoxOptions.RightAlign);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(myBL.numOfApprovedOrders().ToString(), "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(myBL.mostRequestedTypeOfUnit().ToString(), "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(myBL.CheapestHostingUnit().ToString(), "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                               MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }
        }

    }
    
}
