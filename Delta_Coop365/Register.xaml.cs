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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        DbAccessor dbAccessor = new DbAccessor();
        public CheckOutPointsCheck CheckOutPointsCheckWindow { get; set; }
        public Register(int phone)
        {
            InitializeComponent();
            phoneNumber.Text = phone.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = Navn.Text;
            string address = Adresse.Text;
            int zipCode = int.Parse(zip.Text);
            string by = city.Text;
            string mail = email.Text;
            int phone = int.Parse(phoneNumber.Text);
            if(zip.Text.Length != 4)
            {
                MessageBox.Show("Forkert zipcode indtastet. Der blev skrevet"+ zipCode+" men skal skrives som = 8800");
            }
            if(phoneNumber.Text.Length != 8)
            {
                MessageBox.Show("Telefonnummer er ikke korrekt længde. Prøv det skal f.eks. 80808080"+ "Du skrev : "+ phoneNumber.Text);
            }
            if(zip.Text.Length == 4 && phoneNumber.Text.Length == 8) 
            {
                if (!dbAccessor.IsCustomerExisting(phone))
                {
                    dbAccessor.InsertIntoKunder(name, address, zipCode, by, mail, phone, 0);
                    CheckOut.customer = dbAccessor.GetCustomer(phone);
                    if (CheckOutPointsCheckWindow != null)
                    {
                        CheckOutPointsCheckWindow.RegistrationSuccessful = true;
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("En kunde med det telefonnummer eksisterer allerede.");
                }
            }

        }
        private void CharCheck_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Cancel the input
            }
            else if(e.Text.Length == 4)
            {
                e.Handled = true;
            }
        }
        private void NumberCheck_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Cancel the input
            }
        }
    }
}
