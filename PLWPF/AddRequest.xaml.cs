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
    /// Interaction logic for AddRequest.xaml
    /// </summary>
    public partial class AddRequest : Window
    {
        IBL myBL;
        GuestRequest myReq;
        public AddRequest()
        {
            myBL = BLFactory.getBL();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            myReq = new GuestRequest()
            {
                GuestRequestKey = myBL.getRequestsConfig(),
                RegistrationDate = DateTime.Now,
                EntryDate = DateTime.Now,
                ReleaseDate = DateTime.Now
            };

            MyGrid.DataContext = myReq;
            jaccuzi.SelectedItem = Preference.Must;

            this.area.ItemsSource = Enum.GetValues(typeof(BE.area));
            this.type.ItemsSource = Enum.GetValues(typeof(BE.type));
            this.pool.ItemsSource = Enum.GetValues(typeof(BE.Preference));
            this.garden.ItemsSource = Enum.GetValues(typeof(BE.Preference));
            this.attractions.ItemsSource = Enum.GetValues(typeof(BE.Preference));
            this.jaccuzi.ItemsSource = Enum.GetValues(typeof(BE.Preference));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if ((pri.Text.Length == 0) || !(pri.Text.All(x => x == ' ' || char.IsLetter(x))) ||
                (family.Text.Length == 0) || !(family.Text.All(x => x == ' ' || char.IsLetter(x)))
                || (mail.Text.Length == 0) || Convert.ToInt32(adults.Text) == 0 || (Convert.ToInt32(max.Text) <= 0)
                || (Convert.ToInt32(min.Text) <= 0))
            {
                MessageBox.Show("Oops! You forgot to fill some of the details", "Error", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                return;
            }
            else
            {
                if (!(myReq.MailAddress.EndsWith("@gmail.com") || myReq.MailAddress.EndsWith("@walla.com")))
                {
                    MessageBox.Show("Mail address is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    return;
                }
                if ((Convert.ToInt32(min.Text) > (Convert.ToInt32(max.Text))))
                {
                    MessageBox.Show("Incorrect range of prices", "Error", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    return;
                }
                try
                {
                    myBL.addClientRequest(myReq);
                    MessageBox.Show(myReq.ToString(), "Request was added successfully", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                   MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                }

            }
        }
   
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}

 
