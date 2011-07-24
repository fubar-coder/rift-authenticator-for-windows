using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RiftAuthenticator.WinForms
{
    public partial class Information : Form
    {
        private Library.Configuration Configuration;

        public Information(Library.Configuration config)
        {
            InitializeComponent();

            Configuration = config;
            ConfigToControls();
        }

        private void ConfigToControls()
        {
            DeviceId.Text = Configuration.DeviceId;
            SerialKey.Text = Configuration.FormattedSerialKey;
            SecretKey.Text = Configuration.SecretKey;
            TimeOffset.Text = Configuration.TimeOffset.ToString();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
