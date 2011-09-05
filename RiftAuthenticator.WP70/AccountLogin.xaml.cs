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
    public partial class AccountLogin : PhoneApplicationPage
    {
        public AccountLogin()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (App.BackToMainPage)
            {
                NavigationService.GoBack();
            }
            else
            {
                base.OnNavigatedTo(e);
            }
        }

        private void QuerySecurityQuestions_Click(object sender, RoutedEventArgs e)
        {
            App.AuthCreateUsername = UserName.Text;
            App.AuthCreatePassword = Password.Password;

            Library.TrionServer.BeginGetSecurityQuestions((ar) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        App.SecurityQuestions = Library.TrionServer.EndGetSecurityQuestions(ar);
                        NavigationService.Navigate(new Uri("/SecurityQuestions.xaml", UriKind.Relative));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, WP7.Resources.AppResource.MessageBoxTitleError, MessageBoxButton.OK);
                    }
                });
            }, null, App.AuthCreateUsername, App.AuthCreatePassword);
        }
    }
}