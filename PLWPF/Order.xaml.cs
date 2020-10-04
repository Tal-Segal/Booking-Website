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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        IBL myBL;
        int num1;
        List<GuestRequest> lst;
        List<HostingUnit> lst2;
        List<BE.Order> lst3 = new List<BE.Order>();
        int index2;
        public Order(int index)
        {
            myBL = BLFactory.getBL();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            lst = myBL.getGuestRequests();
            lst2 = myBL.getHostingUnits();
            lst3 = myBL.getOrders();
            index2 = index;
            guests.ItemsSource = lst;
            orders.ItemsSource = lst3;
            guests.Items.Refresh();
        }
       
        private void guests_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (guests.SelectedItem != null)
            {
                guests.CommitEdit();
                guests.CommitEdit();
                orders.CommitEdit();
                orders.CommitEdit();
                
                AddOrder Window = new AddOrder(((GuestRequest)guests.SelectedItem),index2);
                Window.ShowDialog();
                lst = myBL.getGuestRequests();
                lst3 = myBL.getOrders();
                guests.ItemsSource = lst;
                orders.ItemsSource = lst3;
                
                guests.Items.Refresh();
                orders.Items.Refresh();
                guests.CancelEdit();
                guests.CancelEdit();
                orders.CancelEdit();
                orders.CancelEdit();
            }
        }

        private void orders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (orders.SelectedItem != null)
            {
                orders.CommitEdit();
                orders.CommitEdit();
                orders.CommitEdit();
                orders.CommitEdit();
                if (myBL.orderIsClosed((BE.Order)orders.SelectedItem))
                {
                    MessageBox.Show("This order was already being closed!", "Error", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    return;
                }
                OrderStatus os = new OrderStatus((BE.Order)orders.SelectedItem);
                os.ShowDialog();
                lst = myBL.getGuestRequests();
                lst3 = myBL.getOrders();
                guests.ItemsSource = lst;
                orders.ItemsSource = lst3;
                guests.Items.Refresh();
                orders.Items.Refresh();
                guests.CancelEdit();
                guests.CancelEdit();
                orders.CancelEdit();
                orders.CancelEdit();

            }

        }
        private void north_Checked(object sender, RoutedEventArgs e)
        {
            //areaGrouping
            if (num.Text == "")
            {
                if (north.IsChecked == true)
                {
                    lst = myBL.areaGrouping(area.North);
                    guests.ItemsSource = lst;
                }
                else
                {
                    lst = myBL.getGuestRequests();
                    guests.ItemsSource = lst;
                }
            }
            else north.IsChecked = false;
        }

        private void south_Checked(object sender, RoutedEventArgs e)
        {
            //areaGrouping
            if (num.Text == "")
            {
                if (south.IsChecked == true)
                {
                    lst = myBL.areaGrouping(area.South);
                    guests.ItemsSource = lst;
                }
                else
                {
                    lst = myBL.getGuestRequests();
                    guests.ItemsSource = lst;
                }
            }
            else south.IsChecked = false;
        }

        private void jerusalem_Checked(object sender, RoutedEventArgs e)
        {
            //areaGrouping
            if (num.Text == "")
            {
                if (jerusalem.IsChecked == true)
                {
                    lst = myBL.areaGrouping(area.Jerusalem);
                    guests.ItemsSource = lst;
                }
                else
                {
                    lst = myBL.getGuestRequests();
                    guests.ItemsSource = lst;
                }
            }
            else jerusalem.IsChecked = false;
        }

        private void center_Checked(object sender, RoutedEventArgs e)
        {
            //areaGrouping
            if (num.Text == "")
            {
                if (center.IsChecked == true)
                {
                    lst = myBL.areaGrouping(area.Center);
                    guests.ItemsSource = lst;
                }
                else
                {
                    lst = myBL.getGuestRequests();
                    guests.ItemsSource = lst;
                }
            }
            else center.IsChecked = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //numOfVisitorsGrouping
            if (!(north.IsChecked == false && center.IsChecked == false && jerusalem.IsChecked == false && south.IsChecked == false))
            {
                num.Text = "";
                return;
            }
            //else
            if (num.Text != "")
            {
                lst = myBL.numOfVisitorsGrouping(Convert.ToInt32(num.Text));
                guests.ItemsSource = lst;

            }
            else //num.Text==""
            {
                lst = myBL.getGuestRequests();
                guests.ItemsSource = lst;
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();                
        }


    }
}