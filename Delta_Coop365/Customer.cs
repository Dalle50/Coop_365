using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace Delta_Coop365
{
    /// <summary>
    /// [Author] Daniel
    /// </summary>
    public class Customer
    {
        public int KundeID { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public int zipCode { get; set; }
        public string city { get; set; }
        public string email { get; set; }
        public int phoneNumber { get; set; }
        public double coopPoints { get; set; }
        public Customer(int KundeID, string name, string address, int zipCode, string city, string email, int phoneNumber, double coopPoints)
        {
            this.KundeID = KundeID;
            this.name = name;
            this.address = address;
            this.zipCode = zipCode;
            this.city = city;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.coopPoints = coopPoints;
        }

    }
}
