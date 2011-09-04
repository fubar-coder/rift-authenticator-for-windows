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
                    if (Library.TrionServer.Platform == null)
                        Library.TrionServer.Platform = new Library.Platform.WP7.Platform(userAgent);
                    if (AccountManager == null)
                    {
                        AccountManager = new Library.IsolatedStorage.AccountManager();
                        try
                        {
                            AccountManager.LoadAccounts();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                            AccountManager.Clear();
                        }
                    }
                    if (AccountManager.Count == 0)
                    {
                        StartNoConfigWizard();
                    }
                    SetAccount(null);
                    UpdateAccountList();
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

        private void UpdateAccountList()
        {
            var oldSelectedAccount = Account;
            Accounts.DataContext = null;
            Accounts.DataContext = AccountManager;
            if (AccountManager.Count == 0)
                AccountManager.Add(AccountManager.CreateAccount());
            int newSelectedAccountIndex = -1;
            if (oldSelectedAccount != null)
            {
                newSelectedAccountIndex = AccountManager.IndexOf(oldSelectedAccount);
            }
            if (newSelectedAccountIndex == -1)
                Accounts.SelectedIndex = 0;
            else
                Accounts.SelectedIndex = newSelectedAccountIndex;
            Accounts.Visibility = ((AccountManager.Count > 1) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
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

        private void AccountSync_Click(object sender, EventArgs e)
        {
            if (Account.IsEmpty)
                return;
            Library.TrionServer.BeginGetTimeOffset((ar) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        var timeOffset = Library.TrionServer.EndGetTimeOffset(ar);
                        Account.TimeOffset = timeOffset;
                        AccountManager.SaveAccounts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                    }
                });
            }, null);
        }

        private void AccountAdd_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CreateAuthenticator.xaml", UriKind.Relative));
        }
    }
}