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
    /// Interaction logic for DeleteUnit.xaml
    /// </summary>
    public partial class DeleteUnit : Window
    {
        IBL myBL;
        string num;
        HostingUnit hu;
        int index2;
        public DeleteUnit(int index)
        {
            myBL = BLFactory.getBL();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            index2 = index;
            DeleteGrid.DataContext = num;
            hu = new HostingUnit();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            num = txt.Text;
            try
            {
                hu = myBL.FindUnit(Convert.ToInt32(num));
                if (hu.Owner.ID == myBL.getHostingUnits()[index2].Owner.ID)
                {
                    DeleteDetails d = new DeleteDetails(hu.HostingUnitKey);
                    this.Close();
                    d.ShowDialog();
                }
                else
                {
                    MessageBox.Show("This hosting unit is not yours!", "Error", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PrivateArea p = new PrivateArea(index2);
            this.Close();
            p.ShowDialog();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
