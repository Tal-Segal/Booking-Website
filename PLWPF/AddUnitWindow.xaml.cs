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
using Microsoft.Win32;
using BE;
using BL;
using System.ComponentModel;
using System.Threading;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AddUnitWindow.xaml
    /// </summary>
    /// 
    public partial class AddUnitWindow : Window
    {
        IBL myBL;
        Host h;
        HostingUnit hu;
        int index2;
        BankBranch b;
        OpenFileDialog op; //for getting image input from user
        bool isImageChanged;
        public AddUnitWindow(int index)
        {
            myBL = BLFactory.getBL();
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            index2 = index;
            isImageChanged = false;
            h = new Host()
            {
                PrivateName = null,
                FamilyName = "",
                MailAddress = ""
            };
            
            
            hu = new HostingUnit()
            {
                HostingUnitKey = myBL.getUnitsConfig(),
                HostingUnitName = null
            };

            this.area.ItemsSource = Enum.GetValues(typeof(BE.area));
            this.type.ItemsSource = Enum.GetValues(typeof(BE.type));
            h.BankBranchDetails = b;
            hu.Owner = h;
            UnitDetails.DataContext = hu;
            PrivateDetails.DataContext = h;
            
            b = new BankBranch();
            List<string> Banks = myBL.getAllBankBranches();
            this.bank.ItemsSource = Banks;
            bank.DataContext = Banks;
           


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (index2 == -1)
            {
                HostWindow hw = new HostWindow();
                this.Close();
                hw.ShowDialog();
            }
            else
            {
                PrivateArea p = new PrivateArea(index2);
                this.Close();
                p.ShowDialog();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //BankNumber = int.Parse(element.Element("BankNumber").Value),
                //BankName = element.Element("BankName").Value,
                //BranchNumber = int.Parse(element.Element("BranchNumber").Value),
                //BranchAddress = element.Element("BranchAddress").Value,
                //BranchCity = element.Element("BranchCity").Value
                
                hu.Jacuzzi = jac.IsChecked.Value;
                if ((h.PrivateName.Length == 0) || !(h.PrivateName.All(x => x == ' ' || char.IsLetter(x))) ||
                    (h.FamilyName.Length == 0) || !(h.FamilyName.All(x => x == ' ' || char.IsLetter(x))) ||
                    (hu.HostingUnitName.Length == 0) || !(hu.HostingUnitName.All(x => x == ' ' || char.IsLetter(x))) ||
                    (h.password.Length == 0) || (h.MailAddress.Length == 0) || (id.Text=="0") || (phone2.Text.Length==0) || (adults1.Text.Length==0)
                        || (children1.Text.Length==0) || (accuont.Text.Length==0 )||(Convert.ToInt32(price1.Text)<=0))
                {
                    MessageBox.Show("Oops! You forgot to fill some of the details", "Error", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    return;
                }
                else
                {
                    if (!mailAddress.Text.EndsWith("@gmail.com")&& !mailAddress.Text.EndsWith("@walla.com"))
                    {
                        MessageBox.Show("Mail address is uncorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                        return;
                    }
                    if(id.Text.Length!=9)
                    {
                        MessageBox.Show("ID is too short", "Error", MessageBoxButton.OK, MessageBoxImage.Stop, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                        return;
                    }
                 else
                    {
                        if (myBL.checkHostID(h))
                        {
                            List<BankBranch> banks2 = myBL.getBankBranches();
                            var v = from n in myBL.getBankBranches()
                                    where n.BankName == bank.SelectedItem.ToString()
                                    select n;
                            b = v.FirstOrDefault();
                            h.BankBranchDetails = b;
                            hu.Owner = h;
                            myBL.addHostingUnit(hu);
                            MessageBox.Show(hu.ToString(),"Information",MessageBoxButton.OK,MessageBoxImage.Information,MessageBoxResult.Cancel,MessageBoxOptions.RightAlign);
                        }
                        else throw new ArgumentException("Uncorrect ID");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,
                                               MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }
         
        
            
            PrivateArea p = new PrivateArea(myBL.FindHost(h.password));
            this.Close();
            p.ShowDialog();
        }

        private void Button_UploadImage_Click(object sender, RoutedEventArgs e)
        {
            op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                isImageChanged = true;
                TesterImage.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
        
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
