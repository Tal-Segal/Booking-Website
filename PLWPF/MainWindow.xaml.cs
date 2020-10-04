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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL myBL;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            myBL = BLFactory.getBL();
            myBL.MonthThread();

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window HostWindow = new HostWindow();
            HostWindow.ShowDialog();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window RequestWindow = new AddRequest();
            RequestWindow.ShowDialog();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window M = new ManagerEnter();
            M.ShowDialog();

        }

    }
}
