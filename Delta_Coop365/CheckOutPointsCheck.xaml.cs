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
        public bool phoneNumberExist;
        private double points;
        DbAccessor dbAccessor = new DbAccessor();
        Customer customer;
        public CheckOutPointsCheck(double points)
        {
            InitializeComponent();
            this.points = points;
        }


        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            int phoneNumber = Int32.Parse(phoneNumberTextBox.Text);
            Console.WriteLine(phoneNumber.ToString().Length);
            if (phoneNumber.ToString().Length == 8)
            {
                bool exists = dbAccessor.IsCustomerExisting(phoneNumber);
                if (exists)
                {
                    phoneNumberExist = true;
                    //Point system implemented here that adds points to customer.
                    customer = dbAccessor.GetCustomer(phoneNumber);
                    CheckOut.customer = customer;
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Vil du betale med dine points? Du har "+customer.coopPoints+" som svarer til: "+ConvertPointsToCash(customer), "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        double remainingPoints = 0.0;
                        double pointsToCash = ConvertPointsToCash(customer);
                        if(pointsToCash > CheckOut.order.GetPrice())
                        {
                            remainingPoints = pointsToCash - CheckOut.order.GetPrice();
                            dbAccessor.UpdateCustomerPoints(phoneNumber, remainingPoints * 100);
                            pointsToCash = pointsToCash - remainingPoints;
                            CheckOut.order.AddDiscount(pointsToCash);
                            Close();
                        }
                        else
                        {
                            CheckOut.order.AddDiscount(pointsToCash);
                            dbAccessor.UpdateCustomerPoints(phoneNumber, 0);
                            Close();
                        }

                        CheckOut.order.AddDiscount(ConvertPointsToCash(customer));
                    }
                    else if(messageBoxResult == MessageBoxResult.No)
                    {
                        dbAccessor.UpdateCustomerPoints(phoneNumber, 0);
                        Close();
                    }
                }
                else 
                {
                    Console.WriteLine("Opening another window to ask if they'd like to be registered....");
                    //Open the window here to register.
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Vil du registreres i systemet?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        Register register = new Register(points ,phoneNumber);
                        register.Show();

                    }
                    else if (messageBoxResult == MessageBoxResult.No)
                    {
                        Close();
                    }
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
            phoneNumberExist = false;
            Close();
        }
        private double ConvertPointsToCash(Customer c)
        {
            return c.coopPoints / 100;
        }
    }
}
