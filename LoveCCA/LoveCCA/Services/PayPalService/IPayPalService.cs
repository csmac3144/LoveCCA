using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Services.PayPalService
{
    public interface IPayPalService
    {
        event EventHandler<PayPalResult> OnPayPalResult;
        void StartCheckout(string amount, string displayName, string currency = "CAD");
    }
}
