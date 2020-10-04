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
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        IBL myBL;
        BE.Order o;
        int index2;
        public AddOrder(GuestRequest gr, int index)
        { 
            myBL = BL.BLFactory.getBL();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            o = new BE.Order()
            {
                GuestRequestKey = gr.GuestRequestKey,
                Fee = 0,
                
                CreateDate =DateTime.Now,
                Status=RequestStatus.Open
            };
            grid.DataContext = o;
            this.status.ItemsSource = Enum.GetValues(typeof(BE.RequestStatus));
            index2 = index;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (unitkey.Text.Length==0)
                {
                    MessageBox.Show("Oops! You forgot to fill some of the details", "Error", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    return;
                }
            try
            {
                myBL.addOrder(o);
                this.Close();
                MessageBox.Show(o.ToString(), "Order was succesfully added" ,MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PLWPF.Order o = new PLWPF.Order(index2);
            this.Close();
            o.ShowDialog();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
