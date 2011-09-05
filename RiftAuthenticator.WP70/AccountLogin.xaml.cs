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