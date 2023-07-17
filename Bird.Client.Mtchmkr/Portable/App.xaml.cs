//using Bird.Client.Mtchmkr.Portable.Services;
//using Bird.Client.Mtchmkr.Portable.Views;
using Bird.Client.Mtchmkr.Business.Common;
using Bird.Client.Mtchmkr.Business.Repositories;
using Bird.Client.Mtchmkr.Business.ServiceCenter.Response;
using Bird.Client.Mtchmkr.Portable.Common;
using Bird.Client.Mtchmkr.Portable.Interfaces;
using Bird.Client.Mtchmkr.Portable.Models;
using Bird.Client.Mtchmkr.Portable.Views;
using Plugin.FirebasePushNotification;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bird.Client.Mtchmkr.Portable
{
    public partial class App : Application
    {
        private static App m_App;
        private static bool m_IsUserLoggedIn = false;
        private static bool m_IsUserAdmin = false;
        public static MtchmkrAppRepository ServiceManager { get; private set; }

        public static bool IsUserLoggedIn
        {
            get { return m_IsUserLoggedIn; }
            //set
            //{
            //    if (m_IsUserLoggedIn != value)
            //        m_IsUserLoggedIn = value;           
            //}
        }
        public static async Task<bool> Login(string userName, string password)
        {
            return true;
        }
        public static async Task<bool> LogOut()
        {
            return true;
        }
        public static void LoadUser(ProfileModel profile)
        {
            
            if (null != profile)
            {
               // m_App.Profile = profile;
                m_IsUserLoggedIn = true;
                m_IsUserAdmin = profile.IsAdmin;
                m_App.LoadShell();
            }
        }
        public App()
        {
            InitializeComponent();
            ServiceManager = new MtchmkrAppRepository();
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
           

            m_App = this;
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(SearchSettingsPage), typeof(SearchSettingsPage));
            Routing.RegisterRoute(nameof(RequestSettingsPage), typeof(RequestSettingsPage));
            Routing.RegisterRoute(nameof(GamesTypesPage), typeof(GamesTypesPage));
            Routing.RegisterRoute(nameof(PlayedMatchesPage), typeof(PlayedMatchesPage));
            Routing.RegisterRoute(nameof(RequestsPage), typeof(RequestsPage));
            Routing.RegisterRoute(nameof(DashBoardPage), typeof(DashBoardPage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));

            LoadShell();

        }
        public static async void SendFCMToken()
        {
            var userId = Preferences.Get(Constants.UserID, string.Empty);
            if (string.IsNullOrEmpty(userId))
                return;
#if DEBUG
            var token = "xcvcgdjdgjshdkkjhdkjshdkshkhs";
#else
            var token = CrossFirebasePushNotification.Current.Token;
            if (string.IsNullOrEmpty(token))
                token = await CrossFirebasePushNotification.Current.GetTokenAsync();
#endif

            
            if (string.IsNullOrEmpty(token))
                return;
            FcmDeviceInfo requestFcmInfo = new FcmDeviceInfo();
            requestFcmInfo.deviceId = DependencyService.Get<IBaseUrl>().GetIdentifier();
            requestFcmInfo.deviceToken = token;
            requestFcmInfo.deviceType = DeviceInfo.Platform.ToString();
            requestFcmInfo.userId = Guid.Parse(userId);
            requestFcmInfo.createdDate = DateTime.Now;
            var res = await App.ServiceManager.InsertFCMInfoAsync(requestFcmInfo);
            if(res)
            {
                Debug.WriteLine("Device info inserted");
            }
            else
            {
                Debug.WriteLine("Failed inserting device info");
            }
        }

        public static void AdminCheck()
        {
            if (!m_IsUserAdmin)
            {
                m_App.LoadShell();
            }
        }

        public static void StandardMode()
        {
            m_App.MainPage = new AppShell();
        }

        public static void LoadAdmin()
        {
            if (!IsUserLoggedIn)
            {
                m_App.MainPage = new LoginShell();
            }
            else
            {
                m_App.MainPage = new AdminShell();
            }
        }
        private static PasswordConstraint m_PasswordConstraint = new PasswordConstraint
        {
            AllowNull=false,
            MinimumLength = 8,
            RequiresLowerCase = true,
            RequiresUpperCase = true,
            RequiresSpecialCharacter = true,
            RequiresNumber = true
        };
        public static PasswordConstraint PasswordConstraint
        {
            get => m_PasswordConstraint;
        }
        public void LoadShell()
        {
            if(!Preferences.ContainsKey(Constants.UserID))
            {
                MainPage = new LoginShell();
            }
            else
            {
                MainPage = new AppShell();
            }
        }

        protected override void OnStart()
        {

            // Handle when your app starts
            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += async (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
                SendFCMToken();
            };
            System.Diagnostics.Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");

            CrossFirebasePushNotification.Current.OnNotificationReceived += async(s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body"))
                    {
                        
                    }

                    if (p.Data.ContainsKey("gcm.notification.match_id"))
                    {
                        var gameName = "Unknown" as object;
                        p.Data.TryGetValue("gcm.notification.game_name", out gameName);
                        var userName = "Unknown" as object;
                        p.Data.TryGetValue("gcm.notification.user_name", out userName);

                        var msgBody = "Unknown" as object;
                        p.Data.TryGetValue("body", out msgBody);

                        var id = await App.Current.MainPage.DisplayActionSheet(msgBody.ToString(), "cancel", null, new string[] { "Accept", "Reject" });

                        var matchId = p.Data["gcm.notification.match_id"].ToString();

                        await ActionOnMatchRequest(id, matchId);
                    }
                }
                catch (Exception ex)
                {
                }

            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += async(s, p) =>
            {
                //System.Diagnostics.Debug.WriteLine(p.Identifier);

                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (p.Data.ContainsKey("gcm.notification.match_id"))
                {
                    var gameName = "Unknown" as object;
                    p.Data.TryGetValue("gcm.notification.game_name", out gameName);
                    var userName = "Unknown" as object;
                    p.Data.TryGetValue("gcm.notification.user_name", out userName);

                    var msgBody = "Unknown" as object;
                    p.Data.TryGetValue("body", out msgBody);
                    var id = await App.Current.MainPage.DisplayActionSheet(msgBody.ToString(), "cancel", null, new string[] { "Accept", "Reject" });

                    var matchId = p.Data["gcm.notification.match_id"].ToString();

                    await ActionOnMatchRequest(id, matchId);
                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += async (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }
                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }
                    if (p.Data.ContainsKey("gcm.notification.match_id"))
                    {
                        var gameName = "Unknown" as object;
                        p.Data.TryGetValue("gcm.notification.game_name", out gameName);
                        var userName = "Unknown" as object;
                        p.Data.TryGetValue("gcm.notification.user_name", out userName);

                        var msgBody = "Unknown" as object;
                        p.Data.TryGetValue("body", out msgBody);
                        var id = await App.Current.MainPage.DisplayActionSheet(msgBody.ToString(), "cancel", null, new string[] { "Accept", "Reject" });

                        var matchId = p.Data["gcm.notification.match_id"].ToString();

                        await ActionOnMatchRequest(id, matchId);
                    }
                }

            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };

        }

        
        private async Task ActionOnMatchRequest(string actionName, string matchId)
        {
            switch (actionName)
            {
                case "Accept":
                    var res = await App.ServiceManager.AcceptOrRejectMTCH(Guid.Parse(matchId), true, Preferences.Get("UserId", string.Empty));
                    if (res)
                    {
                        await App.Current.MainPage.DisplayAlert("Mtchmakr", "Accepted successfully!", "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Mtchmakr", "Accepting Failed!", "OK");
                    }
                    break;
                case "Reject":
                    var res1 = await App.ServiceManager.AcceptOrRejectMTCH(Guid.Parse(matchId), false, Preferences.Get("UserId", string.Empty));
                    if (res1)
                    {
                        await App.Current.MainPage.DisplayAlert("Mtchmakr", "Rejected successfully!", "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Mtchmakr", "Rejecting Failed!", "OK");
                    }
                    break;
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Handle the unhandled exception here
            Exception exception = e.ExceptionObject as Exception;
            HandleException(exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // Handle the unobserved task exception here
            Exception exception = e.Exception;
            HandleException(exception);
        }

        private async void HandleException(Exception exception)
        {
            await App.Current.MainPage.DisplayAlert("Exception", exception.Message, "Ok");
        }

    }
}
