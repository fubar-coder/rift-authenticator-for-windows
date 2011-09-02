/**
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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography.X509Certificates;

namespace RiftAuthenticator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Library.IAccountManager AccountManager = new Library.Registry.AccountManager();
        Library.IAccount Account;
        System.Windows.Threading.DispatcherTimer Timer;

        public MainWindow()
        {
            InitializeComponent();
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
        }

        private void SetPlatform(System.Collections.Specialized.NameValueCollection appSettings)
        {
            var platformId = appSettings["default-platform"];
            if (string.IsNullOrEmpty(platformId))
                platformId = "windows";
            Library.TrionServer.Platform = Library.PlatformBase.LoadPlatform(platformId);
        }

        private void SetAccountManager(System.Collections.Specialized.NameValueCollection appSettings)
        {
            var accountManagerId = appSettings["default-account-manager"];
            if (string.IsNullOrEmpty(accountManagerId))
                accountManagerId = "registry";
            AccountManager = Library.AccountManagerBase.LoadAccountManager(accountManagerId);
        }

        private void SetAccount(System.Collections.Specialized.NameValueCollection appSettings)
        {
            if (AccountManager.Count == 0)
                AccountManager.Add(AccountManager.CreateAccount());
            var accountId = appSettings["default-account"];
            if (!string.IsNullOrEmpty(accountId))
                Account = AccountManager.FindAccount(accountId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            SetPlatform(appSettings);
            SetAccountManager(appSettings);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = Library.TrionServer.CertificateIsValid;
            AccountManager.LoadAccounts();
            SetAccount(appSettings);
            UpdateAccountList();
            StartWizardIfConfigEmpty();
            Timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.ApplicationIdle, Dispatcher);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }

        private bool IsConfigEmpty
        {
            get
            {
                return (AccountManager.Count == 0)
                    || (AccountManager.Count == 1 && AccountManager[0].IsEmpty);
            }
        }

        private void StartWizardIfConfigEmpty()
        {
            if (!IsConfigEmpty)
                return;
            var dlg = new FirstAppStart()
            {
                Owner = this,
            };
            if (!dlg.ShowDialog().GetValueOrDefault())
                return;
            try
            {
                if (dlg.CreateNewAuth.IsChecked.GetValueOrDefault())
                {
                    ExecuteInitWithClipboard();
                }
                else if (dlg.RecoverOldAuth.IsChecked.GetValueOrDefault())
                {
                    ExecuteRecovery();
                }
                else
                {
                    // Nothing to do...
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, App.Localization.Get("MessageBox.Title.Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        void RefreshToken()
        {
            if (Account == null || Account.IsEmpty)
            {
                LoginToken.Text = App.Localization.Get("Status.NoConfig");
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

        void Timer_Tick(object sender, EventArgs e)
        {
            if (Account == null || Account.IsEmpty)
                return;
            RefreshToken();
        }

        private void TimeSync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExecuteTimeSync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, App.Localization.Get("MessageBox.Title.Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ExecuteTimeSync()
        {
            Account.TimeOffset = Library.TrionServer.GetTimeOffset();
            AccountManager.SaveAccounts();
            UpdateAccountList();
        }

        private Library.IAccount CreateNewAccountObject()
        {
            Library.IAccount newAccount;
            if (Account.IsEmpty)
            {
                newAccount = Account;
            }
            else
            {
                newAccount = AccountManager.CreateAccount();
            }
            return newAccount;
        }

        private void SaveNewAccountObject(Library.IAccount newAccount)
        {
            if (newAccount != Account)
            {
                AccountManager.Add(newAccount);
                Account = newAccount;
            }
            AccountManager.SaveAccounts();
            UpdateAccountList();
        }

        private bool ExecuteInitWithClipboard()
        {
            var result = ExecuteInit();
            if (result)
            {
                Clipboard.SetText(Account.DeviceId);
                MessageBox.Show(this, string.Format(App.Localization.Get("MessageBox.Message.RememberDeviceId"), Account.DeviceId), "Remember you device id", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return result;
        }

        private bool ExecuteInit()
        {
            var newAccount = CreateNewAccountObject();
            var dlg = new CreateNewSecretKey() { Owner = this };
            dlg.Description.Text = newAccount.Description;
            if (!dlg.ShowDialog().GetValueOrDefault())
                return false;

            var deviceId = dlg.DeviceId.Text;
            Library.TrionServer.CreateSecurityKey(newAccount, deviceId);
            newAccount.TimeOffset = Library.TrionServer.GetTimeOffset();
            newAccount.Description = dlg.Description.Text;
            SaveNewAccountObject(newAccount);
            AccountManager.SaveAccounts();
            RefreshToken();
            return true;
        }

        private void ExecuteRecovery()
        {
            var newAccount = CreateNewAccountObject();
            var dlgDeviceId = new QueryDeviceId() { Owner = this };
            dlgDeviceId.Description.Text = newAccount.Description;
            dlgDeviceId.DeviceId.Text = Library.TrionServer.GetDeviceId();
            if (!dlgDeviceId.ShowDialog().GetValueOrDefault())
                return;
            var deviceId = dlgDeviceId.DeviceId.Text.Trim();
            if (string.IsNullOrEmpty(deviceId))
            {
                MessageBox.Show(this, App.Localization.Get("MessageBox.Message.NoDeviceId"), App.Localization.Get("MessageBox.Title.UserInputError"), MessageBoxButton.OK);
                return;
            }
            var dlgLogin = new Login() { Owner = this };
            if (!dlgLogin.ShowDialog().GetValueOrDefault())
                return;

            var userEmail = dlgLogin.Email.Text;
            var userPassword = dlgLogin.Password.Password;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
            {
                MessageBox.Show(this, App.Localization.Get("MessageBox.Message.LoginInfoIncomplete"), App.Localization.Get("MessageBox.Title.UserInputError"), MessageBoxButton.OK);
                return;
            }

            var questions = Library.TrionServer.GetSecurityQuestions(userEmail, userPassword);

            var dlgSecurityQuestions = new SecurityQuestions() { Owner = this };
            dlgSecurityQuestions.SecurityAnswer1.IsEnabled =
                dlgSecurityQuestions.SecurityQuestion1.IsEnabled = !string.IsNullOrEmpty(questions[0]);
            dlgSecurityQuestions.SecurityAnswer2.IsEnabled =
                dlgSecurityQuestions.SecurityQuestion2.IsEnabled = !string.IsNullOrEmpty(questions[1]);
            dlgSecurityQuestions.SecurityQuestion1.Text = questions[0];
            dlgSecurityQuestions.SecurityQuestion2.Text = questions[1];
            if (!string.IsNullOrEmpty(questions[0]))
            {
                dlgSecurityQuestions.SecurityAnswer1.Focus();
            }
            else if (!string.IsNullOrEmpty(questions[1]))
            {
                dlgSecurityQuestions.SecurityAnswer2.Focus();
            }
            if (!dlgSecurityQuestions.ShowDialog().GetValueOrDefault())
                return;

            var securityAnswers = new string[2] {
                dlgSecurityQuestions.SecurityAnswer1.Text,
                dlgSecurityQuestions.SecurityAnswer2.Text,
            };
            Library.TrionServer.RecoverSecurityKey(newAccount, userEmail, userPassword, securityAnswers, deviceId);
            newAccount.TimeOffset = Library.TrionServer.GetTimeOffset();
            newAccount.Description = dlgDeviceId.Description.Text;
            SaveNewAccountObject(newAccount);
        }

        private void HelpLicenseMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this,
                App.Localization.Get("MessageBox.Message.License"),
                App.Localization.Get("MessageBox.Title.License"),
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void FileQuitMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AccountCreateMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExecuteInitWithClipboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, App.Localization.Get("MessageBox.Title.Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void AccountRecoverMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExecuteRecovery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, App.Localization.Get("MessageBox.Title.Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void AccountManageMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Accounts(AccountManager, Account)
                {
                    Owner = this,
                };
                if (!dlg.ShowDialog().GetValueOrDefault())
                    return;
                AccountManager.SaveAccounts();
                UpdateAccountList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, App.Localization.Get("MessageBox.Title.Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                AccountManager.LoadAccounts();
                return;
            }
        }

        private void AccountInformationMenu_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Information(Account) { Owner = this };
            dlg.ShowDialog();
        }

        private void HelpAboutMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                this,
                App.Localization.Get("MessageBox.Message.About"),
                App.Localization.Get("MessageBox.Title.About"),
                MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void AccountExportMenu_Click(object sender, RoutedEventArgs e)
        {
            var ExportAccountDialog = new Microsoft.Win32.SaveFileDialog()
            {
                CheckPathExists = true,
                DefaultExt = "xml",
                Filter = App.Localization.Get("MainWindow.ExportAccountDialog.Filter"),
            };
            if (!ExportAccountDialog.ShowDialog(this).GetValueOrDefault())
                return;
            var fileName = ExportAccountDialog.FileName;
            var map = Library.PlatformUtils.Android.AccountMap.GetMap(AccountManager, Account);
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                Library.PlatformUtils.Android.MapFile.WriteMap(stream, map);
        }

        private void AccountImportMenu_Click(object sender, RoutedEventArgs e)
        {
            var ImportAccountDialog = new Microsoft.Win32.OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xml",
                Filter = App.Localization.Get("MainWindow.ExportAccountDialog.Filter"),
            };
            if (!ImportAccountDialog.ShowDialog(this).GetValueOrDefault())
                return;
            var fileName = ImportAccountDialog.FileName;

            Dictionary<string, object> map;
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                map = Library.PlatformUtils.Android.MapFile.ReadMap(stream);

            var newAccount = CreateNewAccountObject();
            Library.PlatformUtils.Android.AccountMap.SetMap(AccountManager, newAccount, map);
            SaveNewAccountObject(newAccount);
        }
    }
}
