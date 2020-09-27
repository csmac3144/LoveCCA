using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Services.PayPalService
{
    public class PayPalPostalAddress
    {
        public string Region { get; set; }
        public string RecipientName { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Locality { get; set; }
        public bool IsEmpty { get; set; }
        public string ExtendedAddress { get; set; }
        public string CountryCodeAlpha2 { get; set; }
        public string SortingCode { get; set; }
        public string StreetAddress { get; set; }
    }
}
