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
        Library.IAccountManager AccountManager;
        Library.IAccount Account;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateAccountList()
        {
            Accounts.SuspendLayout();
            try
            {
                Accounts.Items.Clear();
                var oldSelectedAccount = Account;
                foreach (var account in AccountManager)
                    Accounts.Items.Add(string.Format("{0} ({1})", account.Description, account.FormattedSerialKey));
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
            finally
            {
                Accounts.ResumeLayout();
            }
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

        private void MainWindow_Load(object sender, EventArgs e)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            SetPlatform(appSettings);
            SetAccountManager(appSettings);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = Library.TrionServer.CertificateIsValid;
            AccountManager.LoadAccounts();
            SetAccount(appSettings);
            UpdateAccountList();
        }

        void RefreshToken()
        {
            if (Account == null || Account.IsEmpty)
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

        private bool ExecuteInit()
        {
            var newAccount = CreateNewAccountObject();
            var dlg = new CreateNewSecretKey() { Owner = this };
            dlg.Description.Text = newAccount.Description;
            if (dlg.ShowDialog() != DialogResult.OK)
                return false;

            var deviceId = dlg.DeviceId.Text;
            Library.TrionServer.CreateSecurityKey(newAccount, deviceId);
            newAccount.TimeOffset = Library.TrionServer.GetTimeOffset();
            newAccount.Description = dlg.Description.Text;
            SaveNewAccountObject(newAccount);
            RefreshToken();
            return true;
        }

        private void ExecuteRecovery()
        {
            var newAccount = CreateNewAccountObject();
            var dlgDeviceId = new QueryDeviceId() { Owner = this };
            dlgDeviceId.Description.Text = newAccount.Description;
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
            Library.TrionServer.RecoverSecurityKey(newAccount, userEmail, userPassword, securityAnswers, deviceId);
            newAccount.TimeOffset = Library.TrionServer.GetTimeOffset();
            newAccount.Description = dlgDeviceId.Description.Text;
            SaveNewAccountObject(newAccount);
        }

        private void HelpAboutMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                this,
                Resources.Strings.MessageBox_Message_About,
                Resources.Strings.MessageBox_Title_About,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HelpLicenseMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                this,
                Resources.Strings.MessageBox_Message_License,
                Resources.Strings.MessageBox_Title_License,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FileQuitMenu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AccountInfoMenu_Click(object sender, EventArgs e)
        {
            var dlg = new Information(Account) { Owner = this };
            dlg.ShowDialog();
        }

        private void AccountCreateMenu_Click(object sender, EventArgs e)
        {
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

        private void AccountRecoverMenu_Click(object sender, EventArgs e)
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

        private void AccountExportMenu_Click(object sender, EventArgs e)
        {
            if (ExportAccountDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            var fileName = ExportAccountDialog.FileName;
            var map = Library.PlatformUtils.Android.AccountMap.GetMap(AccountManager, Account);
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                Library.PlatformUtils.Android.MapFile.WriteMap(stream, map);
        }

        private void AccountImportMenu_Click(object sender, EventArgs e)
        {
            if (ImportAccountDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            var fileName = ImportAccountDialog.FileName;

            Dictionary<string, object> map;
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                map = Library.PlatformUtils.Android.MapFile.ReadMap(stream);

            var newAccount = CreateNewAccountObject();
            Library.PlatformUtils.Android.AccountMap.SetMap(AccountManager, newAccount, map);
            SaveNewAccountObject(newAccount);
        }

        private void AccountManageMenu_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new Accounts(AccountManager, Account);
                if (dlg.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    return;
                AccountManager.SaveAccounts();
                UpdateAccountList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.Strings.MessageBox_Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                AccountManager.LoadAccounts();
                return;
            }
        }

        private void Accounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Accounts.SelectedIndex == -1)
            {
                Account = null;
            }
            else
            {
                Account = (Library.IAccount)AccountManager[Accounts.SelectedIndex];
            }
            RefreshToken();
        }
    }
}
