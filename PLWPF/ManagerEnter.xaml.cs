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
    /// Interaction logic for ManagerEnter.xaml
    /// </summary>
    public partial class ManagerEnter : Window
    {
        public ManagerEnter()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            logIn.IsEnabled = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (pass.Password == Configuration.managerPassword)
            {
                ManagerWindow m = new ManagerWindow();
                this.Close();
                m.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are an impostor!!! Shame on you!", "Alarm", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                this.Close();
                return;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (robot.IsChecked == true)
            {
                logIn.IsEnabled = true;
            }
            else
            {
                logIn.IsEnabled = false;
            }
        }
    }
}
