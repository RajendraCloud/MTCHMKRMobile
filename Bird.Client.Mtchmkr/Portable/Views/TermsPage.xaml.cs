using Bird.Client.Mtchmkr.Portable.Interfaces;
using Bird.Client.Mtchmkr.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bird.Client.Mtchmkr.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsPage : ContentPage
    {
        UrlWebViewSource source;

        public TermsPage()
        {
            InitializeComponent();

            this.BindingContext = new TermsViewModel();

            //source = new UrlWebViewSource();
            //string url = DependencyService.Get<IBaseUrl>().Get();
            //string TempUrl = Path.Combine(url, "mmterms.html");

            //string baseUrl = DependencyService.Get<IBaseUrl>().Get();
            //string filePathUrl = Path.Combine(baseUrl, "mmterms.html");
            //source.Url = filePathUrl;

            //try
            //{
            //    ReadFileMethod("mmterms.html");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //webView.BackgroundColor = Color.Transparent;
            //webView.Source = source;

            string url = "https://mtchmkr.co.uk/terms-and-conditions";
            LoadUrl(url);
        }

        //async void ReadFileMethod(string fileName)
        //{
        //    using (var stream = await FileSystem.OpenAppPackageFileAsync(fileName))
        //    {
        //        using (var reader = new StreamReader(stream))
        //        {
        //            var fileContents = await reader.ReadToEndAsync();
        //            source.Html = fileContents;
        //        }
        //    }
        //}

        private void LoadUrl(string url)
        {
            webView.Source = new UrlWebViewSource { Url = url };
        }
    }
}