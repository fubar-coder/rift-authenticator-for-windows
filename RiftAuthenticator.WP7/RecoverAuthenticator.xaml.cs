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
    public partial class RecoverAuthenticator : PhoneApplicationPage
    {
        public RecoverAuthenticator()
        {
            InitializeComponent();
        }

        private void QuerySecurityQuestions_Click(object sender, RoutedEventArgs e)
        {
            var deviceId = (string.IsNullOrEmpty(DeviceId.Text) ? Library.TrionServer.GetOrCreateRandomDeviceId() : DeviceId.Text);

            App.AuthCreateUsername = UserName.Text;
            App.AuthCreatePassword = Password.Password;
            App.AuthCreateDeviceId = deviceId;

            Library.TrionServer.BeginGetSecurityQuestions((ar) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        App.SecurityQuestions = Library.TrionServer.EndGetSecurityQuestions(ar);
                        NavigationService.Navigate(new Uri("/SecurityQuestions.xaml", UriKind.Relative));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                    }
                });
            }, null, App.AuthCreateUsername, App.AuthCreatePassword);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            DeviceId.Text = Library.TrionServer.GetOrCreateRandomDeviceId();
        }

        private void ShowDeviceId_Click(object sender, EventArgs e)
        {
            var isHidden = DeviceId.Visibility == System.Windows.Visibility.Collapsed;
            var newVisibility = (isHidden ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
            DeviceIdLabel.Visibility = DeviceId.Visibility = newVisibility;
        }
    }
}