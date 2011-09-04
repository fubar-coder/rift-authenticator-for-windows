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

        private Library.IAccount Account
        {
            get
            {
                return App.Account;
            }
            set
            {
                App.Account = value;
            }
        }

        private System.Windows.Threading.DispatcherTimer Timer;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();
            // Use this fake UserAgent because HTTP requests are buggy when the WebBrowser control were instantiated
            // by this application
            InitAuthenticatorStuff("Mozilla/4.0 (compatible: MSIE 7.0; Windows Phone OS 7.0; Trident/3.1; IEMobile/7.0; SAMSUNG; SGH-i917)");
        }

        private void SetAccount(string accountId)
        {
            if (AccountManager.Count == 0)
                AccountManager.Add(AccountManager.CreateAccount());
            if (!string.IsNullOrEmpty(accountId))
            {
                Account = AccountManager.FindAccount(accountId);
            }
            else
            {
                Account = AccountManager[0];
            }
        }

        private void InitAuthenticatorStuff(string userAgent)
        {
            Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    Library.TrionServer.Platform = new Library.Platform.WP7.Platform(userAgent);
                    AccountManager = new Library.IsolatedStorage.AccountManager();
                    try
                    {
                        //AccountManager.LoadAccounts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                        AccountManager.Clear();
                    }
                    if (AccountManager.Count == 0)
                    {
                        StartNoConfigWizard();
                    }
                    SetAccount(null);
                    Timer = new System.Windows.Threading.DispatcherTimer();
                    Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                    Timer.Tick += new EventHandler(Timer_Tick);
                    Timer.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                }
            });
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                RefreshToken();
            });
        }

        private void StartNoConfigWizard()
        {
            NavigationService.Navigate(new Uri("/NoConfigPage.xaml", UriKind.Relative));
        }

        void RefreshToken()
        {
            if (Account == null || Account.IsEmpty)
            {
                LoginToken.Text = string.Empty;
                SerialKey.Text = string.Empty;
                RemainingValidTime.Value = 0;
            }
            else
            {
                if (SerialKey.Text != Account.FormattedSerialKey)
                    SerialKey.Text = Account.FormattedSerialKey;
                var loginToken = Account.CalculateToken();
                if (LoginToken.Text != loginToken.Token)
                    LoginToken.Text = loginToken.Token;
                RemainingValidTime.Value = loginToken.RemainingMillis;
            }
        }
    }
}