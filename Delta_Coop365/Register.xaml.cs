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
        double points = 0;
        public Register(double points, int phone)
        {
            InitializeComponent();
            this.points = points;
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
                    dbAccessor.InsertIntoKunder(name, address, zipCode, by, mail, phone, points);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("En kunde med det telefonnummer eksisterer allerede.");
                }
            }

        }
    }
}
