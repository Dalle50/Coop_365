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
    /// [Author] Daniel
    /// </summary>
    public partial class CheckOutPointsCheck : Window
    {
        /// <summary>
        /// dbAccessor to write into the database
        /// Customer to set current customer
        /// phoneNumberExist to confirm in checkout after window is closed
        /// registrationSuccessful to confirm a registration has been made, if window is registered to be closed ( error handling )
        /// </summary>
        DbAccessor dbAccessor = new DbAccessor();
        Customer customer;
        private double points;
        public bool phoneNumberExist;
        private bool registrationSuccessful = false;
        public CheckOutPointsCheck(double points)
        {
            InitializeComponent();
            this.points = points;
        }

        /// <summary>
        /// Enter phonenumber
        /// Controls if number is valid format
        /// Checks if it phonenumber exist in database
        /// If it does it retrieves the customer, and asks if customer wants to pay with their points
        /// If they dont ask for registratrion with options yes no
        /// If they choose to they will be prompted with register window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        dbAccessor.UpdateCustomerPoints(phoneNumber, customer.coopPoints + CheckOut.ConvertItemsToPoints(MainWindow.theOrder.GetOrderLines()));
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
                        Register register = new Register(phoneNumber);
                        register.Show();
                        register.Closed += Register_Closed;

                        register.CheckOutPointsCheckWindow = this;

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
        /// <summary>
        /// Event handler when Register window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Closed(object sender, EventArgs e)
        {
            if (registrationSuccessful)
            {
                phoneNumberExist = true;
                // Close the current window when the Register window is closed and registration was successful
                Close();
            }
        }
        public bool RegistrationSuccessful
        {
            get { return registrationSuccessful; }
            set { registrationSuccessful = value; }
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
        private void phoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text + e.Text;
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Cancel the input
            }

            if (newText.Length > 8)
            {
                e.Handled = true; // Cancel the input
            }
        }
    }
}
