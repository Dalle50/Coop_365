using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// [Author] Daniel
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = Navn.Text;
            string address = Adresse.Text;
            int zipCode = int.Parse(zip.Text);
            string by = city.Text;
            string mail = email.Text;
            int phone = int.Parse(phoneNumber.Text);
            string zipValue = await CheckPostalCode(int.Parse(zip.Text));
            if (zip.Text.Length != 4 || zipValue == "Error")
            {
                MessageBox.Show("Forkert zipcode indtastet. Der blev skrevet"+ zipCode+" men skal skrives som = 8800");
            }
            if(phoneNumber.Text.Length != 8)
            {
                MessageBox.Show("Telefonnummer er ikke korrekt længde. Prøv det skal f.eks. 80808080"+ "Du skrev : "+ phoneNumber.Text);
            }
            if(zip.Text.Length == 4 && phoneNumber.Text.Length == 8 && zipValue != "Error") 
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
        private async void zipCheck_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Cancel the input
                city.Text = "";
            }
            else if (char.IsDigit(e.Text, e.Text.Length - 1)) // Check if the length is 4 digits
            {
                TextBox textBox = sender as TextBox;
                string newText = textBox.Text + e.Text;
                int zipCode;
                if(newText.Length == 4)
                {

                    string stringZipCode = await CheckPostalCode(int.Parse(newText));
                    if(stringZipCode != null && stringZipCode != "Error")
                    {
                        city.Text = stringZipCode;
                    }
                    else
                    {
                        MessageBox.Show("Enter a valid zipcode");
                    }


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
        private async Task<string> CheckPostalCode(int zipCode)
        {
            string returnPostalCode = "Error";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://api.dataforsyningen.dk/postnumre?nr=" + zipCode;

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        // You can use a JSON parser to deserialize the response
                        // into objects or use dynamic typing to access the fields directly.
                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        Console.WriteLine(data);
                        if (data != null && data.Count > 0)
                        {
                            string navn = data[0]["navn"].ToString(); // Access the "navn" field
                            Console.WriteLine($"Navn: {navn}");
                            returnPostalCode = navn;
                        }
                        else
                        {
                            Console.WriteLine("Ikke gyldigt postnummer.");
                        }
                    }
                    else
                    {
                        returnPostalCode = "Error";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return returnPostalCode;
        }
    }
}
