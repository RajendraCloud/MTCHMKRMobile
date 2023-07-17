using System;
using Bird.Client.Mtchmkr.Android.DependencyServices;
using Bird.Client.Mtchmkr.Portable.Interfaces;
using Xamarin.Forms;
using A=Android;


[assembly: Dependency(typeof(BaseUrl_Android))]
namespace Bird.Client.Mtchmkr.Android.DependencyServices
{
    public class BaseUrl_Android : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }

        public string GetIdentifier()
        {
            return A.Provider.Settings.Secure.GetString(A.App.Application.Context.ContentResolver, A.Provider.Settings.Secure.AndroidId);
        }
    }
}