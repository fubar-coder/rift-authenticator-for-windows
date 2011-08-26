﻿using System;
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
        class SelectionInformation
        {
            public List<Library.AccountProxy> Selected;
            public List<Library.AccountProxy> PreSelected;
            public List<Library.AccountProxy> PostSelected;
        }

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

        private SelectionInformation GetSelectionInformation()
        {
            var result = new SelectionInformation()
            {
                Selected = new List<Library.AccountProxy>(),
                PreSelected = new List<Library.AccountProxy>(),
                PostSelected = new List<Library.AccountProxy>(),
            };

            var selectionFound = false;
            foreach (DataGridViewRow row in AccountGrid.Rows)
            {
                var item = (Library.AccountProxy)row.DataBoundItem;
                if (row.Selected)
                {
                    selectionFound = true;
                    result.Selected.Add(item);
                } 
                else if (!selectionFound)
                {
                    result.PreSelected.Add(item);
                }
                else 
                {
                    result.PostSelected.Add(item);
                }
            }

            return result;
        }

        private void MoceAccountUp_Click(object sender, EventArgs e)
        {
            var selectionInfo = GetSelectionInformation();
            var newList = new List<Library.AccountProxy>();
            if (selectionInfo.PreSelected.Count != 0)
                for (int i = 0; i != selectionInfo.PreSelected.Count - 1; ++i)
                    newList.Add(selectionInfo.PreSelected[i]);
            newList.AddRange(selectionInfo.Selected);
            if (selectionInfo.PreSelected.Count != 0)
                newList.Add(selectionInfo.PreSelected[selectionInfo.PreSelected.Count - 1]);
            newList.AddRange(selectionInfo.PostSelected);
            AccountGrid.DataSource = newList;
            ReselectAccounts(selectionInfo.Selected);
        }

        private void MoveAccountDown_Click(object sender, EventArgs e)
        {
            var selectionInfo = GetSelectionInformation();
            var newList = new List<Library.AccountProxy>();
            newList.AddRange(selectionInfo.PreSelected);
            if (selectionInfo.PostSelected.Count != 0)
                newList.Add(selectionInfo.PostSelected[0]);
            newList.AddRange(selectionInfo.Selected);
            if (selectionInfo.PostSelected.Count != 0)
                for (int i = 1; i != selectionInfo.PostSelected.Count; ++i)
                    newList.Add(selectionInfo.PostSelected[i]);
            AccountGrid.DataSource = newList;
            ReselectAccounts(selectionInfo.Selected);
        }

        private void ReselectAccounts(List<Library.AccountProxy> selectedAccounts)
        {
            foreach (DataGridViewRow row in AccountGrid.Rows)
            {
                var item = (Library.AccountProxy)row.DataBoundItem;
                if (selectedAccounts.Contains(item))
                    row.Selected = true;
            }
        }

        private void ApplyAccountChanges_Click(object sender, EventArgs e)
        {
#if FALSE
            var accountToProxyMap = new Dictionary<Library.IAccount, Library.AccountProxy>();
            var addedAccounts = new List<Library.IAccount>();
            foreach (DataGridViewRow row in AccountGrid.Rows)
            {
                var item = (Library.AccountProxy)row.DataBoundItem;
                accountToProxyMap.Add(item.OriginalAccount, item);
                if (!AccountManager.Contains(item))
                    addedAccounts.Add(item);
            }

            var deletedAccounts = new List<Library.IAccount>();
            foreach (var account in AccountManager)
            {
                if (!accountToProxyMap.ContainsKey(account))
                    deletedAccounts.Add(account);
            }

            var originalAccounts = new List<Library.IAccount>(AccountManager);
#endif
            AccountManager.Clear();
            foreach (DataGridViewRow row in AccountGrid.Rows)
            {
                var item = (Library.AccountProxy)row.DataBoundItem;
                AccountManager.Add(item);
            }
            AccountManager.SaveAccounts();
        }
    }
}
