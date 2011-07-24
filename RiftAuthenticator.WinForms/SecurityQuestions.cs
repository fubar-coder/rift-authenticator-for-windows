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
    public partial class SecurityQuestions : Form
    {
        public SecurityQuestions()
        {
            InitializeComponent();
        }

        private void DoRecovery_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
