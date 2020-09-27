using Com.Braintreepayments.Api;
using Com.Braintreepayments.Api.Interfaces;
using Com.Braintreepayments.Api.Models;
using LoveCCA.Droid.Droid.Services;
using LoveCCA.Services.PayPalService;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(PayPalService))]
namespace LoveCCA.Droid.Droid.Services
{

    public class PayPalService : IPayPalService
    {
        private string _amount;
        BraintreeFragment _braintreeFragment;
        public event EventHandler<PayPalResult> OnPayPalResult;

        public PayPalService()
        {
            var activity = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            _braintreeFragment = BraintreeFragment.NewInstance((AndroidX.Fragment.App.FragmentActivity)activity, "sandbox_pgtyn3vw_b57nx3qgs2q5d3kj");
            var listener = new PaymentMethodNonceCreatedListener();
            _braintreeFragment.AddListener(listener);
            listener.OnPayPalNonceResult += Listener_OnPayPalNonceResult;
        }

        private void Listener_OnPayPalNonceResult(object sender, PayPalNonceObtainedEventArgs e)
        {
            //TODO: call checkout

            OnPayPalResult(this, new PayPalResult {
                Nonce = e.Result.Nonce,
                Amount = _amount,
                IsSuccessful = true, TransactionID = "",
                PayPalAccountNonceObtainedResult = e.Result });
        }

        public void StartCheckout(string amount, string displayName, string currency = "CAD")
        {
            _amount = amount;
            PayPalRequest request = new PayPalRequest(amount)
                .InvokeCurrencyCode(currency)
                .InvokeDisplayName(displayName)
                .InvokeIntent(PayPalRequest.IntentAuthorize);


            PayPal.RequestOneTimePayment(_braintreeFragment, request);

        }
    }

    public class PaymentMethodNonceCreatedListener : BraintreeFragment, IPaymentMethodNonceCreatedListener
    {
        public event EventHandler<PayPalNonceObtainedEventArgs> OnPayPalNonceResult;

        public void OnPaymentMethodNonceCreated(PaymentMethodNonce paymentMethodNonce)
        {
            String nonce = paymentMethodNonce.Nonce;

            if (string.IsNullOrEmpty(nonce))
            {
                this.OnPayPalNonceResult(this, new PayPalNonceObtainedEventArgs { IsSuccessful = false });
            }

            if (paymentMethodNonce is PayPalAccountNonce)
            {
                PayPalAccountNonce payPalAccountNonce = (PayPalAccountNonce)paymentMethodNonce;

                var args = new PayPalNonceObtainedEventArgs
                {
                    IsSuccessful = true,
                    Result = new PayPalAccountNonceObtainedResult
                    {
                        Nonce = payPalAccountNonce.Nonce,
                        Email = payPalAccountNonce.Email,
                        FirstName = payPalAccountNonce.FirstName,
                        LastName = payPalAccountNonce.LastName,
                        Phone = payPalAccountNonce.Phone,
                        ClientMetadataId = payPalAccountNonce.ClientMetadataId,
                        PayerId = payPalAccountNonce.PayerId,
                        AuthenticateUrl = payPalAccountNonce.AuthenticateUrl,
                        BillingAddress = new PayPalPostalAddress
                        {
                            CountryCodeAlpha2 = payPalAccountNonce.BillingAddress.CountryCodeAlpha2,
                            PhoneNumber = payPalAccountNonce.BillingAddress.PhoneNumber,
                            PostalCode = payPalAccountNonce.BillingAddress.PostalCode,
                            RecipientName = payPalAccountNonce.BillingAddress.RecipientName,
                            Region = payPalAccountNonce.BillingAddress.Region,
                            SortingCode = payPalAccountNonce.BillingAddress.SortingCode,
                            StreetAddress = payPalAccountNonce.BillingAddress.StreetAddress,
                            ExtendedAddress = payPalAccountNonce.BillingAddress.ExtendedAddress,
                            Locality = payPalAccountNonce.BillingAddress.Locality
                        },
                        ShippingAddress = new PayPalPostalAddress
                        {
                            CountryCodeAlpha2 = payPalAccountNonce.ShippingAddress.CountryCodeAlpha2,
                            PhoneNumber = payPalAccountNonce.ShippingAddress.PhoneNumber,
                            PostalCode = payPalAccountNonce.ShippingAddress.PostalCode,
                            RecipientName = payPalAccountNonce.ShippingAddress.RecipientName,
                            Region = payPalAccountNonce.ShippingAddress.Region,
                            SortingCode = payPalAccountNonce.ShippingAddress.SortingCode,
                            StreetAddress = payPalAccountNonce.ShippingAddress.StreetAddress,
                            ExtendedAddress = payPalAccountNonce.ShippingAddress.ExtendedAddress,
                            Locality = payPalAccountNonce.ShippingAddress.Locality
                        }
                    }
                };
                this.OnPayPalNonceResult(this, args);
            }
            else
            {
                this.OnPayPalNonceResult(this, new PayPalNonceObtainedEventArgs { IsSuccessful = false });
            }
        }
    }
}