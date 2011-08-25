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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RiftAuthenticator.WinForms
{
    public partial class MainWindow : Form
    {
        Library.IAccountManager AccountManager = new Library.Registry.AccountManager();
        Library.IAccount Account;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = Library.TrionServer.CertificateIsValid;
            AccountManager.LoadAccounts();
            if (AccountManager.Count == 0)
                AccountManager.Add(AccountManager.CreateAccount());
            Account = AccountManager[0];
            RefreshToken();
        }

        void RefreshToken()
        {
            if (Account.IsEmpty)
            {
                LoginToken.Text = Resources.Strings.Status_NoConfig;
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
                RemainingValidTime.Value = (int)loginToken.RemainingMillis;
            }
        }

        private void TokenUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (Account.IsEmpty)
                return;
            RefreshToken();
        }

        private void TimeSync_Click(object sender, EventArgs e)
        {
            try
            {
                ExecuteTimeSync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.Strings.MessageBox_Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ExecuteTimeSync()
        {
            Account.TimeOffset = Library.TrionServer.GetTimeOffset();
            AccountManager.SaveAccounts();
            RefreshToken();
        }

        private void ShowLicense_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                this, 
                Resources.Strings.MessageBox_Message_License,
                Resources.Strings.MessageBox_Title_License, 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Information_Click(object sender, EventArgs e)
        {
            var dlg = new Information(Account) { Owner = this };
            dlg.ShowDialog();
        }

        private void Initialize_Click(object sender, EventArgs e)
        {
            if (!Account.IsEmpty)
            {
                if (MessageBox.Show(
                    this, 
                    Resources.Strings.MessageBox_Message_AlreadyInitialized, 
                    Resources.Strings.MessageBox_Title_Warning, 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    return;
            }

            try
            {
                if (ExecuteInit())
                {
                    Clipboard.SetText(Account.DeviceId);
                    MessageBox.Show(this,
                        string.Format(Resources.Strings.MessageBox_Message_RememberDeviceId, Account.DeviceId), Resources.Strings.MessageBox_Title_RememberDeviceId, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.Strings.MessageBox_Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private bool ExecuteInit()
        {
            var dlg = new CreateNewSecretKey() { Owner = this };
            if (dlg.ShowDialog() != DialogResult.OK)
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
            if (dlgDeviceId.ShowDialog() != DialogResult.OK)
                return;
            var deviceId = dlgDeviceId.DeviceId.Text.Trim();
            if (string.IsNullOrEmpty(deviceId))
            {
                MessageBox.Show(this, Resources.Strings.MessageBox_Message_NoDeviceId, Resources.Strings.MessageBox_Title_UserInputError, MessageBoxButtons.OK);
                return;
            }
            var dlgLogin = new Login() { Owner = this };
            if (dlgLogin.ShowDialog() != DialogResult.OK)
                return;

            var userEmail = dlgLogin.Email.Text;
            var userPassword = dlgLogin.Password.Text;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
            {
                MessageBox.Show(this, Resources.Strings.MessageBox_Message_LoginInfoIncomplete, Resources.Strings.MessageBox_Title_UserInputError, MessageBoxButtons.OK);
                return;
            }

            var questions = Library.TrionServer.GetSecurityQuestions(userEmail, userPassword);

            var dlgSecurityQuestions = new SecurityQuestions() { Owner = this };
            dlgSecurityQuestions.SecurityAnswer1.Enabled =
                dlgSecurityQuestions.SecurityQuestion1.Enabled = !string.IsNullOrEmpty(questions[0]);
            dlgSecurityQuestions.SecurityAnswer2.Enabled =
                dlgSecurityQuestions.SecurityQuestion2.Enabled = !string.IsNullOrEmpty(questions[1]);
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
            if (dlgSecurityQuestions.ShowDialog() != System.Windows.Forms.DialogResult.OK)
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

        private void Recover_Click(object sender, EventArgs e)
        {
            try
            {
                ExecuteRecovery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.Strings.MessageBox_Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
