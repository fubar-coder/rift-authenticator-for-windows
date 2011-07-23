/**
 * This file is part of RIFT Authenticator for Windows.
 *
 * RIFT Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT Authenticator for Windows.  If not, see 
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
        Configuration Configuration = new Configuration();
        System.Windows.Threading.DispatcherTimer Timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = CertValidation;
            Configuration.Load();
            RefreshToken();
            Timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.ApplicationIdle, Dispatcher);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }

        bool CertValidation(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
                return false;
            var cert = certificate as X509Certificate2;
            var hostName = cert.GetNameInfo(X509NameType.DnsName, false);
            var validTrionHostNames = new string[] {
                ".trionworlds.com",
                ".triongames.com",
                ".trionworld.priv",
                ".triongames.priv",
            };
            foreach (var validTrionHostName in validTrionHostNames)
            {
                if (hostName.EndsWith(validTrionHostName))
                    return true;
            }
            return false;
        }

        void RefreshToken()
        {
            if (Configuration.IsEmpty)
            {
                LoginToken.Text = "No configuration. Initialization required.";
                SerialKey.Text = string.Empty;
                RemainingValidTime.Value = 0;
            }
            else
            {
                if (SerialKey.Text != Configuration.FormattedSerialKey)
                    SerialKey.Text = Configuration.FormattedSerialKey;
                var loginToken = Configuration.CalculateToken();
                if (LoginToken.Text != loginToken.Token)
                    LoginToken.Text = loginToken.Token;
                RemainingValidTime.Value = loginToken.RemainingMillis;
            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (Configuration.IsEmpty)
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ExecuteTimeSync()
        {
            Configuration.TimeOffset = TrionServer.GetTimeOffset();
            Configuration.Save();
            RefreshToken();
        }

        private void Information_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Information(Configuration) { Owner = this };
            dlg.ShowDialog();
        }

        private void Initialize_Click(object sender, RoutedEventArgs e)
        {
            if (!Configuration.IsEmpty)
            {
                if (MessageBox.Show("Authenticator already initialized!\nReinitialization will make you loose your current configuration!\nContinue?", "R U sure?", MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
                    return;
            }

            try
            {
                if (ExecuteInit())
                {
                    Clipboard.SetText(Configuration.DeviceId);
                    MessageBox.Show(string.Format("The device id has been copied into your clipboard.\nSave it!\nIt's required to use the restore functionality!\nThe device id is {0}", Configuration.DeviceId), "Remember you device id", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool ExecuteInit()
        {
            var dlg = new CreateNewSecretKey() { Owner = this };
            if (!dlg.ShowDialog().GetValueOrDefault())
                return false;

            var tempConfig = new Configuration()
            {
                DeviceId = dlg.DeviceId.Text,
            };
            TrionServer.CreateSecurityKey(tempConfig);
            tempConfig.TimeOffset = TrionServer.GetTimeOffset();
            tempConfig.Save();
            Configuration.Load();
            RefreshToken();
            return true;
        }

        private void ExecuteRecovery()
        {
            var dlgDeviceId = new QueryDeviceId() { Owner = this };
            if (!dlgDeviceId.ShowDialog().GetValueOrDefault())
                return;
            var deviceId = dlgDeviceId.DeviceId.Text.Trim();
            if (string.IsNullOrEmpty(deviceId))
            {
                MessageBox.Show("No device ID given.", "Bad boy!", MessageBoxButton.OK);
                return;
            }
            var dlgLogin = new Login() { Owner = this };
            if (!dlgLogin.ShowDialog().GetValueOrDefault())
                return;

            var userEmail = dlgLogin.Email.Text;
            var userPassword = dlgLogin.Password.Password;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
            {
                MessageBox.Show("No user name or password given.", "Bad boy!", MessageBoxButton.OK);
                return;
            }

            var questions = TrionServer.GetSecurityQuestions(userEmail, userPassword);

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
            var tempConfig = new Configuration()
            {
                DeviceId = deviceId,
            };
            TrionServer.RecoverSecurityKey(userEmail, userPassword, securityAnswers, tempConfig);
            tempConfig.TimeOffset = TrionServer.GetTimeOffset();
            tempConfig.Save();
            Configuration.Load();
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ShowLicense_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "This application is distributed under the terms of GNU General Public License Version 3.\n" +
                "The application is under copyright of the RIFT Authenticator for Windows project except for " +
                "some parts which are under the copyright of the following companies/projects:\n" +
                "Novell\n" +
                "The Apache Software Foundation", "License stuff", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
