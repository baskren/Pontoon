﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using Microsoft.Phone.Controls;
using Windows.Storage;
using System.IO;
using InTheHand.UI.Popups;
using InTheHand.ApplicationModel;
using InTheHand.UI.ApplicationSettings;

namespace InTheHandUI.ApplicationSettings
{
    internal partial class SettingsPage : PhoneApplicationPage
    {
        private IList<SettingsCommand> commands;

        public SettingsPage()
        {
            InitializeComponent();

            if(Application.Current.RootVisual is Microsoft.Phone.Controls.TransitionFrame)
            {
                NavigationInTransition transitionIn = new NavigationInTransition() { Backward = new TurnstileFeatherTransition { Mode = TurnstileFeatherTransitionMode.BackwardIn }, Forward = new SwivelTransition { Mode = SwivelTransitionMode.ForwardIn } };
                NavigationOutTransition transitionOut = new NavigationOutTransition() { Backward = new SwivelTransition { Mode = SwivelTransitionMode.BackwardOut }, Forward = new SwivelTransition { Mode = SwivelTransitionMode.ForwardOut } };

                this.SetValue(Microsoft.Phone.Controls.TransitionService.NavigationInTransitionProperty, transitionIn);
                this.SetValue(Microsoft.Phone.Controls.TransitionService.NavigationOutTransitionProperty, transitionOut);
            }

            Loaded += SettingsPage_Loaded;
        }

        async void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(Package.Current.Logo);
            using (var stream = await file.OpenStreamForReadAsync())
            {
                BitmapImage bmp = new BitmapImage();
                bmp.CreateOptions = BitmapCreateOptions.None;
                bmp.SetSource(stream);

                WriteableBitmap wb = new WriteableBitmap(bmp);
                int rgb = wb.Pixels[0];

                byte r, g, b;
                r = (byte)((rgb & 0xFF0000) >> 16);
                g = (byte)((rgb & 0xFF00) >> 8);
                b = (byte)(rgb & 0xFF);

                Color chrome = (Color)Application.Current.Resources["PhoneBackgroundColor"];
                r = (byte)((int)(r + (3 * chrome.R)) / 4);
                g = (byte)((int)(g + (3 * chrome.G)) / 4);
                b = (byte)((int)(b + (3 * chrome.B)) / 4);
                Color c = Color.FromArgb(0xff, r, g, b);
                SolidColorBrush brush = new SolidColorBrush(c);
                LayoutRoot.Background = brush;
                Microsoft.Phone.Shell.SystemTray.BackgroundColor = c;

            }
                SettingsHeader.Text = "Settings".ToLower();

            AppNameText.Text = Package.Current.DisplayName.ToUpper();

            if (SettingsPane.GetForCurrentView().showPublisher)
            {
                AuthorText.Text = string.Format("By {0}", Package.Current.PublisherDisplayName);
            }
            else
            {
                AuthorText.Visibility = Visibility.Collapsed;
            }

            Version.Text = string.Format("Version {0}", Package.Current.Id.Version.ToString(4));
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            commands = SettingsPane.GetForCurrentView().OnCommandsRequested();

            if (commands == null)
            {
                commands = new List<SettingsCommand>();
            }

            // for store distribution include rate and review
#if DEBUG
            if(true)
#else
            if (!Package.Current.IsDevelopmentMode)
#endif
            {
                commands.Add(new SettingsCommand("RateAndReview", "RateAndReview", RateAndReviewSelected));

                if (SettingsPane.GetForCurrentView().showPublisher)
                {
                    commands.Add(new SettingsCommand("PrivacyPolicy", "PrivacyPolicy", async (c) =>
                        {
                            await InTheHand.ApplicationModel.Store.CurrentApp.RequestDetailsAsync();
                        }));
                }
            }

            SettingsList.ItemsSource = commands;

            base.OnNavigatedTo(e);
        }

        void RateAndReviewSelected(IUICommand command)
        {
            Microsoft.Phone.Tasks.MarketplaceReviewTask reviewTask = new Microsoft.Phone.Tasks.MarketplaceReviewTask();
            reviewTask.Show();
        }

        private void SettingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsList.SelectedItem != null)
            {
                ((SettingsCommand)SettingsList.SelectedItem).Invoked((SettingsCommand)SettingsList.SelectedItem);
            }
        }
    }
}