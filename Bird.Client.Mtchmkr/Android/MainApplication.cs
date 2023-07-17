﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.FirebasePushNotification;

namespace Bird.Client.Mtchmkr.Android
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();


            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }


            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, new NotificationUserCategory[]
            {
            new NotificationUserCategory("message",new List<NotificationUserAction> {
                new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground),
                new NotificationUserAction("Forward","Forward",NotificationActionType.Foreground)

            }),
            new NotificationUserCategory("request",new List<NotificationUserAction> {
                new NotificationUserAction("Accept","Accept",NotificationActionType.Default,"check"),
                new NotificationUserAction("Reject","Reject",NotificationActionType.Default,"cancel")
            })

            }, true);
#else
            FirebasePushNotificationManager.Initialize(this, new NotificationUserCategory[]
     {
                   new NotificationUserCategory("message",new List<NotificationUserAction> {
                       new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground),
                       new NotificationUserAction("Forward","Forward",NotificationActionType.Foreground)

                   }),
                   new NotificationUserCategory("request",new List<NotificationUserAction> {
                       new NotificationUserAction("Accept","Accept",NotificationActionType.Default,"check"),
                       new NotificationUserAction("Reject","Reject",NotificationActionType.Default,"cancel")
                   })

     }, false);

            //FirebasePushNotificationManager.Initialize(this, new NotificationUserCategory[]
            //{
            //new NotificationUserCategory("message",new List<NotificationUserAction> {
            //    new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
            //})
            //}, false);
#endif

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);
                
            };



        }
    }
}