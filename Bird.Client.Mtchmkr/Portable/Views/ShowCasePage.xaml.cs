﻿using Bird.Client.Mtchmkr.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bird.Client.Mtchmkr.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowCasePage : ContentPage
    {
        public ShowCasePage()
        {
            InitializeComponent();
            BindingContext = new ShowCaseViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (this.BindingContext as ShowCaseViewModel).GetShowCaseData();
        }

        //void Button_Clicked(System.Object sender, System.EventArgs e)
        //{
        //    Shell.Current.FlyoutIsPresented = !Shell.Current.FlyoutIsPresented;
        //}
    }
}