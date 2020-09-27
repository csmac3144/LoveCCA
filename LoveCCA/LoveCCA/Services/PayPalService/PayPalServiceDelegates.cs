using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Services.PayPalService
{
    public class PayPalNonceObtainedEventArgs : EventArgs
    {
        public bool IsSuccessful { get; set; }
        public PayPalAccountNonceObtainedResult Result { get; set; }
    }

}
