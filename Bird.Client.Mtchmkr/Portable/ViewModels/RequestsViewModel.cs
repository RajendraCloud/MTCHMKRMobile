using Bird.Client.Mtchmkr.Business.ServiceCenter.Response;
using Bird.Client.Mtchmkr.Portable.Comon;
using Bird.Client.Mtchmkr.Portable.Interfaces;
using Bird.Client.Mtchmkr.Portable.Models;
using Bird.Client.Mtchmkr.Portable.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Bird.Client.Mtchmkr.Portable.ViewModels
{
    public class RequestsViewModel : BaseViewModel
    {
        private readonly IProgressDialog _progDialog;

        private ObservableCollection<PendingMatchResponse> pendingMatchSource;
        public ObservableCollection<PendingMatchResponse> PendingMatchSource
        {
            get
            {
                return pendingMatchSource;
            }
            set
            {
                pendingMatchSource = value;
                OnPropertyChanged();
            }
        }

        private bool isNoDataView = false;
        public bool IsNoDataView
        {
            get
            {
                return isNoDataView;
            }
            set
            {
                isNoDataView = value;
                OnPropertyChanged();
            }
        }

        private bool isVisibleList = false;
        public bool IsVisibleList
        {
            get
            {
                return isVisibleList;
            }
            set
            {
                isVisibleList = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlayNowCommand { get; }

        public RequestsViewModel() 
        {
            _progDialog = DependencyService.Get<IProgressDialog>();
            GetPendingMatchData();
            PlayNowCommand = new Command<PendingMatchResponse>(PlayMatchMethod);
        }

        public async void PlayMatchMethod(PendingMatchResponse obj)
        {
            var Item = obj as PendingMatchResponse;
            Preferences.Set("MatchId", Item.matchId.ToString());

            if (Item.matchDate >= DateTime.Now)
            {
                await App.Current.MainPage.DisplayAlert(Bird.Client.Mtchmkr.Business.Common.Constants.APP_NAME, "MTCH Can not be started befor MTCH date " + Item.matchDate.ToString("dd/MM/yyyy hh:mm tt"), "OK");
            }
            else
            {
                await App.Current.MainPage.Navigation.PushAsync(new ScoreCardPage(obj));
            }
        }

        public override void OnAppearing()
        {
            IsBusy = true;
            GetPendingMatchData();
        }


        public async void GetPendingMatchData()
        {
            _progDialog.ShowProgress("Loading...");
            var userId = Guid.Parse(Preferences.Get("UserId", string.Empty));
            var result = await App.ServiceManager.GetPendingMatchAsync(userId);
            if (result != null && result.Count > 0)
            {
                IsVisibleList = true;
                PendingMatchSource = new ObservableCollection<PendingMatchResponse>(result);
                IsNoDataView = false;
            }
            else
            {
                IsVisibleList = false;
                PendingMatchSource = new ObservableCollection<PendingMatchResponse>();
                IsNoDataView = true;
            }

            _progDialog.HideProgress();
        }
    }
}