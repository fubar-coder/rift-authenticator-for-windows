﻿/*
 * This file is part of RIFT™ Authenticator for Windows.
 *
 * RIFT™ Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT™ Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT™ Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace RiftAuthenticator.WP7
{
    public partial class AuthenticatorDescription : PhoneApplicationPage
    {
        private bool IsEdit { get; set; }

        public AuthenticatorDescription()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (App.BackToMainPage)
            {
                NavigationService.GoBack();
            }
            else
            {
                base.OnNavigatedTo(e);
            }
        }

        private void ShowDeviceId_Click(object sender, EventArgs e)
        {
            var isHidden = DeviceId.Visibility == System.Windows.Visibility.Collapsed;
            var newVisibility = (isHidden ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
            SetDeviceIdVisibility(newVisibility);
        }

        private void SetDeviceIdVisibility(System.Windows.Visibility visibility)
        {
            DeviceIdLabel.Visibility = DeviceId.Visibility = DeviceIdWithHelp.Visibility = visibility;
            if (!IsEdit)
                DeviceIdWarning.Visibility = DeviceIdHelp.Visibility = visibility;
        }

        private void InitPageUI()
        {
#if !WP70
            ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
#endif

            SetDeviceIdVisibility(System.Windows.Visibility.Collapsed);

            if (ApplicationBar.Buttons.Count == 0)
            {
                var appBarButton = new Microsoft.Phone.Shell.ApplicationBarIconButton(new Uri("/Images/appbar.feature.settings.rest.png", UriKind.Relative))
                {
                    Text = WP7.Resources.AppResource.ShowDeviceId,
                };
                appBarButton.Click += ShowDeviceId_Click;
                ApplicationBar.Buttons.Add(appBarButton);
            }

            if (this.NavigationContext.QueryString.ContainsKey("action"))
            {
                IsEdit = NavigationContext.QueryString["action"] == "edit";
            }
            if (IsEdit)
            {
                DeviceId.IsEnabled = false;
                LoadSecurityQuestions.Content = WP7.Resources.AppResource.Save;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitPageUI();

            if (!App.AppStartedNormally || App.AuthCreateStep > 1)
            {
                // Nothing to do here...
            }
            else if (IsEdit)
            {
                App.AuthCreateDescription = App.Account.Description;
                App.AuthCreateDeviceId = App.Account.DeviceId;
            }
            else
            {
                App.AuthCreateDescription = App.CreateDefaultAccountDescription();
                App.AuthCreateDeviceId = Library.TrionServer.GetOrCreateRandomDeviceId();
            }
            AuthDescription.Text = App.AuthCreateDescription ?? string.Empty;
            DeviceId.Text = App.AuthCreateDeviceId ?? string.Empty;
        }

        private void LoadSecurityQuestions_Click(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
            {
                App.AuthConfigReset();
                App.Account.Description = AuthDescription.Text;
                App.AccountManager.SaveAccounts();
                App.BackToMainPage = true;
                NavigationService.GoBack();
                //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                App.AuthCreateDeviceId = AuthDeviceId;
                App.AuthCreateDescription = AuthDescription.Text;
                NavigationService.Navigate(new Uri("/AccountLogin.xaml", UriKind.Relative));
            }
        }

        private string AuthDeviceId
        {
            get
            {
                return (string.IsNullOrEmpty(DeviceId.Text) ? Library.TrionServer.GetOrCreateRandomDeviceId() : DeviceId.Text);
            }
        }

        private void DeviceIdHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(WP7.Resources.AppResource.DeviceIdHelp);
        }

        private void AuthDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.AuthCreateDescription = AuthDescription.Text;
        }

        private void DeviceId_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.AuthCreateDeviceId = AuthDeviceId;
        }
    }
}