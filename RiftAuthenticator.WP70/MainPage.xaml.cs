/*
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (App.ExitApp)
            {
                App.Quit();
            }
            else if (App.BackToMainPage)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    App.BackToMainPage = false;
                    base.OnNavigatedTo(e);
                }
            }
            else
            {
                InitAuthenticatorSystem();
                base.OnNavigatedTo(e);
            }
            InitAuthenticatorUI();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
#if !WP70
            ApplicationBar.Mode = Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
#endif
            if (ApplicationBar.Buttons.Count == 0)
            {
                var appBarButton = new Microsoft.Phone.Shell.ApplicationBarIconButton(new Uri("/Images/appbar.add.rest.png", UriKind.Relative))
                {
                    Text = WP7.Resources.AppResource.Add,
                };
                appBarButton.Click += AccountAdd_Click;
                ApplicationBar.Buttons.Add(appBarButton);

                appBarButton = new Microsoft.Phone.Shell.ApplicationBarIconButton(new Uri("/Images/appbar.download.rest.png", UriKind.Relative))
                {
                    Text = WP7.Resources.AppResource.CmdRecover,
                };
                appBarButton.Click += AccountRecover_Click;
                ApplicationBar.Buttons.Add(appBarButton);

                appBarButton = new Microsoft.Phone.Shell.ApplicationBarIconButton(new Uri("/Images/appbar.delete.rest.png", UriKind.Relative))
                {
                    Text = WP7.Resources.AppResource.Delete,
                };
                appBarButton.Click += AccountDelete_Click;
                ApplicationBar.Buttons.Add(appBarButton);

                appBarButton = new Microsoft.Phone.Shell.ApplicationBarIconButton(new Uri("/Images/appbar.edit.rest.png", UriKind.Relative))
                {
                    Text = WP7.Resources.AppResource.Edit,
                };
                appBarButton.Click += AccountEdit_Click;
                ApplicationBar.Buttons.Add(appBarButton);
            }
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
            UpdateAccountList();
        }

        private void InitAuthenticatorUI()
        {
            if (Account == null)
                SetAccount(null);
            UpdateAccountList();
            if (Timer == null)
            {
                Timer = new System.Windows.Threading.DispatcherTimer();
                Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                Timer.Tick += new EventHandler(Timer_Tick);
                Timer.Start();
            }
        }

        private bool InitAuthenticatorSystem()
        {
            // Use this fake UserAgent because HTTP requests are buggy when the WebBrowser control were instantiated
            // by this application
            return InitAuthenticatorSystem("Mozilla/4.0 (compatible: MSIE 7.0; Windows Phone OS 7.0; Trident/3.1; IEMobile/7.0; SAMSUNG; SGH-i917)");
        }

        private bool InitAuthenticatorSystem(string userAgent)
        {
            try
            {
                if (Library.TrionServer.Platform == null)
                    Library.TrionServer.Platform = new Library.Platform.WP7.Platform(userAgent);
                var newAccountManager = AccountManager == null;
                if (newAccountManager)
                {
                    AccountManager = new Library.IsolatedStorage.AccountManager();
                    try
                    {
                        AccountManager.LoadAccounts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, WP7.Resources.AppResource.MessageBoxTitleError, MessageBoxButton.OK);
                        AccountManager.Clear();
                    }
                }
                if (AccountManager.Count == 0)
                {
                    NavigationService.Navigate(new Uri("/NoConfigPage.xaml", UriKind.Relative));
                }
                return newAccountManager;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, WP7.Resources.AppResource.MessageBoxTitleError, MessageBoxButton.OK);
                return false;
            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                RefreshToken();
            });
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

        private void AccountAdd_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CreateAuthenticator.xaml", UriKind.Relative));
        }

        private void Accounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Accounts.SelectedIndex == -1)
            {
                Account = null;
            }
            else
            {
                Account = (Library.IAccount)Accounts.SelectedItem;
            }
            RefreshToken();
        }

        private void AccountRecover_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AuthenticatorDescription.xaml", UriKind.Relative));
        }

        private void AccountDelete_Click(object sender, EventArgs e)
        {
            if (Account == null || Account.IsEmpty)
                return;
            if (MessageBox.Show(WP7.Resources.AppResource.DeleteAuthenticatorQuestion, WP7.Resources.AppResource.MessageBoxTitleWarning, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;
            AccountManager.Remove(Account);
            AccountManager.SaveAccounts();
            SetAccount(null);
        }

        private void ExecSyncAll_Click(object sender, RoutedEventArgs e)
        {
            if (Account == null || Account.IsEmpty)
                return;
            App.ExecuteTimeSync(Dispatcher);
        }

        private void AccountEdit_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AuthenticatorDescription.xaml?action=edit", UriKind.Relative));
        }
    }
}