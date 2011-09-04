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
    public partial class NoConfigPage : PhoneApplicationPage
    {
        public NoConfigPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        private void AuthCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CreateAuthenticator.xaml", UriKind.Relative));
        }

        private void AuthRecover_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RecoverAuthenticator.xaml", UriKind.Relative));
        }
    }
}