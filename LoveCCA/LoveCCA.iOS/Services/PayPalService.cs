using BraintreeCore;
using BraintreePayPal;
using Foundation;
using LoveCCA.iOS.Services;
using LoveCCA.Services.PayPalService;
using System;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PayPalService))]
namespace LoveCCA.iOS.Services
{
    //TODO: add url scheme!
    public class AppSwitchDelegate : BTAppSwitchDelegate
    {
        public override void AppSwitcher(NSObject appSwitcher, BTAppSwitchTarget target)
        {
            throw new NotImplementedException();
        }

        public override void AppSwitcherWillPerformAppSwitch(NSObject appSwitcher)
        {
            throw new NotImplementedException();
        }

        public override void AppSwitcherWillProcessPaymentInfo(NSObject appSwitcher)
        {
            throw new NotImplementedException();
        }
    }


    public class PresentingDelegate : BTViewControllerPresentingDelegate
    {
        public override void RequestsDismissalOfViewController(NSObject driver, UIViewController viewController)
        {
            throw new NotImplementedException();
        }

        public override void RequestsPresentationOfViewController(NSObject driver, UIViewController viewController)
        {
            throw new NotImplementedException();
        }
    }

    public class PayPalService : IPayPalService
    {
        BTAPIClient braintreeClient;

        public event EventHandler<PayPalResult> OnPayPalResult;

        public void StartCheckout()
        {

        }

        public void StartCheckout(string amount, string displayName, string currency = "CAD")
        {
            braintreeClient = new BTAPIClient(PayPalConfig.AUTHORIZATION);
            var payPalDriver = new BTPayPalDriver(braintreeClient);
            payPalDriver.ViewControllerPresentingDelegate = new PresentingDelegate();
            payPalDriver.AppSwitchDelegate = new AppSwitchDelegate(); // Optional

            var request = new BTPayPalRequest(amount);
            request.CurrencyCode = currency;
            request.DisplayName = displayName;

            payPalDriver.RequestOneTimePayment(request, (payPalAccountNonce, NSError) =>
            {
                if (payPalAccountNonce != null)
                {

                    var billingAddress = new PayPalPostalAddress
                    {
                        CountryCodeAlpha2 = payPalAccountNonce.BillingAddress.CountryCodeAlpha2,
                        PostalCode = payPalAccountNonce.BillingAddress.PostalCode,
                        RecipientName = payPalAccountNonce.BillingAddress.RecipientName,
                        Region = payPalAccountNonce.BillingAddress.Region,
                        StreetAddress = payPalAccountNonce.BillingAddress.StreetAddress,
                        ExtendedAddress = payPalAccountNonce.BillingAddress.ExtendedAddress,
                        Locality = payPalAccountNonce.BillingAddress.Locality
                    };
                    var shippingAddress = new PayPalPostalAddress
                    {
                        CountryCodeAlpha2 = payPalAccountNonce.ShippingAddress.CountryCodeAlpha2,
                        PostalCode = payPalAccountNonce.ShippingAddress.PostalCode,
                        RecipientName = payPalAccountNonce.ShippingAddress.RecipientName,
                        Region = payPalAccountNonce.ShippingAddress.Region,
                        StreetAddress = payPalAccountNonce.ShippingAddress.StreetAddress,
                        ExtendedAddress = payPalAccountNonce.ShippingAddress.ExtendedAddress,
                        Locality = payPalAccountNonce.ShippingAddress.Locality
                    };


                    OnPayPalResult(this, new PayPalResult
                    {
                        IsSuccessful = true,
                        Nonce = payPalAccountNonce.Nonce,
                        Amount = amount,
                        PayPalAccountNonceObtainedResult = new PayPalAccountNonceObtainedResult
                        {
                            Email = payPalAccountNonce.Email,
                            FirstName = payPalAccountNonce.FirstName,
                            LastName = payPalAccountNonce.LastName,
                            Phone = payPalAccountNonce.Phone,
                            ClientMetadataId = payPalAccountNonce.ClientMetadataId,
                            PayerId = payPalAccountNonce.PayerId,
                            BillingAddress = billingAddress,
                            ShippingAddress = shippingAddress,
                        }
                    });

                }
                else
                {
                    OnPayPalResult(this, new PayPalResult
                    {
                        IsSuccessful = false,
                        IsError = NSError != null
                    });
                }
            });
        }
    }
}