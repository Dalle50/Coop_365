using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
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
using static System.Net.WebRequestMethods;

namespace Delta_Coop365
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// [Author] Daniel
    /// </summary>
    public partial class Register : Window
    {
        DbAccessor dbAccessor = new DbAccessor();
        // Makes it possible to check if registration is successful
        public CheckOutPointsCheck CheckOutPointsCheckWindow { get; set; }
        /// <summary>
        /// Constructs registry window with the phone number entered before
        /// </summary>
        /// <param name="phone"></param>
        public Register(int phone)
        {
            InitializeComponent();
            phoneNumber.Text = phone.ToString();
        }
        /// <summary>
        /// When register is entered saves the entered values into variables and validates the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = Navn.Text;
            string address = Adresse.Text;
            string by = city.Text;
            string mail = email.Text;
            int phone = int.Parse(phoneNumber.Text);


            //If lenght or non exisiting så giver den en besked
            if (!int.TryParse(zip.Text, out int zipCode) || zip.Text.Length != 4)
            {
                MessageBox.Show("Forkert zipcode indtastet. Der skal skrives fire cifre (f.eks. 8800).");
                return;
            }
            string zipValue = await CheckPostalCode(int.Parse(zip.Text));
            if (phoneNumber.Text.Length != 8)
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
        /// <summary>
        /// Asynchronous controls the zipcode and control that the entered valie is a number
        /// Checks last entered letter and if its a character it throws the character away
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void zipCheck_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Cancel the input
                city.Text = "";
            }
            else if (char.IsDigit(e.Text, e.Text.Length - 1)) 
            {
                TextBox textBox = sender as TextBox;
                string newText = textBox.Text + e.Text;
                int zipCode;
                // Check if the length is 4 digits
                if (newText.Length == 4)
                {

                    string zipCodeToCity = await CheckPostalCode(int.Parse(newText));
                    if(zipCodeToCity != null && zipCodeToCity != "Error")
                    {
                        city.Text = zipCodeToCity;
                    }
                    else
                    {
                        MessageBox.Show("Enter a valid zipcode");
                    }


                }
            }
        }
        /// <summary>
        /// Controls the phonenumber to prevent letters to be entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberCheck_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Cancel the input
            }
        }
        /// <summary>
        /// Async task that recieves a zipCode for validation
        /// Holds the zipcode up against a danish API that contains all the cities of denmark.
        /// If succesful HttpRequest it reads the data async, since loading in data can pause for a while
        /// After validating the data it returns the city of the zipCode again if data is valid.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        private async Task<string> CheckPostalCode(int zipCode)
        {
            string returnCity = "Error";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://api.dataforsyningen.dk/postnumre?nr=" + zipCode;

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        Console.WriteLine(data);
                        if (data != null && data.Count > 0)
                        {
                            string navn = data[0]["navn"].ToString(); 
                            Console.WriteLine($"Navn: {navn}");
                            returnCity = navn;
                        }
                        else
                        {
                            Console.WriteLine("Ikke gyldigt postnummer.");
                        }
                    }
                    else
                    {
                        returnCity = "Error";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return returnCity;
        }
    }
}
