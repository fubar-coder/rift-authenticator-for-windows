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
    public partial class CreateAuthenticator : PhoneApplicationPage
    {
        public CreateAuthenticator()
        {
            InitializeComponent();
        }

        private void InitPageUI()
        {
#if !WP70
            ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
#endif
            if (ApplicationBar.Buttons.Count == 0)
            {
                var appBarButton = new Microsoft.Phone.Shell.ApplicationBarIconButton(new Uri("/Images/appbar.feature.settings.rest.png", UriKind.Relative))
                {
                    Text = WP7.Resources.AppResource.ShowDeviceId,
                };
                appBarButton.Click += ShowDeviceId_Click;
                ApplicationBar.Buttons.Add(appBarButton);
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitPageUI();

            SetDeviceIdVisibility(System.Windows.Visibility.Collapsed);

            if (App.AppStartedNormally)
            {
                App.AuthCreateDeviceId = Library.TrionServer.GetOrCreateRandomDeviceId();
                App.AuthCreateDescription = App.CreateDefaultAccountDescription();
            }
            AuthDescription.Text = App.AuthCreateDescription ?? string.Empty;
            DeviceId.Text = App.AuthCreateDeviceId ?? string.Empty;
        }

        private string AuthDeviceId
        {
            get
            {
                return (string.IsNullOrEmpty(DeviceId.Text) ? Library.TrionServer.GetOrCreateRandomDeviceId() : DeviceId.Text);
            }
        }

        private void AuthCreate_Click(object sender, RoutedEventArgs e)
        {
            AuthCreate.IsEnabled = false;
            var deviceId = AuthDeviceId;
            var description = AuthDescription.Text;
            var account = App.CreateNewAccountObject();
            Library.TrionServer.BeginCreateSecurityKey((ar) =>
            {
                try
                {
                    Library.TrionServer.EndCreateSecurityKey(ar);
                    account.Description = description;
                    App.AddNewAccountObject(account);
                    App.ExecuteTimeSync(Dispatcher, () =>
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            App.ExitApp = false;
                            App.BackToMainPage = true;
                            App.AuthConfigReset();
                            NavigationService.GoBack();
                            //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        });
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(ex.Message, WP7.Resources.AppResource.MessageBoxTitleError, MessageBoxButton.OK);
                        AuthCreate.IsEnabled = true;
                    });
                }
            }, null, account, deviceId);
        }

        private void ShowDeviceId_Click(object sender, EventArgs e)
        {
            var isHidden = DeviceId.Visibility == System.Windows.Visibility.Collapsed;
            var newVisibility = (isHidden ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
            SetDeviceIdVisibility(newVisibility);
        }

        private void SetDeviceIdVisibility(System.Windows.Visibility visibility)
        {
            DeviceIdLabel.Visibility = DeviceId.Visibility = DeviceIdWarning.Visibility = DeviceIdWithHelp.Visibility = DeviceIdHelp.Visibility = visibility;
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
            App.AuthCreateDeviceId = DeviceId.Text;
        }
    }
}