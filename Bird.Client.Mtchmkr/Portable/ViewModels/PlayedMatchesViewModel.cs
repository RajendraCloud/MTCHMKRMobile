using Bird.Client.Mtchmkr.Business.Common;
using Bird.Client.Mtchmkr.Portable.Interfaces;
using Bird.Client.Mtchmkr.Portable.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Telerik.XamarinForms.Input.Calendar;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Bird.Client.Mtchmkr.Portable.ViewModels
{
    public class PlayedMatchesViewModel : BaseViewModel
    {
        private IProgressDialog _progDialog;
        private string _thisUserImageData;
        private string _thisUserName;

        public ObservableCollection<PlayedMatchModel> Matches { get; set; }

        private string _totalMatches;
        public string TotalMatches
        {
            get=>_totalMatches;
            set
            {
                _totalMatches = value;
                OnPropertyChanged();
            }
        }

        private string _winMatches;
        public string WinMatches
        {
            get => _winMatches;
            set
            {
                _winMatches = value;
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

        public PlayedMatchesViewModel()
        {
            _progDialog = DependencyService.Get<IProgressDialog>();
            Matches = new ObservableCollection<PlayedMatchModel>();
        }

        public async void Init()
        {
            await GetProfile();
            await GetMatches();
        }

        private async Task GetProfile()
        {
            try
            {
                _progDialog.ShowProgress("Loading...");
                var userId = Guid.Parse(Preferences.Get("UserId", string.Empty));
                var result = await App.ServiceManager.GetProfileByIdAsync(userId);
                if (result != null)
                {
                    _thisUserImageData = result.imageData;
                    _thisUserName = result.name;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(Constants.APP_NAME, "Somthing went wrong, please try again", "Ok");
                }
                _progDialog.HideProgress();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetMatches()
        {
            var uid = Preferences.Get(Constants.UserID, String.Empty);
            _progDialog.ShowProgress("Getting played MTCHs...");
            var res = await App.ServiceManager.GetPlayedMatches(uid);
            _progDialog.HideProgress();
            Matches.Clear();
            if(res!=null)
            {
                int totalWin = 0;
                foreach (var item in res)
                {
                    Matches.Add(new PlayedMatchModel
                    {
                        Player1 = item.name,
                        Player1Image = item.imageData != null ? Xamarin.Forms.ImageSource.FromStream(
                        () => new MemoryStream(Convert.FromBase64String(item.imageData))) : "",
                        Player2Image = _thisUserImageData != null ? Xamarin.Forms.ImageSource.FromStream(
                        () => new MemoryStream(Convert.FromBase64String(_thisUserImageData))) : "",
                        GameImage =  "",
                        Player2=_thisUserName,
                        Game=item.gameName,
                        Date=item.matchDate,
                        Player1Wins=item.frameWinCount,
                        Matches=item.maxFrame,
                        Player2Wins=item.myFrameWinCount
                    });

                    if(item.myFrameWinCount>item.frameWinCount)
                    {
                        totalWin++;
                    }
                }

                if (Matches.Count > 0)
                {
                    IsNoDataView = false;
                }
                else
                {
                    IsNoDataView = true;
                }

                TotalMatches = Matches.Count.ToString(); ;
                WinMatches = totalWin.ToString();
            }
        }
    }
}