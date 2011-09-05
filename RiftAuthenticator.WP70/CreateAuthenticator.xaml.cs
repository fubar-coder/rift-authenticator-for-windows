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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
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
            
            AuthDescription.Text = App.CreateDefaultAccountDescription();
            DeviceId.Text = Library.TrionServer.GetOrCreateRandomDeviceId();
        }

        private void AuthCreate_Click(object sender, RoutedEventArgs e)
        {
            AuthCreate.IsEnabled = false;
            var deviceId = (string.IsNullOrEmpty(DeviceId.Text) ? Library.TrionServer.GetOrCreateRandomDeviceId() : DeviceId.Text);
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
            DeviceIdLabel.Visibility = DeviceId.Visibility = newVisibility;
        }
    }
}