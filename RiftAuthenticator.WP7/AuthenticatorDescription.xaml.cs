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
            DeviceId.Text = Library.TrionServer.GetOrCreateRandomDeviceId();
        }

        private void LoadSecurityQuestions_Click(object sender, RoutedEventArgs e)
        {
            var deviceId = (string.IsNullOrEmpty(DeviceId.Text) ? Library.TrionServer.GetOrCreateRandomDeviceId() : DeviceId.Text);
            App.AuthCreateDeviceId = deviceId;
            App.AuthCreateDescription = AuthDescription.Text;
            NavigationService.Navigate(new Uri("/RecoverAuthenticator.xaml", UriKind.Relative));
        }
    }
}