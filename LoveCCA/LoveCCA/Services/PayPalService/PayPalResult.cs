using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Services.PayPalService
{
    public class PayPalResult
    {
        public bool IsSuccessful { get; set; }
        public string Nonce { get; set; }
        public string Amount { get; set; }
        public string TransactionID { get; set; }
        public PayPalAccountNonceObtainedResult PayPalAccountNonceObtainedResult { get; set; }

    }
}
