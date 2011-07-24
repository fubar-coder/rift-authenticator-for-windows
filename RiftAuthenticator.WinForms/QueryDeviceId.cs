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
    public partial class QueryDeviceId : Form
    {
        public QueryDeviceId()
        {
            InitializeComponent();
        }

        private void CreateSecretKey_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
