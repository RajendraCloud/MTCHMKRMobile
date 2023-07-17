using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Android.Content;
using Plugin.FirebasePushNotification;
using Plugin.GoogleClient;
using A = Android;
using Android.Content.Res;
using Android.Gms.Wallet;
using System.Collections.Generic;
using Java.Lang;

namespace Bird.Client.Mtchmkr.Android
{
    [Activity(Label = "MTCHMKR", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity ShareActivityContext;
        public const int LOAD_PAYMENT_DATA_REQUEST_CODE = 991;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
    
            base.OnCreate(savedInstanceState);
            ShareActivityContext = this;

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            GoogleClientManager.Initialize(this);
           // openGooglePay(this, 1);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            FirebasePushNotificationManager.ProcessIntent(this, Intent);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("AppDomain.CurrentDomain.UnhandledException: {0}. IsTerminating: {1}", e.ExceptionObject, e.IsTerminating);
            };

            AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("AndroidEnvironment.UnhandledExceptionRaiser: {0}. IsTerminating: {1}", e.Exception, e.Handled);
                e.Handled = true;
            };

            LoadApplication(new Bird.Client.Mtchmkr.Portable.App());
            

            
        }

        //public override Resources Resources
        //{
        //    get
        //    {
        //        var config = new Configuration();
        //        config.SetToDefaults();
        //        return CreateConfigurationContext(config).Resources;
        //    }
        //}

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, A.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            GoogleClientManager.OnAuthCompleted(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void openGooglePay(Activity act, int money)
        {
            PaymentsClient paymentsClient = WalletClass.GetPaymentsClient(
                 this,
                 new WalletClass.WalletOptions.Builder()
                         .SetEnvironment(WalletConstants.EnvironmentTest)
                         .Build()
            );

            TransactionInfo tran = TransactionInfo.NewBuilder()
                .SetTotalPriceStatus(WalletConstants.TotalPriceStatusFinal)
                .SetTotalPrice(money.ToString())
                .SetCurrencyCode("USD")
                .Build();

            var req = createPaymentDataRequest(tran);

            var futurePay = paymentsClient.LoadPaymentData(req);

            AutoResolveHelper.ResolveTask(futurePay, act, LOAD_PAYMENT_DATA_REQUEST_CODE);
        }

        PaymentDataRequest createPaymentDataRequest(TransactionInfo transactionInfo)
        {
            var paramsBuilder = PaymentMethodTokenizationParameters.NewBuilder()
                .SetPaymentMethodTokenizationType(
                WalletConstants.PaymentMethodTokenizationTypePaymentGateway)
                .AddParameter("gateway", "myGateway")
                .AddParameter("gatewayMerchantId", "myMerchant");

            return createPaymentDataRequest(transactionInfo, paramsBuilder.Build());
        }

        private PaymentDataRequest createPaymentDataRequest(TransactionInfo transactionInfo, PaymentMethodTokenizationParameters paymentMethodTokenizationParameters)
        {
            return PaymentDataRequest.NewBuilder()
                .SetPhoneNumberRequired(false)
                .SetEmailRequired(false)
                .SetShippingAddressRequired(false)
       .SetTransactionInfo(transactionInfo)
       .AddAllowedPaymentMethods(new List<Integer>() { (Integer)WalletConstants.PaymentMethodCard, (Integer)WalletConstants.PaymentMethodTokenizedCard })
       .SetCardRequirements(
           CardRequirements.NewBuilder()
               .AddAllowedCardNetworks(new List<Integer>() { (Integer)WalletConstants.CardNetworkVisa, (Integer)WalletConstants.CardNetworkMastercard })
               .SetAllowPrepaidCards(true)
               .SetBillingAddressFormat(WalletConstants.BillingAddressFormatFull)
               .Build()
       )
       .SetPaymentMethodTokenizationParameters(paymentMethodTokenizationParameters)
       .SetUiRequired(true)
       .Build();
        }

    }
}