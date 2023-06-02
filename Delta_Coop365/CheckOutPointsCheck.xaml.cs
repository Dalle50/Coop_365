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

namespace Delta_Coop365
{
    /// <summary>
    /// Interaction logic for CheckOutPointsCheck.xaml
    /// </summary>
    public partial class CheckOutPointsCheck : Window
    {
        public bool phoneNumberExsist;
        public CheckOutPointsCheck()
        {
            InitializeComponent();
        }


        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            int phoneNumber = phoneNumberTextBox.Text.Length;
            if (phoneNumber == 8 && phoneNumber > 0)
            {
                CheckPhoneNumber();
                if (CheckPhoneNumber() == true)
                {
                    phoneNumberExsist = true;
                    //Point system implemented here that adds points to customer.
                }
                if (CheckPhoneNumber() == false)
                {
                    Console.WriteLine("Opening another window to ask if they'd like to be registered....");
                    //Open the window here to register.
                    phoneNumberExsist = false;
                }
            }
            else
            {
                Console.WriteLine($"Telefon nummeret er ikke gyldigt. bruger instastede: {phoneNumberTextBox.Text}");
                invalidNumber.Content = "Ikke gyldigt telefon nummer.";
            }
        }
        private void UndoClick(object sender, RoutedEventArgs e)
        {
            phoneNumberExsist = false;
            Close();
        }
        private bool CheckPhoneNumber()
        {
            int phoneNumber = int.Parse(phoneNumberTextBox.Text);
            DbAccessor dbAccessor= new DbAccessor();
            if (dbAccessor.IsCustomerExisting(phoneNumber) == true)
            {
                dbAccessor.GetCustomerPoints(phoneNumber);
                phoneNumberExsist = true;
                return true;
            }
            return false;
        }
    }
}
