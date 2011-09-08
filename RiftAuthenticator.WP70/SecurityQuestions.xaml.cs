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
    public partial class SecurityQuestions : PhoneApplicationPage
    {
        private List<KeyValuePair<TextBlock, TextBox>> SecurityQuestionPairs;

        public SecurityQuestions()
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

        private void InitPageUI()
        {
            SecurityQuestionPairs = new List<KeyValuePair<TextBlock, TextBox>>
            {
                new KeyValuePair<TextBlock, TextBox>(SecurityQuestion1, SecurityAnswer1),
                new KeyValuePair<TextBlock, TextBox>(SecurityQuestion2, SecurityAnswer2),
            };
            System.Diagnostics.Debug.Assert(App.AuthCreateSecurityQuestions != null);
            System.Diagnostics.Debug.Assert(App.AuthCreateSecurityQuestions.Length <= SecurityQuestionPairs.Count);
            for (int i = 0; i != App.AuthCreateSecurityQuestions.Length; ++i)
            {
                var securityQuestionPair = SecurityQuestionPairs[i];
                if (string.IsNullOrEmpty(App.AuthCreateSecurityQuestions[i]))
                {
                    securityQuestionPair.Key.Visibility =
                        securityQuestionPair.Value.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    securityQuestionPair.Key.Text = App.AuthCreateSecurityQuestions[i] ?? string.Empty;
                    securityQuestionPair.Value.Text = (App.AuthCreateSecurityAnswers == null ? string.Empty : (App.AuthCreateSecurityAnswers[i] ?? string.Empty));
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitPageUI();

            App.AuthCreateStep = 3;
        }

        private void StoreAuthSecurityQuestions()
        {
            var securityQuestionAnswers = new string[App.AuthCreateSecurityQuestions.Length];
            for (int i = 0; i != App.AuthCreateSecurityQuestions.Length; ++i)
            {
                if (!string.IsNullOrEmpty(App.AuthCreateSecurityQuestions[i]))
                {
                    securityQuestionAnswers[i] = SecurityQuestionPairs[i].Value.Text;
                }
            }
            App.AuthCreateSecurityAnswers = securityQuestionAnswers;
        }

        private void RecoverAuthenticator_Click(object sender, RoutedEventArgs e)
        {
            StoreAuthSecurityQuestions();
            RecoverAuthenticator.IsEnabled = false;
            var account = App.CreateNewAccountObject();
            try
            {
                Library.TrionServer.BeginRecoverSecurityKey((ar) =>
                {
                    try
                    {
                        Library.TrionServer.EndRecoverSecurityKey(ar);
                        account.Description = App.AuthCreateDescription;
                        App.AddNewAccountObject(account);
                        App.ExecuteTimeSync(Dispatcher, () =>
                        {
                            Dispatcher.BeginInvoke(() =>
                            {
                                App.AuthConfigReset();
                                App.ExitApp = false;
                                App.BackToMainPage = true;
                                NavigationService.GoBack();
                                //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                            });
                        });
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show(ex.Message, WP7.Resources.AppResource.MessageBoxTitleError, MessageBoxButton.OK);
                            RecoverAuthenticator.IsEnabled = true;
                        });
                    }
                }, null, account, App.AuthCreateUsername, App.AuthCreatePassword, App.AuthCreateSecurityAnswers, App.AuthCreateDeviceId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, WP7.Resources.AppResource.MessageBoxTitleError, MessageBoxButton.OK);
                RecoverAuthenticator.IsEnabled = true;
            }
        }

        private void SecurityAnswer1_TextChanged(object sender, TextChangedEventArgs e)
        {
            StoreAuthSecurityQuestions();
        }

        private void SecurityAnswer2_TextChanged(object sender, TextChangedEventArgs e)
        {
            StoreAuthSecurityQuestions();
        }
    }
}