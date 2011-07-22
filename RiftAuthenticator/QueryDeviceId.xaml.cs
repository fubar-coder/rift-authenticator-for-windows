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
    public partial class QueryDeviceId : Window
    {
        public QueryDeviceId()
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
            DeviceId.Focus();
        }
    }
}
