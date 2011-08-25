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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = Library.TrionServer.CertificateIsValid;
            AccountManager.LoadAccounts();
            if (AccountManager.Count == 0)
                AccountManager.Add(AccountManager.CreateAccount());
            Account = AccountManager[0];
            RefreshToken();
            Timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.ApplicationIdle, Dispatcher);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }

        void RefreshToken()
        {
            if (Account.IsEmpty)
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
            if (Account.IsEmpty)
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
            RefreshToken();
        }

        private void Information_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Information(Account) { Owner = this };
            dlg.ShowDialog();
        }

        private void Initialize_Click(object sender, RoutedEventArgs e)
        {
            if (!Account.IsEmpty)
            {
                if (MessageBox.Show(this, App.Localization.Get("MessageBox.Message.AlreadyInitialized"), App.Localization.Get("MessageBox.Title.Warning"), MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
                    return;
            }

            try
            {
                if (ExecuteInit())
                {
                    Clipboard.SetText(Account.DeviceId);
                    MessageBox.Show(this, string.Format(App.Localization.Get("MessageBox.Message.RememberDeviceId"), Account.DeviceId), "Remember you device id", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, App.Localization.Get("MessageBox.Title.Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool ExecuteInit()
        {
            var dlg = new CreateNewSecretKey() { Owner = this };
            if (!dlg.ShowDialog().GetValueOrDefault())
                return false;

            var deviceId = dlg.DeviceId.Text;
            Library.TrionServer.CreateSecurityKey(Account, deviceId);
            Account.TimeOffset = Library.TrionServer.GetTimeOffset();
            AccountManager.SaveAccounts();
            RefreshToken();
            return true;
        }

        private void ExecuteRecovery()
        {
            var dlgDeviceId = new QueryDeviceId() { Owner = this };
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
            Library.TrionServer.RecoverSecurityKey(Account, userEmail, userPassword, securityAnswers, deviceId);
            Account.TimeOffset = Library.TrionServer.GetTimeOffset();
            AccountManager.SaveAccounts();
            RefreshToken();
        }

        private void Recover_Click(object sender, RoutedEventArgs e)
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

        private void ShowLicense_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this,
                App.Localization.Get("MessageBox.Message.License"),
                App.Localization.Get("MessageBox.Title.License"),
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
