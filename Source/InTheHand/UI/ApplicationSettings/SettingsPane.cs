﻿//-----------------------------------------------------------------------
// <copyright file="SettingsPane.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using InTheHand.ApplicationModel;
using InTheHand.Storage;
using InTheHand.Foundation;
using InTheHand.UI.Popups;
#if WINDOWS_UWP || WINDOWS_PHONE_APP
using InTheHandUI.ApplicationSettings;
#endif

#if __IOS__ || __TVOS__
using Foundation;
using UIKit;
#elif WINDOWS_PHONE
using System.Windows;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace InTheHand.UI.ApplicationSettings
{
    /// <summary>
    /// A static class that enables the app to control the Settings page.
    /// The app can add or remove commands, receive a notification when the user opens the pane, or open the page programmatically.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
    /// </remarks>
    public sealed class SettingsPane
    {
        private static SettingsPane instance;
#if WINDOWS_UWP
        private static bool hasSettingsPane;
#endif
        /// <summary>
        /// Gets a <see cref="SettingsPane"/> object that is associated with the current app.
        /// </summary>
        /// <returns></returns>
        public static SettingsPane GetForCurrentView()
        {
            if (instance == null)
            {
#if WINDOWS_UWP
                hasSettingsPane = Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.UI.ApplicationSettings.ApplicationsSettingsContract", 1);
#endif
                instance = new SettingsPane();
            }

            return instance;
        }

        internal bool showPublisher = true;
        internal bool showAbout = false;

        private SettingsPane()
        {         
        }

        /// <summary>
        /// Displays the Settings page to the user.
        /// </summary>
        public static void Show()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("InTheHand.UI.ApplicationSettings.ShowPublisher"))
            {
                if (!(bool)ApplicationData.Current.LocalSettings.Values["InTheHand.UI.ApplicationSettings.ShowPublisher"])
                {
                    GetForCurrentView().showPublisher = false;
                }
            }

            object objAbout = null;
            if(ApplicationData.Current.LocalSettings.Values.TryGetValue("InTheHand.UI.ApplicationSettings.ShowAbout", out objAbout))
            {
                GetForCurrentView().showAbout = (bool)objAbout;
            }

#if __ANDROID__
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(typeof(SettingsActivity));
#elif __IOS__ || __TVOS__
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
#elif WINDOWS_UWP
            if(hasSettingsPane)
            {
#pragma warning disable 618
                Windows.UI.ApplicationSettings.SettingsPane.Show();
            }
            else
            {
                Frame f = Window.Current.Content as Frame;
                if (f != null)
                {
                    f.Navigate(typeof(SettingsPage));
                }
            }
#elif WINDOWS_APP
            Windows.UI.ApplicationSettings.SettingsPane.Show();
#elif WINDOWS_PHONE_APP
            Frame f = Window.Current.Content as Frame;
            if (f != null)
            {
                f.Navigate(typeof(SettingsPage));
            }
#elif WINDOWS_PHONE
            ((Microsoft.Phone.Controls.PhoneApplicationFrame)Application.Current.RootVisual).Navigate(new Uri("/InTheHand;component/UI/ApplicationSettings/SettingsPage.SL.xaml", UriKind.Relative));
#else
            throw new PlatformNotSupportedException();
#endif
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        internal IList<SettingsCommand> OnCommandsRequested()
        {
            SettingsPaneCommandsRequestedEventArgs e = new SettingsPaneCommandsRequestedEventArgs();
            try
            {
                if (_commandsRequested != null)
                {
                    _commandsRequested(this, e);
                    return e.Request.ApplicationCommands;
                }
            }
            catch { }

            return null;
        }
#endif
        private event TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs> _commandsRequested;
        /// <summary>
        /// Occurs when the user opens the settings pane.
        /// Listening for this event lets the app initialize the setting commands and pause its UI until the user closes the pane.
        /// During this event, append your SettingsCommand objects to the available ApplicationCommands vector to make them available to the SettingsPane UI.
        /// </summary>
        public event TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs> CommandsRequested
        {
            add
            {
#if WINDOWS_UWP
                if (hasSettingsPane)
                {
#endif
#if WINDOWS_UWP || WINDOWS_APP
                    if (_commandsRequested == null)
                    {
#pragma warning disable 618
                        Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView().CommandsRequested += SettingsPane_CommandsRequested;
                    }
#endif
#if WINDOWS_UWP
                }
#endif
                _commandsRequested += value;
            }
            remove
            {
                _commandsRequested -= value;
#if WINDOWS_UWP
                if (hasSettingsPane)
                {
#endif
#if WINDOWS_UWP || WINDOWS_APP
                    if (_commandsRequested == null)
                    {
                        Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView().CommandsRequested -= SettingsPane_CommandsRequested;
                    }
#endif
#if WINDOWS_UWP
                }
#endif
            }
        }

#if WINDOWS_UWP || WINDOWS_APP

        private void SettingsPane_CommandsRequested(Windows.UI.ApplicationSettings.SettingsPane sender, Windows.UI.ApplicationSettings.SettingsPaneCommandsRequestedEventArgs args)
        {
            foreach(SettingsCommand cmd in OnCommandsRequested())
            {
#pragma warning disable 618
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand(cmd.Id, cmd.Label, new Windows.UI.Popups.UICommandInvokedHandler((c)=> { SettingsCommand sc = new SettingsCommand(c.Id, c.Label, cmd.Invoked ); sc.Invoked.Invoke(sc); })));
            }

#if WINDOWS_UWP
            // add missing 8.1 commands
#if DEBUG
            if(true)
#else
            if(!Package.Current.IsDevelopmentMode)
#endif
            {

#pragma warning disable 618
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand("rateAndReview", "Rate and review", async (c) =>
                {
                    await InTheHand.ApplicationModel.Store.CurrentApp.RequestReviewAsync();
                }));
#pragma warning disable 618
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand("privacyPolicy", "Privacy policy", async (c) =>
                {
                    await InTheHand.ApplicationModel.Store.CurrentApp.RequestDetailsAsync();
                }));
            }


#endif
            /*if (showAbout)
            {
                args.Request.ApplicationCommands.Add(new Windows.UI.ApplicationSettings.SettingsCommand("about", "About", (c) =>
                {
                    SettingsFlyout flyout = new SettingsFlyout();
                    flyout.Title = "About";
                    //flyout.IconSource = new BitmapImage(InTheHand.ApplicationModel.Package.Current.Logo);
                    flyout.Content = new InTheHandUI.AboutView();
                    flyout.Show();
                }));
            }*/
        }
#endif
    }
}