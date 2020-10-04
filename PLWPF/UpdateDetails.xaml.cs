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
    /// Interaction logic for UpdateDetails.xaml
    /// </summary>
    public partial class UpdateDetails : Window
    {
        IBL myBL;
        HostingUnit hu1;
        int index2;
        public UpdateDetails(HostingUnit hu, int index)
        {
            myBL = BLFactory.getBL();
            hu1 = hu;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.area.ItemsSource = Enum.GetValues(typeof(BE.area));
            this.type.ItemsSource = Enum.GetValues(typeof(BE.type));
            UpdateGrid.DataContext = hu1;
            index2 = index;


        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                hu1.Jacuzzi = jac.IsChecked.Value;
                if ((hu1.HostingUnitName.Length == 0) || !(hu1.HostingUnitName.All(x => x == ' ' || char.IsLetter(x)) ||
                   (adults1.Text.Length == 0)) || (children1.Text.Length == 0))

                {
                    MessageBox.Show("Oops! You forgot to fill some of the details", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    return;
                }
                else
                     myBL.updateHostingUnit(hu1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                               MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }
            MessageBox.Show(hu1.ToString());
            PrivateArea p = new PrivateArea(myBL.FindHost(hu1.Owner.password));
            this.Close();
            p.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateUnit U = new UpdateUnit(myBL.FindHost(hu1.Owner.password));
            this.Close();
            U.ShowDialog();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
