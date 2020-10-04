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
    /// Interaction logic for PrivateArea.xaml
    /// </summary>
    public partial class PrivateArea : Window
    {
        IBL myBL;
        int index2;
        Host h;
        HostingUnit hu;
        string unitKey;
        public PrivateArea(int index)
        {
            myBL = BLFactory.getBL();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            index2 = index;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HostWindow h = new HostWindow();
            this.Close();
            h.ShowDialog();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((h.PrivateName.Length == 0) || !(h.PrivateName.All(x => x == ' ' || char.IsLetter(x))) ||
                    (h.FamilyName.Length == 0) || !(h.FamilyName.All(x => x == ' ' || char.IsLetter(x))) ||
                    (hu.HostingUnitName.Length == 0) || !(hu.HostingUnitName.All(x => x == ' ' || char.IsLetter(x))) ||
                    (h.password.Length == 0)|| (h.MailAddress.Length == 0) )
                {
                    MessageBox.Show("Oops! You forgot to fill some of the details", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    return;
                }
                else
                    myBL.addHostingUnit(hu);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK,
                                               MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }
            MessageBox.Show(hu.ToString());
        }
       
 

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AddUnitWindow a = new AddUnitWindow(index2);
            this.Close();
            a.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DeleteUnit d = new DeleteUnit(index2);
            this.Close();
            d.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UpdateUnit d = new UpdateUnit(index2);
            this.Close();
            d.ShowDialog();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Order o = new Order(index2);
            this.Close();
            o.Show();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            unitKey = annual.Text;
            MessageBox.Show(myBL.AnnualOccupancyPercent(myBL.FindUnit(Convert.ToInt32(unitKey))), "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
            return;
        }
    }



}
