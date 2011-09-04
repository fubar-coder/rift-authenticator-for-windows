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

        private string CreateDefaultAccountDescription()
        {
            var activeAccounts = App.AccountManager.Where(x => !x.IsEmpty).Count();
            if (activeAccounts == 0)
            {
                return "Default";
            }
            else
            {
                var accountIndex = activeAccounts + 1;
                string accountDescription;
                while (App.AccountManager.FindAccount((accountDescription = string.Format("Account {0}", accountIndex))) != null)
                {
                    ++accountIndex;
                }
                return accountDescription;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            AuthDescription.Text = CreateDefaultAccountDescription();
            DeviceId.Text = Library.TrionServer.GetOrCreateRandomDeviceId();
        }

        private void AuthCreate_Click(object sender, RoutedEventArgs e)
        {
            var deviceId = (string.IsNullOrEmpty(DeviceId.Text) ? Library.TrionServer.GetOrCreateRandomDeviceId() : DeviceId.Text);
            var description = AuthDescription.Text;
            var account = App.CreateNewAccountObject();
            Library.TrionServer.BeginCreateSecurityKey((ar) =>
            {
                try
                {
                    Library.TrionServer.EndCreateSecurityKey(ar);
                    account.Description = description;
                    App.SaveNewAccountObject(account);
                    Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                    });
                }
            }, null, account, deviceId);
        }

        private void ShowDeviceId_Click(object sender, EventArgs e)
        {
            var isHidden = DeviceId.Visibility == System.Windows.Visibility.Collapsed;
            var newVisibility = (isHidden ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
            DeviceIdLabel.Visibility = DeviceId.Visibility = newVisibility;
        }
    }
}