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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SecurityQuestionPairs = new List<KeyValuePair<TextBlock, TextBox>>
            {
                new KeyValuePair<TextBlock, TextBox>(SecurityQuestion1, SecurityAnswer1),
                new KeyValuePair<TextBlock, TextBox>(SecurityQuestion2, SecurityAnswer2),
            };
            System.Diagnostics.Debug.Assert(App.SecurityQuestions != null);
            System.Diagnostics.Debug.Assert(App.SecurityQuestions.Length <= SecurityQuestionPairs.Count);
            for (int i = 0; i != App.SecurityQuestions.Length; ++i)
            {
                var securityQuestionPair = SecurityQuestionPairs[i];
                if (string.IsNullOrEmpty(App.SecurityQuestions[i]))
                {
                    securityQuestionPair.Key.Visibility =
                        securityQuestionPair.Value.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    securityQuestionPair.Key.Text = App.SecurityQuestions[i];
                    securityQuestionPair.Value.Text = string.Empty;
                }
            }
        }

        private void RecoverAuthenticator_Click(object sender, RoutedEventArgs e)
        {
            var securityQuestionAnswers = new string[App.SecurityQuestions.Length];
            for (int i = 0; i != App.SecurityQuestions.Length; ++i)
            {
                if (!string.IsNullOrEmpty(App.SecurityQuestions[i]))
                {
                    securityQuestionAnswers[i] = SecurityQuestionPairs[i].Value.Text;
                }
            }
            var account = App.CreateNewAccountObject();
            try
            {
                Library.TrionServer.BeginRecoverSecurityKey((ar) =>
                {
                    try
                    {
                        Library.TrionServer.EndRecoverSecurityKey(ar);
                        App.SaveNewAccountObject(account);
                        Dispatcher.BeginInvoke(() =>
                        {
                            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        });
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                        });
                    }
                }, null, account, App.AuthCreateUsername, App.AuthCreatePassword, securityQuestionAnswers, App.AuthCreateDeviceId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                return;
            }
        }
    }
}