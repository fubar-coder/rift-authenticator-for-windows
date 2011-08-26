using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RiftAuthenticator.WinForms
{
    public partial class Accounts : Form
    {

        public Library.IAccountManager AccountManager { get; private set; }
        public Library.IAccount ActiveAccount { get; private set; }

        public Accounts(Library.IAccountManager accountManager, Library.IAccount activeAccount)
        {
            AccountManager = accountManager;
            ActiveAccount = activeAccount;

            InitializeComponent();

            var accounts = new List<Library.AccountProxy>();
            foreach (var account in AccountManager)
                accounts.Add(new Library.AccountProxy(account));

            AccountGrid.DataSource = accounts;
        }
    }
}
