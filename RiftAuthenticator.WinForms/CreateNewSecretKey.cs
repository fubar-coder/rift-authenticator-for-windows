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
    public partial class CreateNewSecretKey : Form
    {
        public CreateNewSecretKey()
        {
            InitializeComponent();
        }

        private void CreateSecretKey_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static string CreateDeviceId()
        {
            return Guid.NewGuid().ToString().ToUpper().Replace("-", string.Empty);
        }

        private void CreateNewSecretKey_Load(object sender, EventArgs e)
        {
            DeviceId.Text = CreateDeviceId();
        }

        private void RecreateDeviceId_Click(object sender, EventArgs e)
        {
            DeviceId.Text = CreateDeviceId();
        }
    }
}
