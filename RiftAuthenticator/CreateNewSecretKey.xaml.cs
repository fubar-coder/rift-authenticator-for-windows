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
    /// Interaktionslogik für CreateNewSecretKey.xaml
    /// </summary>
    public partial class CreateNewSecretKey : Window
    {
        public CreateNewSecretKey()
        {
            InitializeComponent();
        }

        private void CreateSecretKey_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DeviceId.Text = CreateDeviceId();
        }

        public static string CreateDeviceId()
        {
            return Guid.NewGuid().ToString().ToUpper().Replace("-", string.Empty);
        }

        private void RecreateDeviceId_Click(object sender, RoutedEventArgs e)
        {
            DeviceId.Text = CreateDeviceId();
        }
    }
}
