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
    /// Interaction logic for HostWindow.xaml
    /// </summary>
    public partial class HostWindow : Window
    {
        IBL myBL;
        string myPassword;
        int id;
        public HostWindow()
        {
            myBL = BLFactory.getBL();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            HostGrid.DataContext = pass;
            HostGrid.DataContext = myID;
            signUp.IsEnabled = false;
            logIn.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            AddUnitWindow au = new AddUnitWindow(index);
            this.Close();
            au.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            myPassword = pass.Password;
            if(myID.Text.Length!=9)
            {
                MessageBox.Show("ID may contain nine numbers", "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                return;
            }
            int id;
            if (!int.TryParse(myID.Text, out id))
            {
                MessageBox.Show("ID may contain ONLY numbers", "Error", MessageBoxButton.OK,
                              MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                return;
            }
            else
            {
                int index = myBL.FindHost(myPassword);
                if (index == -1 || (Convert.ToInt32(myID.Text)) != myBL.getHostingUnits()[index].Owner.ID) //not existed  
                {
                    Exception ex = new KeyNotFoundException("Unexisted host");
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                  MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                }
                else
                {
                    PrivateArea pa = new PrivateArea(index);
                    this.Close();
                    pa.Show();
                }
            }
        }

        private void Button_MouseClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (robot.IsChecked==true)
            {
                logIn.IsEnabled = true;
                signUp.IsEnabled = true;
            }
            else
            {
                logIn.IsEnabled = false;
                signUp.IsEnabled = false;
            }
        }

        private void Button_MouseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
