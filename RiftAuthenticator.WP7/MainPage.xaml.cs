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
    public partial class MainPage : PhoneApplicationPage
    {
        private Library.IAccountManager AccountManager
        {
            get
            {
                return App.AccountManager;
            }
            set
            {
                App.AccountManager = value;
            }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();
            GetUserAgent();
        }

        private void GetUserAgent()
        {
            WebBrowserForUserAgent.ScriptNotify += (sender, e) =>
            {
                InitAuthenticatorStuff(e.Value);
            };
            var htmlCode =
@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"">
<html>
    <head>
        <script language=""JavaScript"" type=""text/JavaScript"">
            function printUserAgent() {
                window.external.notify(navigator.userAgent);
            }
        </script>
    </head>
    <body onload=""printUserAgent();"">
    </body>
</html>";
            WebBrowserForUserAgent.NavigateToString(htmlCode);
        }

        private void InitAuthenticatorStuff(string userAgent)
        {
            Library.TrionServer.Platform = new Library.Platform.WP7.Platform(userAgent);
            AccountManager = new Library.IsolatedStorage.AccountManager();
            if (AccountManager.Count == 0)
            {
                StartNoConfigWizard();
            }
        }

        private void StartNoConfigWizard()
        {
            NavigationService.Navigate(new Uri("/NoConfigPage.xaml", UriKind.Relative));
        }
    }
}