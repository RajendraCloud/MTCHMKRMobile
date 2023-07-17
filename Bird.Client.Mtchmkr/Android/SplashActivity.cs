using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Bird.Client.Mtchmkr.Portable.Models;
using Plugin.FirebasePushNotification;

namespace Bird.Client.Mtchmkr.Android
{
    [Activity(Label = "MTCHMKR", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true, Theme = "@style/splashscreen", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_splashscreen);

            await Task.Delay(1000);

            var mainActivityIntent = new Intent(this, typeof(MainActivity));

            var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            : PendingIntentFlags.UpdateCurrent;
            var pendingActivityIntent = PendingIntent.GetActivity(Application.Context, 1, mainActivityIntent, pendingIntentFlags);

            mainActivityIntent.AddFlags(ActivityFlags.NoAnimation); //Add this line
            StartActivity(mainActivityIntent);
        }

    }
}