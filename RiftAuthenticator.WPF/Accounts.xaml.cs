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
using System.Windows.Shapes;

namespace RiftAuthenticator
{
    /// <summary>
    /// Interaktionslogik für Accounts.xaml
    /// </summary>
    public partial class Accounts : Window
    {
        class SelectionInformation
        {
            public List<Library.AccountProxy> Selected;
            public List<Library.AccountProxy> PreSelected;
            public List<Library.AccountProxy> PostSelected;
        }

        public Library.IAccountManager AccountManager { get; private set; }
        public Library.IAccount ActiveAccount { get; private set; }
        private ListCollectionView AccountView { get; set; }

        public Accounts(Library.IAccountManager accountManager, Library.IAccount activeAccount)
        {
            AccountManager = accountManager;
            ActiveAccount = activeAccount;

            InitializeComponent();

            SetDataContext(new List<Library.AccountProxy>(AccountManager.Select(x => new Library.AccountProxy(x))));
        }

        private void SetDataContext(List<Library.AccountProxy> accounts)
        {
            AccountView = new ListCollectionView(accounts);
            DataContext = AccountView;
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
            var selectedItems = AccountGrid.SelectedItems.Cast<Library.AccountProxy>().ToDictionary(x => x);
            if (AccountView.MoveCurrentToFirst())
            {
                do
                {
                    var item = (Library.AccountProxy)AccountView.CurrentItem;
                    if (selectedItems.ContainsKey(item))
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
                } while (AccountView.MoveCurrentToNext());
            }

            return result;
        }

        private void MoveAccountUp_Click(object sender, RoutedEventArgs e)
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
            SetDataContext(newList);
            ReselectAccounts(selectionInfo.Selected);
        }

        private void MoveAccountDown_Click(object sender, RoutedEventArgs e)
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
            SetDataContext(newList);
            ReselectAccounts(selectionInfo.Selected);
        }

        private void ReselectAccounts(List<Library.AccountProxy> selectedAccounts)
        {
            AccountGrid.SelectedItems.Clear();
            foreach (var item in selectedAccounts)
                AccountGrid.SelectedItems.Add(item);
        }

        private void ApplyAccountChanges_Click(object sender, RoutedEventArgs e)
        {
            var accounts = new List<Library.IAccount>();
            var index = 0;
            if (AccountView.MoveCurrentToFirst())
            {
                do
                {
                    var item = (Library.AccountProxy)AccountView.CurrentItem;
                    item.Save(AccountManager, index++);
                    accounts.Add(item.OriginalAccount);
                } while (AccountView.MoveCurrentToNext());
            }
            AccountManager.Clear();
            foreach (var account in accounts)
                AccountManager.Add(account);

            DialogResult = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var col in AccountGrid.Columns)
                col.Header = App.Localization.Get(col.Header.ToString());
        }
    }
}
