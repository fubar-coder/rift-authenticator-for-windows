﻿using System;
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

        private void ShowDeviceId_Click(object sender, EventArgs e)
        {
            var isHidden = DeviceId.Visibility == System.Windows.Visibility.Collapsed;
            var newVisibility = (isHidden ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
            DeviceIdLabel.Visibility = DeviceId.Visibility = newVisibility;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("action"))
            {
                IsEdit = NavigationContext.QueryString["action"] == "edit";
            }
            if (IsEdit)
            {
                AuthDescription.Text = App.Account.Description;
                DeviceId.Text = App.Account.DeviceId;
                DeviceId.IsEnabled = false;
            }
            else
            {
                DeviceId.Text = Library.TrionServer.GetOrCreateRandomDeviceId();
            }

        }

        private void LoadSecurityQuestions_Click(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
            {
                App.Account.Description = AuthDescription.Text;
                App.AccountManager.SaveAccounts();
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                var deviceId = (string.IsNullOrEmpty(DeviceId.Text) ? Library.TrionServer.GetOrCreateRandomDeviceId() : DeviceId.Text);
                App.AuthCreateDeviceId = deviceId;
                App.AuthCreateDescription = AuthDescription.Text;
                NavigationService.Navigate(new Uri("/AccountLogin.xaml", UriKind.Relative));
            }
        }
    }
}