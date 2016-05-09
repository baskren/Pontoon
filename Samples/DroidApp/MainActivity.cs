﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using InTheHand.UI.Popups;
using System.Threading.Tasks;

namespace DroidApp
{
    [Activity(Label = "DroidApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {            
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            InTheHand.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += MainActivity_DataRequested;
            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            InTheHand.Storage.ApplicationData.Current.LocalSettings.Values.MapChanged += Values_MapChanged;
            InTheHand.Storage.ApplicationData.Current.LocalSettings.Values.Add("MyNewTest", "cheese");
            InTheHand.Storage.ApplicationData.Current.LocalSettings.Values["MyNewTest"] = "bread";
            InTheHand.Storage.ApplicationData.Current.LocalSettings.Values.Remove("MyNewTest");

            button.Click += new EventHandler(async (s,e)=> { button.Text = string.Format("{0} clicks!", count++);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.DisplayName);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.FullName);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Name);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.Id.Version);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.InstalledDate);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.IsDevelopmentMode);
                System.Diagnostics.Debug.WriteLine(InTheHand.ApplicationModel.Package.Current.PublisherDisplayName);

                InTheHand.UI.Popups.MessageDialog md = new MessageDialog("message","title");
                md.Commands.Add(new UICommand("One", (c) => { System.Diagnostics.Debug.WriteLine("One"); }));
                md.Commands.Add(new UICommand("Two", (c) => { System.Diagnostics.Debug.WriteLine("Two"); }));
                await md.ShowAsync();
                

               

                // InTheHand.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
                //Task.Run(async () =>
                //{
                    /*MessageDialog md = new MessageDialog("Hello Android");
                    MessageDialog.Context = this;
                     md.ShowAsync();*/

                //System.Diagnostics.Debug.WriteLine(InTheHand.g)

                /*InTheHand.ApplicationModel.Email.EmailMessage msg = new InTheHand.ApplicationModel.Email.EmailMessage();
                msg.Subject = "Cheesy Subject";
                msg.Body = "Body text";
                msg.To.Add(new InTheHand.ApplicationModel.Email.EmailRecipient("peter@inthehand.com"));
                InTheHand.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(msg);*/
                    //System.Diagnostics.Debug.WriteLine("Hello Android Dismissed");
                //});
            });

            InTheHand.UI.Notifications.ToastNotifier n = InTheHand.UI.Notifications.ToastNotificationManager.CreateToastNotifierForApplication();
            var notification = InTheHand.UI.Notifications.ToastNotificationCreator.CreateToastNotification("content", "title");
            n.Show(notification);
        }

        private void Values_MapChanged(InTheHand.Foundation.Collections.IObservableMap<string, object> sender, InTheHand.Foundation.Collections.IMapChangedEventArgs<string> eventArgs)
        {
            System.Diagnostics.Debug.WriteLine(eventArgs.CollectionChange.ToString() + " " + eventArgs.Key);
        }

        private void MainActivity_DataRequested(object sender, InTheHand.ApplicationModel.DataTransfer.DataRequestedEventArgs e)
        {
            e.Request.Data.SetWebLink(new Uri("http://peterfoot.net"));
            e.Request.Data.Properties.Title = "Pete's Website";
        }
    }
}

