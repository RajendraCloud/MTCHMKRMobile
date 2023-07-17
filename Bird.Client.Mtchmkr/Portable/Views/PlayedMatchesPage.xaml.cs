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
    public partial class PlayedMatchesPage : ContentPage
    {
        public PlayedMatchesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new PlayedMatchesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as PlayedMatchesViewModel).Init();
        }
    }
}