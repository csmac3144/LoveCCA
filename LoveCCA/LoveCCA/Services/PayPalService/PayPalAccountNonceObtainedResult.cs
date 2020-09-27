using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Services.PayPalService
{
    public class PayPalAccountNonceObtainedResult
    {
        public string Nonce { get; set; }
        public string Phone { get; set; }
        public string PayerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public PayPalPostalAddress BillingAddress { get; set; }
        public string AuthenticateUrl { get; set; }
        public PayPalPostalAddress ShippingAddress { get; set; }
        public string ClientMetadataId { get; set; }
    }
}
