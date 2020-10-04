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
    /// Interaction logic for DeleteDetails.xaml
    /// </summary>
    public partial class DeleteDetails : Window
    {
        IBL myBL;
        HostingUnit hu1 = new HostingUnit();
        public DeleteDetails(int unitKey)
        {
            myBL = BLFactory.getBL();
            
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.area.ItemsSource = Enum.GetValues(typeof(BE.area));
            this.type.ItemsSource = Enum.GetValues(typeof(BE.type));
            hu1 = myBL.FindUnit(unitKey);
            deleteGrid.DataContext = hu1;
           

        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                hu1.Jacuzzi = jac.IsChecked.Value;
                int hostKey = myBL.FindHost(hu1.Owner.password);
                myBL.deleteHostingUnit(hu1);
                MessageBox.Show("Hosting unit was deleted", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                PrivateArea p = new PrivateArea(hostKey);
                this.Close();
                p.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "שגיאה", MessageBoxButton.OK,
                        MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PrivateArea p = new PrivateArea(myBL.FindHost(hu1.Owner.password));
            this.Close();
            p.ShowDialog();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
